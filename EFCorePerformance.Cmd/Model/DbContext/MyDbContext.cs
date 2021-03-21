using Microsoft.EntityFrameworkCore;

namespace EFCorePerformance.Cmd.Model
{
    //Add-Migration <migration name> -Context MyDbContext -StartupProject EFCorePerformance.Cmd -Project EFCorePerformance.Cmd

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }  

     
        public virtual DbSet<Report> Reports { get; set; }

        public virtual DbSet<ReportComment> ReportComments { get; set; }

        public virtual DbSet<ReportConfig> ReportConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Primary keys
            modelBuilder.Entity<Report>().HasKey(v => v.ReportId);
            modelBuilder.Entity<ReportComment>().HasKey((System.Linq.Expressions.Expression<System.Func<ReportComment, object>>)(v => (object)v.CommentId));
            modelBuilder.Entity<ReportConfig>().HasKey(v => v.ConfigId);

            //Relationships
            modelBuilder.Entity<ReportComment>()
              .HasOne(rc => rc.Report)
              .WithMany(r => r.Comments)
              .HasForeignKey(rc=> rc.ReportId);

            modelBuilder.Entity<Report>()
            .HasOne(r => r.Config)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.ConfigId);

            //Indexs
            modelBuilder.Entity<Report>()
            .HasIndex(r => r.Name)
            .IncludeProperties(r => new
            {
                r.ReportId,
                r.Status
            })
            .HasFilter("[IsArchived] = 0");          
        }
    }
}
