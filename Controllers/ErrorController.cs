using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace asp_net_core8_webApiSample.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("/error")]
  public class ErrorController : ControllerBase
  {
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
      _logger = logger;
    }

    [HttpGet, HttpPost, HttpPut, HttpDelete, HttpOptions, HttpPatch]
    public IActionResult HandleError()
    {
      var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
      var exception = context?.Error;
      if (exception != null)
      {
        _logger.LogError(exception, "[全域例外] {Message}", exception.Message);
      }
      return Problem(
          detail: exception?.Message ?? "未知錯誤",
          statusCode: 500,
          title: "伺服器錯誤"
      );
    }
  }
}
