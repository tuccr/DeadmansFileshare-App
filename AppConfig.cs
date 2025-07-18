using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DeadmansFileshareAppCSharp
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; }

        static AppConfig()
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            Configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

        }

        public static string API_URI => Configuration["API_URI"];
        public static string CRED_NAME => Configuration["CRED_NAME"];
        public static string TOKEN_NAME => Configuration["TOKEN_NAME"];
    }
}
