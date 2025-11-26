using ERP_BL.Data;
using ERP_BL.Entities.Core.Statuses;
using ERP_BL.Entities.Core.Statuses.Dtos;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ERP_REPO.Repo.Core.Statuses
{
    public interface IStatusRepo
    {
        Task<IEnumerable<StatusDto>> GetAllStatusesAsync(TransactionItemType? type = null, string status = "all");
        Task<StatusDto?> GetStatusByIdAsync(int id);
        Task<StatusDto> AddStatusAsync(StatusCreateDto dto, TransactionItemType type, ClaimsPrincipal user);
        Task<StatusDto?> UpdateStatusAsync(StatusUpdateDto dto, TransactionItemType type, ClaimsPrincipal user);
        Task<bool> DeactivateStatusAsync(int id);
    }

    public class StatusService : IStatusRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StatusService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // ────── GET CURRENT USER ID (like EmployeeRepo) ──────
        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException("User not authenticated.");
            return userId;
        }

        // ────── VALIDATE USER IS NOT POWERUSER ──────
        private void ValidateUserNotPowerUser(ClaimsPrincipal user)
        {
            if (user.IsInRole("PowerUser"))
                throw new UnauthorizedAccessException("PowerUser is not allowed to modify statuses.");
        }

        // ────── GET ALL ──────
        public async Task<IEnumerable<StatusDto>> GetAllStatusesAsync(TransactionItemType? type = null, string status = "all")
        {
            var query = _context.Statuses
                .Include(s => s.CreatedBy)
                .Include(s => s.LastModifiedBy)
                .AsQueryable();

            status = status?.Trim().ToLower() ?? "all";
            query = status switch
            {
                "active" => query.Where(s => s.IsActive),
                "inactive" => query.Where(s => !s.IsActive),
                _ => query
            };

            if (type.HasValue)
                query = query.Where(s => s.TransactionItemType == type.Value);

            return await query
                .Select(s => new StatusDto
                {
                    Id = s.Id,
                    StatusName = s.StatusName,
                    IsActive = s.IsActive,
                    BackColor = s.BackColor ?? string.Empty,
                    ForeColor = s.ForeColor ?? string.Empty,
                    TransactionItemType = s.TransactionItemType.ToString(),
                    CreationDate = s.CreationDate,
                    CreatedBy = s.CreatedBy != null ? s.CreatedBy.UserName ?? "Unknown" : "Unknown",
                    ModifiedDate = s.ModifiedDate,
                    LastModifiedBy = s.LastModifiedBy != null ? s.LastModifiedBy.UserName ?? "Unknown" : "Unknown"
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // ────── GET BY ID ──────
        public async Task<StatusDto?> GetStatusByIdAsync(int id)
        {
            return await _context.Statuses
                .Include(s => s.CreatedBy)
                .Include(s => s.LastModifiedBy)
                .Where(s => s.Id == id)
                .Select(s => new StatusDto
                {
                    Id = s.Id,
                    StatusName = s.StatusName,
                    IsActive = s.IsActive,
                    BackColor = s.BackColor ?? string.Empty,
                    ForeColor = s.ForeColor ?? string.Empty,
                    TransactionItemType = s.TransactionItemType.ToString(),
                    CreationDate = s.CreationDate,
                    CreatedBy = s.CreatedBy != null ? s.CreatedBy.UserName ?? "Unknown" : "Unknown",
                    ModifiedDate = s.ModifiedDate,
                    LastModifiedBy = s.LastModifiedBy != null ? s.LastModifiedBy.UserName ?? "Unknown" : "Unknown"
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        // ────── CREATE ──────
        public async Task<StatusDto> AddStatusAsync(StatusCreateDto dto, TransactionItemType type, ClaimsPrincipal user)
        {
            ValidateUserNotPowerUser(user); // Block PowerUser
            int userId = GetCurrentUserId();

            var entity = new Status
            {
                StatusName = dto.StatusName.Trim(),
                BackColor = dto.BackColor?.Trim(),
                ForeColor = dto.ForeColor?.Trim(),
                IsActive = dto.IsActive,
                TransactionItemType = type,
                CreatedById = userId,
                CreationDate = DateTime.UtcNow
            };

            _context.Statuses.Add(entity);
            await _context.SaveChangesAsync();

            return await MapToDtoAsync(entity.Id);
        }

        // ────── UPDATE ──────
        public async Task<StatusDto?> UpdateStatusAsync(StatusUpdateDto dto, TransactionItemType type, ClaimsPrincipal user)
        {
            ValidateUserNotPowerUser(user); // Block PowerUser
            int userId = GetCurrentUserId();

            var existing = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == dto.Id);
            if (existing == null) return null;

            if (existing.TransactionItemType != type)
                throw new InvalidOperationException("Cannot change TransactionItemType.");

            existing.StatusName = dto.StatusName.Trim();
            existing.BackColor = dto.BackColor?.Trim();
            existing.ForeColor = dto.ForeColor?.Trim();
            existing.IsActive = dto.IsActive;
            existing.LastModifiedById = userId;
            existing.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await MapToDtoAsync(existing.Id);
        }

        // ────── DEACTIVATE ──────
        public async Task<bool> DeactivateStatusAsync(int id)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == id);
            if (status == null) return false;

            status.IsActive = false;
            status.ModifiedDate = DateTime.UtcNow;
            // Note: Deactivate doesn't require user — it's admin-only via route
            await _context.SaveChangesAsync();
            return true;
        }

        // ────── HELPER: Map to DTO ──────
        private async Task<StatusDto> MapToDtoAsync(int id)
        {
            return await GetStatusByIdAsync(id)
                   ?? throw new InvalidOperationException("Failed to map status to DTO.");
        }
    }
}