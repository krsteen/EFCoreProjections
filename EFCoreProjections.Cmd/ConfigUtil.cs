using Microsoft.Extensions.Configuration;
using System.IO;

namespace EFCoreProjections.Cmd
{
    public static class ConfigUtil
    {
        public static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true).Build();
        }

        public static string GetValue(string key)
        {
           return GetConfig()[key];
        }

        public static string GetDbConnectionString()
        {
            return GetValue("DbConnectionString");
        }        
    }
}
