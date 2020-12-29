using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EFCorePerformance.Cmd.Service
{
    public class ServiceBase
    {
        protected MyDbContext _db { get; private set; }

       public ServiceBase() {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        
             .Build();

            string connectionString = config["DbConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
            _db = new MyDbContext(optionsBuilder.Options);
        }
    }
}
