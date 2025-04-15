using Microsoft.AspNetCore.Mvc;

namespace asp_net_core8_webApiSample.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BaseController : ControllerBase
  {
    protected IActionResult ApiResponse(object data, string message = "操作成功")
    {
      return Ok(new
      {
        success = true,
        data,
        error = (object)null,
        message
      });
    }

    protected IActionResult ApiError(string errorCode, string errorMessage, string message = "操作失敗")
    {
      return BadRequest(new
      {
        success = false,
        data = (object)null,
        error = new { code = errorCode, message = errorMessage },
        message
      });
    }
  }
}
