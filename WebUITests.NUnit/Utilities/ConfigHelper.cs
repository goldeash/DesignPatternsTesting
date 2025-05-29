using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace WebUITests.NUnit.Utilities
{
    public static class ConfigHelper
    {
        private static readonly IConfiguration Configuration;

        static ConfigHelper()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configPath = Path.Combine(assemblyLocation, "Config", "appsettings.json");

            Configuration = new ConfigurationBuilder()
                .SetBasePath(assemblyLocation)
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();
        }

        public static string BaseUrl => Configuration["ApiSettings:BaseUrl"];
        public static string AuthUrl => Configuration["ApiSettings:AuthUrl"];
        public static string ClientId => Configuration["ApiSettings:ClientId"];
        public static string ClientSecret => Configuration["ApiSettings:ClientSecret"];
        public static string Scope => Configuration["ApiSettings:Scope"];
    }
}