using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace asp_net_core8_webApiSample.Controllers
{
  [Authorize]
  public class SampleController : BaseController
  {
    private readonly ILogger<SampleController> _logger;

    public SampleController(ILogger<SampleController> logger)
    {
      _logger = logger;
    }

    [HttpGet("hello")]
    [AllowAnonymous]
    public IActionResult Hello()
    {
      _logger.LogInformation("[Hello] 匿名存取 hello endpoint");
      return ApiResponse(new { greeting = "Hello, 世界!" });
    }

    [HttpGet("secure-data")]
    public IActionResult SecureData()
    {
      _logger.LogInformation("[SecureData] 使用者 {User} 存取 secure-data endpoint", User.Identity?.Name);
      // 只有帶 JWT Token 才能存取
      return ApiResponse(new { secret = "這是受保護的資料" });
    }
  }
}
