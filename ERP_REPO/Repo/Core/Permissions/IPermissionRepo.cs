using ERP_BL.Data;
using ERP_BL.Entities.Core.Permissions;
using ERP_BL.Entities.Core.Permissions.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ERP_REPO.Repo.Core.Permissions
{
    public interface IPermissionRepo
    {
        Task<PaginatedPermissions> GetAllPermissionsPaginatedAsync(int page = 1, int pageSize = 10, string? search = null);


        Task<Permission?> GetPermissionByIdAsync(int id);
        Task<Permission> AddPermissionAsync(Permission permission);
        Task<Permission> UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int id);




    }



    public class PermissionService : IPermissionRepo
    {
        private readonly ApplicationDbContext _db;

        public PermissionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedPermissions> GetAllPermissionsPaginatedAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            var query = _db.Permissions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description != null && p.Description.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var permissions = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedPermissions
            {
                Permissions = permissions,
                TotalCount = totalCount
            };
        }

        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            return await _db.Permissions.FindAsync(id);
        }

        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            permission.CreationDate = DateTime.UtcNow;
            _db.Permissions.Add(permission);
            await _db.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            var existing = await _db.Permissions.FindAsync(permission.Id);
            if (existing == null) throw new KeyNotFoundException($"Permission with ID {permission.Id} not found.");

            existing.Name = permission.Name;
            existing.Description = permission.Description;
            existing.LastModified = DateTime.UtcNow;
            existing.LastModifiedBy = permission.LastModifiedBy;
            existing.IsActive = permission.IsActive;
            existing.IsVoid = permission.IsVoid;

            _db.Permissions.Update(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _db.Permissions.FindAsync(id);
            if (permission == null) return false;

            _db.Permissions.Remove(permission);
            await _db.SaveChangesAsync();
            return true;
        }

    }


}