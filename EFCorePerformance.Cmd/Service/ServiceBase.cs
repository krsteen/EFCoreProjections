using AutoMapper;
using EFCorePerformance.Cmd.Dto;
using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCorePerformance.Cmd.Service
{
    public class ServiceBase
    {
        protected string ConnectionString;
        protected MyDbContext Db { get { return GetDb(); } }

        protected MapperConfiguration MapperConfiguration;
        protected Mapper Mapper;


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

        public MyDbContext GetDb()
        {
            var db = new MyDbContextFactory().CreateDbContext(new string[0]);
            return db;            
        }

        protected string QueryTag(string testName)
        {
            return $"{this.GetType().Name} - {testName}";
        }
    }
}
