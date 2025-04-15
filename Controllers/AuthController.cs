using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using asp_net_core8_webApiSample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace asp_net_core8_webApiSample.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly ILogger<AuthController> _logger;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(ILogger<AuthController> logger, IJwtTokenService jwtTokenService)
    {
      _logger = logger;
      _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 401)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
      _logger.LogInformation("[登入] 使用者: {Username} 嘗試登入", request.Username);

      // 範例帳號密碼驗證（實務請查資料庫）
      if (request.Username == "admin" && request.Password == "123456")
      {
        _logger.LogInformation("[登入成功] 使用者: {Username}", request.Username);
        var jwt = _jwtTokenService.產生JwtToken(request.Username);
        return Ok(new { success = true, token = jwt });
      }

      _logger.LogWarning("[登入失敗] 使用者: {Username}", request.Username);
      return Unauthorized(new { success = false, message = "帳號或密碼錯誤" });
    }
  }

  public class LoginRequest
  {
    public required string Username { get; set; }
    public required string Password { get; set; }
  }
}
