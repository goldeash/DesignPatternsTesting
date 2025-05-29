using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebUITests.NUnit.Models;

namespace WebUITests.NUnit.Utilities
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private string _token;
        private bool _disposed;

        public ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(ConfigHelper.BaseUrl) };
            Authenticate().Wait();
        }

        private async Task Authenticate()
        {
            var authClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_Id", ConfigHelper.ClientId),
                new KeyValuePair<string, string>("client_Secret", ConfigHelper.ClientSecret),
                new KeyValuePair<string, string>("scope", ConfigHelper.Scope),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await authClient.PostAsync(ConfigHelper.AuthUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
            _token = authResponse?.AccessToken;

            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            LogResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            LogResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            LogResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            LogResponse(response);
            return response;
        }

        private void LogResponse(HttpResponseMessage response)
        {
            var logger = LoggerConfig.ConfigureLogger("ApiClient");
            logger.Information($"Request: {response.RequestMessage.Method} {response.RequestMessage.RequestUri}");
            logger.Information($"Response: {response.StatusCode}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }
                _disposed = true;
            }
        }

        private class AuthResponse
        {
            public string AccessToken { get; set; }
        }
    }
}