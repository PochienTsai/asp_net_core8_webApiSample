using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace asp_net_core8_webApiSample.Services
{
  public interface IJwtTokenService
  {
    string 產生JwtToken(string username);
  }

  public class JwtTokenService : IJwtTokenService
  {
    private readonly string _jwtKey;
    public JwtTokenService(IConfiguration configuration)
    {
      // 這裡可改為從 appsettings 取得金鑰
      _jwtKey = configuration["JwtKey"] ?? "ThisIsASecretKeyForJwtToken123456!";
    }

    public string 產生JwtToken(string username)
    {
      var claims = new[]
      {
                new Claim(ClaimTypes.Name, username)
            };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(
          issuer: "yourIssuer",
          audience: "yourAudience",
          claims: claims,
          expires: DateTime.UtcNow.AddHours(1),
          signingCredentials: creds
      );
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
