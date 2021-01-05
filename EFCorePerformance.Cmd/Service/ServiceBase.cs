using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFCorePerformance.Cmd.Service
{
    public class ServiceBase
    {
        protected string ConnectionString;
        protected MyDbContext Db { get; private set; }

        public ServiceBase()
        {
            ConnectionString = ConfigUtil.GetDbConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
            Db = new MyDbContextFactory().CreateDbContext(new string[0]);
        }

        protected string Serialize(object whatToSerialize)
        {

            return JsonConvert.SerializeObject(whatToSerialize, Formatting.None,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
        }
    }
}
