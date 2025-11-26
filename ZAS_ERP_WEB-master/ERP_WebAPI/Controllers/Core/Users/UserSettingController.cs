using ERP_BL.Data;
using ERP_BL.Entities.Core.PowerUsers;
using ERP_BL.Entities.Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_BL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserSettingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserSettingController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /UserSetting/{settingKey}
        [HttpGet("{settingKey}")]
        public async Task<ActionResult<string>> GetSetting(string settingKey)
        {
            Console.WriteLine($"GetSetting called with key: {settingKey}");
            Console.WriteLine($"User.Identity.Name: {User.Identity?.Name ?? "null"}");

            // Try to get Identity user via UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            User? identityUser = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }

            if (identityUser != null)
            {
                Console.WriteLine($"Found Identity User by UserId claim:");
                Console.WriteLine($"  Id = {identityUser.Id}");
                Console.WriteLine($"  UserName = {identityUser.UserName}");

                var setting = await _context.UserSettings
                    .Where(s => s.UserId == identityUser.Id && s.SettingKey == settingKey)
                    .Select(s => s.SettingValue)
                    .FirstOrDefaultAsync();

                return Ok(setting ?? "");
            }

            // Try to find a PowerUser by PowerUserId claim
            var powerUserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            PowerUser? powerUser = null;
            if (!string.IsNullOrEmpty(powerUserIdClaim) && int.TryParse(powerUserIdClaim, out var powerUserId))
            {
                powerUser = await _context.PowerUsers.FirstOrDefaultAsync(p => p.Id == powerUserId);
            }

            if (powerUser != null)
            {
                Console.WriteLine($"Found PowerUser by PowerUserId claim:");
                Console.WriteLine($"  Id = {powerUser.Id}");
                Console.WriteLine($"  UserName = {powerUser.UserName}");

                var setting = await _context.UserSettings
                    .Where(s => s.PowerUserId == powerUser.Id && s.SettingKey == settingKey)
                    .Select(s => s.SettingValue)
                    .FirstOrDefaultAsync();

                return Ok(setting ?? "");
            }

            // If not found, dump claims for debugging
            Console.WriteLine("User not found in AspNetUsers or PowerUsers. Dumping claims:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            return Unauthorized("User not found in either AspNetUsers or PowerUsers.");
        }

        // POST: /UserSetting
        [HttpPost]
        public async Task<ActionResult> SaveSetting([FromBody] UserSettingRequest request)
        {
            if (string.IsNullOrEmpty(request.SettingKey) || string.IsNullOrEmpty(request.SettingValue))
                return BadRequest("Setting key and value are required.");

            Console.WriteLine($"SaveSetting called with key: {request.SettingKey}, value: {request.SettingValue}");
            Console.WriteLine($"User.Identity.Name: {User.Identity?.Name ?? "null"}");

            // Try to get Identity user via UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            User? identityUser = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }

            if (identityUser != null)
            {
                Console.WriteLine($"Found Identity User by UserId claim:");
                Console.WriteLine($"  Id = {identityUser.Id}");
                Console.WriteLine($"  UserName = {identityUser.UserName}");

                var existingSetting = await _context.UserSettings
                    .FirstOrDefaultAsync(s => s.UserId == identityUser.Id && s.SettingKey == request.SettingKey);

                try
                {
                    if (existingSetting == null)
                    {
                        var newSetting = new UserSetting
                        {
                            UserId = identityUser.Id,
                            PowerUserId = null,
                            SettingKey = request.SettingKey,
                            SettingValue = request.SettingValue,
                            LastModified = DateTime.UtcNow
                        };
                        _context.UserSettings.Add(newSetting);
                    }
                    else
                    {
                        existingSetting.SettingValue = request.SettingValue;
                        existingSetting.LastModified = DateTime.UtcNow;
                        _context.UserSettings.Update(existingSetting);
                    }

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Setting saved successfully." });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error saving setting: {ex.InnerException?.Message}");
                    return StatusCode(500, new { message = "Failed to save setting due to a database error." });
                }
            }

            // Try to find PowerUser by PowerUserId claim

            var powerUserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            PowerUser? powerUser = null;
            if (!string.IsNullOrEmpty(powerUserIdClaim) && int.TryParse(powerUserIdClaim, out var powerUserId))
            {
                powerUser = await _context.PowerUsers.FirstOrDefaultAsync(p => p.Id == powerUserId);
            }

            if (powerUser != null)
            {
                Console.WriteLine($"Found PowerUser by PowerUserId claim:");
                Console.WriteLine($"  Id = {powerUser.Id}");
                Console.WriteLine($"  UserName = {powerUser.UserName}");

                var existingSetting = await _context.UserSettings
                    .FirstOrDefaultAsync(s => s.PowerUserId == powerUser.Id && s.SettingKey == request.SettingKey);

                try
                {
                    if (existingSetting == null)
                    {
                        var newSetting = new UserSetting
                        {
                            UserId = null,
                            PowerUserId = powerUser.Id,
                            SettingKey = request.SettingKey,
                            SettingValue = request.SettingValue,
                            LastModified = DateTime.UtcNow
                        };
                        _context.UserSettings.Add(newSetting);
                    }
                    else
                    {
                        existingSetting.SettingValue = request.SettingValue;
                        existingSetting.LastModified = DateTime.UtcNow;
                        _context.UserSettings.Update(existingSetting);
                    }

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Setting saved successfully." });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error saving setting: {ex.InnerException?.Message}");
                    return StatusCode(500, new { message = "Failed to save setting due to a database error." });
                }
            }

            // If not found, dump claims for debugging
            Console.WriteLine("User not found in AspNetUsers or PowerUsers. Dumping claims:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            return Unauthorized("User not found in either AspNetUsers or PowerUsers.");
        }
    }

    public class UserSettingRequest
    {
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
    }
}