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
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string API_URI => Configuration["API_URI"];
    }
}
