using ERP_BL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP_BL.Entities.Core.Users;

namespace ERP_REPO.Repo.Authentications
{
  public interface IAuthRepo
  {
    Task<string> RegisterAsync(User incomingUser, string password);
    Task<(string AccessToken, string RefreshToken, User user)> LoginAsync(string username, string password);
    Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
  }

  public class AuthService : IAuthRepo
  {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<User> userManager,
                       SignInManager<User> signInManager,
                       IConfiguration config,
                       ApplicationDbContext context)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _config = config;
      _context = context;
    }

    public async Task<string> RegisterAsync(User incomingUser, string password)
    {
      var existingUser = await _userManager.FindByNameAsync(incomingUser.UserName ?? "");
      if (existingUser != null)
        return "User already exists.";

      incomingUser.IsActive = true;
      incomingUser.IsVoid = false;
      incomingUser.IsLoggedIn = false;

      var result = await _userManager.CreateAsync(incomingUser, password);
      if (!result.Succeeded)
        return string.Join(", ", result.Errors.Select(e => e.Description));

      return "User registered successfully.";
    }

    public async Task<(string AccessToken, string RefreshToken, User user)> LoginAsync(string username, string password)
    {
      var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
      if (user == null || !user.IsActive || user.IsVoid)
        return (null, null, null);

      var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
      if (!result.Succeeded)
        return (null, null, null);

      var accessToken = await GenerateJwtTokenAsync(user);
      var refreshToken = GenerateRefreshToken();

      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
      await _userManager.UpdateAsync(user);

      return (accessToken, refreshToken, user);
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
    {
      var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

      if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        return (null, null);

      var newAccessToken = await GenerateJwtTokenAsync(user);
      var newRefreshToken = GenerateRefreshToken();

      user.RefreshToken = newRefreshToken;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
      await _userManager.UpdateAsync(user);

      return (newAccessToken, newRefreshToken);
    }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key missing"))
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim("UserId", user.Id.ToString()),
    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),   
    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};


            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var userRoles = await _context.Roles
                .Where(r => roles.Contains(r.Name!))
                .Include(r => r.Permissions)
                .ToListAsync();

            var permissions = userRoles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            claims.AddRange(permissions.Select(p => new Claim("permission", p ?? string.Empty)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        private string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }
  }
}
