using ERP_BL.Entities.Core.Permissions.Dtos;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace ERP_REPO.Repo.Core.Users
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetAllUsersAsync(string status);
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserRoleResponseDto>> GetUserRolesAsync(int id);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(int id, User user);
        Task<IdentityResult> DeleteUserAsync(int id);
        Task<object?> GetCurrentUserAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }

    public class UserService : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync(string status = "all")
        {
            var query = _userManager.Users
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Person)
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Contact)
                .Where(u => !u.IsVoid)
                .AsQueryable();

            status = status?.Trim().ToLower() ?? "all";

            query = status switch
            {
                "active" => query.Where(u => u.IsActive),
                "inactive" => query.Where(u => !u.IsActive),
                _ => query
            };

            return await query.AsNoTracking().ToListAsync();
        }


        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _userManager.Users
                    .Include(u => u.Employee)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsVoid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching user with ID {id}.");
                return null;
            }
        }

        public async Task<IEnumerable<UserRoleResponseDto>> GetUserRolesAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()); // ✅ Id int → string
                if (user == null || user.IsVoid)
                {
                    _logger.LogWarning($"User with ID {id} not found or is voided.");
                    return Enumerable.Empty<UserRoleResponseDto>();
                }

                var roleNames = await _userManager.GetRolesAsync(user);

                var roles = await _roleManager.Roles
                    .Where(r => roleNames.Contains(r.Name!) && r.IsActive && !r.IsVoid)
                    .Include(r => r.Permissions.Where(p => p.IsActive)) // Only include active permissions
                    .Select(r => new UserRoleResponseDto
                    {
                        Id = r.Id,
                        Name = r.Name ?? string.Empty,
                        Description = r.Description,
                        ParentRoleId = r.ParentRoleId,
                        IsActive = r.IsActive,
                        Permissions = r.Permissions.Select(p => new PermissionDto
                        {
                            Id = p.Id,
                            Name = p.Name ?? string.Empty,
                            Description = p.Description,
                            ParentPermissionId = p.ParentPermissionId,
                            IsActive = p.IsActive
                        }).ToList()
                    })
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching roles for user with ID {id}.");
                return Enumerable.Empty<UserRoleResponseDto>();
            }
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty.");

                if (string.IsNullOrWhiteSpace(user.UserName))
                    throw new ArgumentException("UserName cannot be empty.");

                user.IsActive = true;
                user.IsVoid = false;
                user.IsLoggedIn = false;

                var result = await _userManager.CreateAsync(user, password);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user.");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(int id, User user)
        {
            try
            {
                var existing = await _userManager.FindByIdAsync(id.ToString()); // ✅ fix
                if (existing == null || existing.IsVoid)
                {
                    _logger.LogWarning($"Update failed. User with ID {id} not found.");
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                existing.UserName = user.UserName;
                existing.EmployeeId = user.EmployeeId;
                existing.IsActive = user.IsActive;

                return await _userManager.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}.");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()); // ✅ fix
                if (user == null || user.IsVoid)
                {
                    _logger.LogWarning($"Delete failed. User with ID {id} not found.");
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                user.IsVoid = true;
                var result = await _userManager.UpdateAsync(user); // soft delete
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}.");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }

        }
        public async Task<object?> GetCurrentUserAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentNullException(nameof(userId));

                var user = await _userManager.Users
                    .Where(u => u.Id.ToString() == userId && !u.IsVoid)
                    .Select(u => new
                    {
                        u.UserName,
                        u.Email,
                        u.Id,
                        u.IsActive
                    })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching current user with ID {userId}.");
                return null;
            }

        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsVoid)
                    return IdentityResult.Failed(new IdentityError { Description = "User not found or is voided." });

                var check = await _userManager.CheckPasswordAsync(user, oldPassword);
                if (!check)
                    return IdentityResult.Failed(new IdentityError { Description = "Old password is incorrect." });

                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (!result.Succeeded)
                    return result;

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error changing password for user ID {userId}.");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

    }
}
        
    

