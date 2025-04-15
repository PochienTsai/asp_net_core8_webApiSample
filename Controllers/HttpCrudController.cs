using asp_net_core8_webApiSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core8_webApiSample.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class HttpCrudController : ControllerBase
  {
    private readonly HttpService _httpService;
    public HttpCrudController(HttpService httpService)
    {
      _httpService = httpService;
    }

    // 範例：取得外部 API 資訊
    [HttpGet("get-demo")]
    public async Task<IActionResult> GetDemo(string url)
    {
      var result = await _httpService.GetAsync<object>(url);
      return Ok(result);
    }

    // 範例：建立外部 API 資訊
    [HttpPost("post-demo")]
    public async Task<IActionResult> PostDemo(string url, [FromBody] object data)
    {
      var result = await _httpService.PostAsync<object, object>(url, data);
      return Ok(result);
    }

    // 範例：更新外部 API 資訊
    [HttpPut("put-demo")]
    public async Task<IActionResult> PutDemo(string url, [FromBody] object data)
    {
      var result = await _httpService.PutAsync<object, object>(url, data);
      return Ok(result);
    }

    // 範例：刪除外部 API 資訊
    [HttpDelete("delete-demo")]
    public async Task<IActionResult> DeleteDemo(string url)
    {
      var success = await _httpService.DeleteAsync(url);
      return Ok(new { success });
    }
  }
}
