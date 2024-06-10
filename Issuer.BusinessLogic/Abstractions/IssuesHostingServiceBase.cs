using System.Text;
using System.Text.Json;

namespace Issuer.BusinessLogic.Abstractions
{
    public abstract class IssuesHostingServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _authToken;

        protected IssuesHostingServiceBase(string authToken)
        {
            _httpClient = new HttpClient();
            _authToken = authToken;
        }

        protected HttpRequestMessage CreateRequest(HttpMethod method, string url, object data = null)
        {
            var request = new HttpRequestMessage(method, url);

            ConfigureRequestHeaders(request);

            if (data != null)
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                request.Content = content;
            }

            return request;
        }

        protected async Task<string> SendRequestAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        protected abstract void ConfigureRequestHeaders(HttpRequestMessage request);
    }
}