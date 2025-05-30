using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace WebUITests.NUnit.Utilities
{
    public static class ConfigHelper
    {
        public static string BaseUrl => Environment.GetEnvironmentVariable("API_BASE_URL") ?? "https://lecture-books-api.azurewebsites.net";
        public static string AuthUrl => Environment.GetEnvironmentVariable("AUTH_URL") ?? "https://login.microsoftonline.com/{tenant_id}/oauth2/v2.0/token";
        public static string ClientId => Environment.GetEnvironmentVariable("CLIENT_ID") ?? "your_client_id_here";
        public static string ClientSecret => Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? "your_client_secret_here";
        public static string Scope => Environment.GetEnvironmentVariable("SCOPE") ?? "api://{application_id}/.default";
    }
}