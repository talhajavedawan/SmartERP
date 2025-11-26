using ERP_BL.Data;
using ERP_BL.Entities.Core.Permissions;
using ERP_BL.Entities.Core.Permissions.Dtos;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Roles.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP_REPO.Repo.Core.Roles
{
    public interface IRoleRepo
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);   // 👈 string → int
        Task AddRoleAsync(Role role, List<int> permissionIds);
        Task UpdateRoleAsync(Role role, List<int> permissionIds);
        Task DeleteRoleAsync(int roleId);             // 👈 string → int
    }

    public class RoleService : IRoleRepo
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleService(RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .Include(r => r.Permissions)
                .Where(r => r.IsActive && !r.IsVoid)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    ParentRoleId = r.ParentRoleId,
                    IsActive = r.IsActive,
                    IsVoid = r.IsVoid,
                    CreatedBy = r.CreatedBy,
                    CreationDate = r.CreationDate,
                    LastModifiedBy = r.LastModifiedBy,

                    PermissionIds = r.Permissions.Select(p => p.Id).ToList(),

                    Permissions = r.Permissions.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)   // 👈 string → int
        {
            return await _roleManager.Roles
                .Include(r => r.Permissions)
                .Where(r => r.Id == roleId && r.IsActive && !r.IsVoid)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    ParentRoleId = r.ParentRoleId,
                    IsActive = r.IsActive,
                    IsVoid = r.IsVoid,
                    CreatedBy = r.CreatedBy,
                    LastModifiedBy = r.LastModifiedBy,

                    PermissionIds = r.Permissions.Select(p => p.Id).ToList(),

                    Permissions = r.Permissions.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task AddRoleAsync(Role role, List<int> permissionIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                if (permissionIds?.Count > 0)
                {
                    var permissions = await _context.Permissions
                        .Where(p => permissionIds.Contains(p.Id))
                        .ToListAsync();

                    foreach (var permission in permissions)
                        role.Permissions.Add(permission);

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateRoleAsync(Role role, List<int> permissionIds)
        {
            var existing = await _context.Set<Role>()
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == role.Id);

            if (existing == null)
                throw new KeyNotFoundException($"Role with ID '{role.Id}' not found.");

            if (role.ParentRoleId == role.Id)
                throw new InvalidOperationException("A role cannot be its own parent.");

            if (role.ParentRoleId.HasValue)  // 👈 int? ParentRoleId
            {
                var parentRole = await _roleManager.FindByIdAsync(role.ParentRoleId.Value.ToString());
                if (parentRole == null)
                    throw new InvalidOperationException("The specified parent role does not exist.");
            }

            existing.Name = role.Name;
            existing.NormalizedName = role.Name?.ToUpper();
            existing.Description = role.Description;
            existing.ParentRoleId = role.ParentRoleId;
            existing.IsActive = role.IsActive;
            existing.IsVoid = role.IsVoid;
            existing.LastModified = DateTime.UtcNow;
            existing.LastModifiedBy = role.LastModifiedBy;

            existing.Permissions.Clear();

            if (permissionIds?.Count > 0)
            {
                var permissions = await _context.Set<Permission>()
                    .Where(p => permissionIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var permission in permissions)
                    existing.Permissions.Add(permission);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int roleId)   // 👈 string → int
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString()); // 👈 FindByIdAsync abhi string accept karta hai, isliye ToString()
            if (role == null)
                throw new KeyNotFoundException($"Role with ID '{roleId}' not found.");

            // Check for child roles
            var hasChildRoles = await _roleManager.Roles.AnyAsync(r => r.ParentRoleId == roleId);
            if (hasChildRoles)
                throw new InvalidOperationException("Cannot delete this role because it is a parent of other roles.");

            // Soft delete instead of hard delete
            role.IsActive = false;
            role.IsVoid = true;
            role.LastModified = DateTime.UtcNow;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
