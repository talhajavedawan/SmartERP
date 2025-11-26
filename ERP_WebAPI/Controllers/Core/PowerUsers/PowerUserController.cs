using BCrypt.Net;
using ERP_BL.Entities.Core.PowerUsers;
using ERP_REPO.Repo.Core.PowerUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ERP_WebAPI.Controllers.Core.PowerUsers
{
    [Route("[controller]")]
    [ApiController]
  
    public class PowerUserController : ControllerBase
    {
        private readonly IPowerUserRepo _repo;
        private readonly IConfiguration _config;

        public PowerUserController(IPowerUserRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] PowerUser user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                if (user.UserName.ToLower() != "administrator")
                    return BadRequest("Only 'Administrator' user can be registered.");

                var existing = await _repo.GetByPowerUserNameAsync(user.UserName);
                if (existing != null)
                    return BadRequest("Administrator already exists.");

                // Trim and hash the password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password.Trim());

                await _repo.AddPowerUserAsync(user);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] PowerUser loginUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUser.UserName) || string.IsNullOrWhiteSpace(loginUser.Password))
                    return BadRequest("Username and password are required.");

                var user = await _repo.GetByPowerUserNameAsync(loginUser.UserName);
                if (user == null)
                    return Unauthorized(new { message = "Invalid username or password" });

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUser.Password.Trim(), user.Password);
                if (!isPasswordValid)
                    return Unauthorized(new { message = "Invalid username or password" });

                // ✅ Generate Access Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var claims = new[]
                {
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim(ClaimTypes.Role, "Admin"),
                   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim("UserId", user.Id.ToString()),
                   new Claim("LoginType", "PowerUser") // ✅ new claim
                 };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);

                // ✅ Generate & Save Refresh Token
                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _repo.UpdatePowerUserAsync(user); // 🔥 save in DB

                return Ok(new
                {
                    message = "Login successful",
                    username = user.UserName,
                    userId = user.Id,
                    role = "Admin",
                    accessToken,
                    refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }


    }
}
