using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCorePerformance.Cmd.Service
{
    public class ServiceBase
    {
        protected string ConnectionString;
        protected MyDbContext Db { get { return GetDb(); } }

        public ServiceBase()
        {
            ConnectionString = ConfigUtil.GetDbConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        public MyDbContext GetDb()
        {
            var db = new MyDbContextFactory().CreateDbContext(new string[0]);
            return db;
            
        }
    }
}
