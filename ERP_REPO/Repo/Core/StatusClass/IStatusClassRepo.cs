using ERP_BL.Data;
using ERP_BL.Entities.Core.StatusClass;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ERP_REPO.Repo.Core.StatusClasses
{
    public interface IStatusClassRepo
    {
        Task<IEnumerable<StatusClassDto>> GetAllStatusClassesAsync(TransactionItemType? type = null, string status = "all");
        Task<StatusClassDto?> GetStatusClassByIdAsync(int id);
        Task<StatusClassDto> AddStatusClassAsync(StatusClassCreateDto dto, TransactionItemType type, ClaimsPrincipal user);
        Task<StatusClassDto?> UpdateStatusClassAsync(StatusClassUpdateDto dto, TransactionItemType type, ClaimsPrincipal user);
        Task<bool> DeactivateStatusClassAsync(int id);
    }

    public class StatusClassService : IStatusClassRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StatusClassService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException("User not authenticated.");
            return userId;
        }

        // ─── Get All ───
        public async Task<IEnumerable<StatusClassDto>> GetAllStatusClassesAsync(TransactionItemType? type = null, string status = "all")
        {
            var query = _context.StatusClasses
                .Include(s => s.Status)
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
                .Select(s => new StatusClassDto
                {
                    Id = s.Id,
                    ClassName = s.ClassName,
                    IsApproved = s.IsApproved,
                    IsActive = s.IsActive,
                    BackColor = s.BackColor ?? "",
                    ForeColor = s.ForeColor ?? "",
                    StatusId = s.StatusId,
                    StatusName = s.Status.StatusName,
                    TransactionItemType = s.TransactionItemType.ToString(),
                    CreationDate = s.CreationDate,
                    CreatedBy = s.CreatedBy != null ? s.CreatedBy.UserName ?? "Unknown" : "Unknown",
                    ModifiedDate = s.ModifiedDate,
                    LastModifiedBy = s.LastModifiedBy != null ? s.LastModifiedBy.UserName ?? "Unknown" : "Unknown"
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // ─── Get By ID ───
        public async Task<StatusClassDto?> GetStatusClassByIdAsync(int id)
        {
            return await _context.StatusClasses
                .Include(s => s.Status)
                .Include(s => s.CreatedBy)
                .Include(s => s.LastModifiedBy)
                .Where(s => s.Id == id)
                .Select(s => new StatusClassDto
                {
                    Id = s.Id,
                    ClassName = s.ClassName,
                    IsApproved = s.IsApproved,
                    IsActive = s.IsActive,
                    BackColor = s.BackColor ?? "",
                    ForeColor = s.ForeColor ?? "",
                    StatusId = s.StatusId,
                    StatusName = s.Status.StatusName,
                    TransactionItemType = s.TransactionItemType.ToString(),
                    CreationDate = s.CreationDate,
                    CreatedBy = s.CreatedBy != null ? s.CreatedBy.UserName ?? "Unknown" : "Unknown",
                    ModifiedDate = s.ModifiedDate,
                    LastModifiedBy = s.LastModifiedBy != null ? s.LastModifiedBy.UserName ?? "Unknown" : "Unknown"
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        // ─── Create ───
        public async Task<StatusClassDto> AddStatusClassAsync(StatusClassCreateDto dto, TransactionItemType type, ClaimsPrincipal user)
        {
            int userId = GetCurrentUserId();

            var statusExists = await _context.Statuses.AnyAsync(s => s.Id == dto.StatusId);
            if (!statusExists)
                throw new ArgumentException("Invalid StatusId");

            var entity = new StatusClass
            {
                ClassName = dto.ClassName.Trim(),
                IsApproved = dto.IsApproved,
                IsActive = dto.IsActive,
                BackColor = dto.BackColor?.Trim(),
                ForeColor = dto.ForeColor?.Trim(),
                StatusId = dto.StatusId,
                TransactionItemType = type,
                CreatedById = userId,
                CreationDate = DateTime.UtcNow
            };

            _context.StatusClasses.Add(entity);
            await _context.SaveChangesAsync();

            return await GetStatusClassByIdAsync(entity.Id)
                ?? throw new Exception("Mapping failed.");
        }

        // ─── Update ───
        public async Task<StatusClassDto?> UpdateStatusClassAsync(StatusClassUpdateDto dto, TransactionItemType type, ClaimsPrincipal user)
        {
            int userId = GetCurrentUserId();

            var existing = await _context.StatusClasses.FirstOrDefaultAsync(s => s.Id == dto.Id);
            if (existing == null) return null;

            if (existing.TransactionItemType != type)
                throw new InvalidOperationException("Cannot change TransactionItemType.");

            existing.ClassName = dto.ClassName.Trim();
            existing.IsApproved = dto.IsApproved;
            existing.IsActive = dto.IsActive;
            existing.BackColor = dto.BackColor?.Trim();
            existing.ForeColor = dto.ForeColor?.Trim();
            existing.StatusId = dto.StatusId;
            existing.LastModifiedById = userId;
            existing.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetStatusClassByIdAsync(existing.Id);
        }

        // ─── Deactivate ───
        public async Task<bool> DeactivateStatusClassAsync(int id)
        {
            var item = await _context.StatusClasses.FirstOrDefaultAsync(s => s.Id == id);
            if (item == null) return false;

            item.IsActive = false;
            item.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
