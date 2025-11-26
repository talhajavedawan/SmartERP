using ERP_BL.Data;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.Core.Users.Dtos;
using ERP_REPO.Repo.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.Core.Users
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepo _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(IUserRepo userRepo, ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string status = "all")
        {
            var users = await _userRepo.GetAllUsersAsync(status);
            return Ok(new { data = users });
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = $"User with ID {id} not found." });
            return Ok(user);
        }

        [HttpGet("GetUserRoles/{id:int}")]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            var roles = await _userRepo.GetUserRolesAsync(id);
            return Ok(roles);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Password is required." });

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                EmployeeId = model.EmployeeId,
                IsActive = model.IsActive,
                IsLoggedIn = false
            };

            var result = await _userRepo.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

            if (model.Roles != null && model.Roles.Any())
            {
                var roleNames = new List<string>();
                foreach (var roleId in model.Roles)
                {
                    var role = await _roleManager.FindByIdAsync(roleId.ToString()); // ✅ FIXED
                    if (role == null)
                        return BadRequest(new { Errors = new[] { $"Role with ID {roleId} does not exist." } });

                    if (!string.IsNullOrEmpty(role.Name))
                        roleNames.Add(role.Name);
                }

                var roleResult = await _userManager.AddToRolesAsync(user, roleNames);
                if (!roleResult.Succeeded)
                    return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
            }

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null || user.IsVoid)
                return NotFound(new { message = "User not found or is voided." });

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.EmployeeId = model.EmployeeId;
            user.IsActive = model.IsActive;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                    return BadRequest(new { Errors = removePasswordResult.Errors.Select(e => e.Description) });

                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                    return BadRequest(new { Errors = addPasswordResult.Errors.Select(e => e.Description) });
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { Errors = updateResult.Errors.Select(e => e.Description) });

            var currentRoleNames = await _userManager.GetRolesAsync(user);
            var currentRoleIds = await _roleManager.Roles
                .Where(r => currentRoleNames.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync();

            var rolesToRemove = currentRoleIds.Except(model.Roles).ToList();
            var rolesToAdd = model.Roles.Except(currentRoleIds).ToList();

            if (rolesToRemove.Any())
            {
                var removeNames = await _roleManager.Roles
                    .Where(r => rolesToRemove.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync();

                var removeResult = await _userManager.RemoveFromRolesAsync(user, removeNames);
                if (!removeResult.Succeeded)
                    return BadRequest(new { Errors = removeResult.Errors.Select(e => e.Description) });
            }

            if (rolesToAdd.Any())
            {
                var addNames = await _roleManager.Roles
                    .Where(r => rolesToAdd.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync();

                var addResult = await _userManager.AddToRolesAsync(user, addNames);
                if (!addResult.Succeeded)
                    return BadRequest(new { Errors = addResult.Errors.Select(e => e.Description) });
            }

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("Void/{id:int}")]
        public async Task<IActionResult> Void(int id)
        {
            var result = await _userRepo.DeleteUserAsync(id);

            if (!result.Succeeded)
                return NotFound(new { message = $"User with ID {id} not found or already voided." });

            return Ok(new { message = "User voided successfully." });
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var loginType = User.FindFirst("LoginType")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            if (loginType == "PowerUser")
            {
                return Ok(new
                {
                    userName = "Administrator",
                    email = "Administrator2244@gmail.com",
                    id = int.Parse(userId),
                    isActive = true
                });
            }

            var user = await _userRepo.GetCurrentUserAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeCredentialsDto dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User not authenticated." });

                var result = await _userRepo.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        message = "Failed to change password.",
                        errors = result.Errors.Select(e => e.Description)
                    });
                }

                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in ChangePassword: {ex.Message}");

                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while changing the password.",
                    error = ex.Message
                });
            }
        }


    }
}
