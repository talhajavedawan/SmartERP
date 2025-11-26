using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.Core.Users.Dtos;
using ERP_REPO.Repo.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepo _authRepo;

    public AuthController(IAuthRepo authRepo)
    {
      _authRepo = authRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = new User
      {
        UserName = model.UserName,
        EmployeeId = model.EmployeeId,
        IsActive = true,
        IsVoid = false,
        IsLoggedIn = false
      };

      var result = await _authRepo.RegisterAsync(user, model.Password);

      if (result == "User already exists.")
        return BadRequest(new { message = result });

      return Ok(new { message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var (accessToken, refreshToken, user) = await _authRepo.LoginAsync(model.UserName, model.Password);

      if (accessToken == null || user == null)
        return Unauthorized(new { message = "Invalid username or password." });

      return Ok(new
      {
        accessToken,
        refreshToken,
        username = user.UserName,
        userId = user.Id
      });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
    {
      if (string.IsNullOrWhiteSpace(model.RefreshToken))
        return BadRequest(new { message = "Refresh token is required." });

      var (newAccessToken, newRefreshToken) = await _authRepo.RefreshTokenAsync(model.RefreshToken);

      if (newAccessToken == null || newRefreshToken == null)
        return Unauthorized(new { message = "Invalid or expired refresh token." });

      return Ok(new
      {
        accessToken = newAccessToken,
        refreshToken = newRefreshToken
      });
    }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Auth API is alive");
        }
    }
}
