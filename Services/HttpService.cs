using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace asp_net_core8_webApiSample.Services
{
  public class HttpService
  {
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    // GET
    public async Task<T?> GetAsync<T>(string url)
    {
      return await _httpClient.GetFromJsonAsync<T>(url);
    }

    // POST
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
      var response = await _httpClient.PostAsJsonAsync(url, data);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    // PUT
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
      var response = await _httpClient.PutAsJsonAsync(url, data);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    // DELETE
    public async Task<bool> DeleteAsync(string url)
    {
      var response = await _httpClient.DeleteAsync(url);
      return response.IsSuccessStatusCode;
    }
  }
}
