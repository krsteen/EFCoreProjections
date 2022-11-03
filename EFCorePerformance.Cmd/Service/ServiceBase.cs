using AutoMapper;
using EFCoreProjections.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFCoreProjections.Cmd.Service
{
    public class ServiceBase
    {
        protected string ConnectionString;
        protected MyDbContext Db { get { return GetDatabaseContext(); } }

        protected Mapper Mapper;
        protected MapperConfiguration MapperConfiguration;
      

        public ServiceBase()
        {
            ConnectionString = ConfigUtil.GetDbConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);

            MapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutomapperConfigs>();              
            });

            Mapper = new Mapper(MapperConfiguration);
        }

        public MyDbContext GetDatabaseContext()
        {
            return new MyDbContextFactory().CreateDbContext(new string[0]);                   
        }

        protected string QueryTag(string testName)
        {
            return $"{this.GetType().Name} - {testName}";
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
