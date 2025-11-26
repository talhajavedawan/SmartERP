using ERP_BL.Data;
using ERP_BL.Entities.Company_Center.Companies;
using ERP_BL.Entities.CompanyCenter.Companies;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ERP_REPO.Repo.Company_Center.Companies
{
    public interface IGroupRepo
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync(string status = "all");
        Task<Group?> GetGroupByIdAsync(int id);
        Task<Group> AddGroupAsync(Group group, ClaimsPrincipal user);
        Task<Group?> UpdateGroupAsync(int id, Group group, ClaimsPrincipal user);
        Task<bool> DeactivateGroupAsync(int id);
    }

    public class GroupService : IGroupRepo
    {
        private readonly ApplicationDbContext _context;

        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync(string status = "all")
        {
            var query = _context.Groups
                .Include(g => g.Companies)
                .Include(g => g.CreatedBy)
                .Include(g => g.LastModifiedBy)
                .AsQueryable();

            status = status?.Trim().ToLower() ?? "all";

            query = status switch
            {
                "active" => query.Where(g => g.IsActive),
                "inactive" => query.Where(g => !g.IsActive),
                _ => query
            };

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Companies)
                .Include(g => g.CreatedBy)
                .Include(g => g.LastModifiedBy)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Group> AddGroupAsync(Group group, ClaimsPrincipal user)
        {
            var (userId, _) = GetUserInfo(user);

            await ValidateCompaniesNotInOtherGroups(group.Companies);

            group.CreatedById = userId;
            group.CreationDate = DateTime.UtcNow;
            group.LastModifiedById = null;
            group.LastModified = null;
            group.IsActive = true;

            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            return group;
        }

        // ✅ Update existing group safely
        public async Task<Group?> UpdateGroupAsync(int id, Group group, ClaimsPrincipal user)
        {
            var existing = await _context.Groups
                .Include(g => g.Companies)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (existing == null)
                return null;

            var (userId, _) = GetUserInfo(user);

            // 🔹 Prevent duplicate company assignments
            await ValidateCompaniesNotInOtherGroups(group.Companies, id);

            existing.GroupName = group.GroupName;
            existing.IsActive = group.IsActive;
            existing.LastModifiedById = userId;
            existing.LastModified = DateTime.UtcNow;

            // 🔹 Update company list
            existing.Companies.Clear();
            foreach (var company in group.Companies)
            {
                var trackedCompany = await _context.Companies.FindAsync(company.Id);
                if (trackedCompany != null)
                    existing.Companies.Add(trackedCompany);
            }

            _context.Groups.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        // ✅ Deactivate group (and optionally mark companies inactive)
        public async Task<bool> DeactivateGroupAsync(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Companies)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null) return false;

            group.IsActive = false;
            group.LastModified = DateTime.UtcNow;

            // Optional: also deactivate its companies
            foreach (var company in group.Companies)
            {
                company.IsActive = false;
                company.LastModified = DateTime.UtcNow;
            }

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔍 Prevent company duplication across groups
        private async Task ValidateCompaniesNotInOtherGroups(IEnumerable<Company> companies, int? updatingGroupId = null)
        {
            var companyIds = companies.Select(c => c.Id).ToList();
            if (!companyIds.Any()) return;

            var conflictingCompanies = await _context.Companies
                .Where(c => companyIds.Contains(c.Id)
                            && c.GroupId != null
                            && (!updatingGroupId.HasValue || c.GroupId != updatingGroupId))
                .Select(c => c.CompanyName)
                .ToListAsync();

            if (conflictingCompanies.Any())
                throw new InvalidOperationException(
                    $"These companies already belong to another group: {string.Join(", ", conflictingCompanies)}");
        }

        // 🧩 Extract current user info from Claims
        private (int?, string?) GetUserInfo(ClaimsPrincipal user)
        {
            if (user.IsInRole("Admin"))
                return (null, "Administrator");

            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("UserId")
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            int? userId = null;
            if (int.TryParse(userIdClaim, out var parsedId))
                userId = parsedId;

            return (userId, user.Identity?.Name);
        }
    }
}
