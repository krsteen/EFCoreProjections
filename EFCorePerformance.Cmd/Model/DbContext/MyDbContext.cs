using EFCorePerformance.Cmd.Model.EF;
using Microsoft.EntityFrameworkCore;

namespace EFCorePerformance.Cmd.Model
{
    //Add-Migration <migration name> -Context MyDbContext -StartupProject EFCorePerformance.Cmd -Project EFCorePerformance.Cmd

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //MODELS WITH ONLY BASIC INDEXES
        public virtual DbSet<ReportWithBasicIndex> ReportsWithBasicIndex { get; set; }

        public virtual DbSet<ReportCommentWithBasicIndex> ReportCommentsWithBasicIndex { get; set; }

        public virtual DbSet<ReportConfigWithBasicIndex> ReportConfigsWithBasicIndexes { get; set; }

        //MODELS WITH BETTER INDEXES
        public virtual DbSet<ReportWithBetterIndex> ReportsWithBetterIndex { get; set; }

        public virtual DbSet<ReportCommentWithBetterIndex> ReportCommentsWithBetterIndex { get; set; }

        public virtual DbSet<ReportConfigWithBetterIndex> ReportConfigsWithBetterIndexes { get; set; }

        public DbSet<ReportLightWithBetterIndex> ReportsLigthBetterIndex { get; set; }

        //WITH INDEXING

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupReportsWithBasicIndexes(modelBuilder);
            SetupReportsWithBetterIndexes(modelBuilder);
        }

        void SetupReportsWithBasicIndexes(ModelBuilder modelBuilder)
        {
            //Primary keys
            modelBuilder.Entity<ReportWithBasicIndex>().HasKey(v => v.ReportId);
            modelBuilder.Entity<ReportCommentWithBasicIndex>().HasKey(v => v.CommentId);
            modelBuilder.Entity<ReportConfigWithBasicIndex>().HasKey(v => v.ConfigId);

            //Relationships
            modelBuilder.Entity<ReportCommentWithBasicIndex>()
              .HasOne(d => d.Report)
              .WithMany(r => r.Comments)
              .HasForeignKey(d => d.ReportId);

            modelBuilder.Entity<ReportWithBasicIndex>()
            .HasOne(r => r.Config)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.ConfigId);
        }

        void SetupReportsWithBetterIndexes(ModelBuilder modelBuilder)
        {
            //Primary keys
            modelBuilder.Entity<ReportWithBetterIndex>().HasKey(v => v.ReportId);
            modelBuilder.Entity<ReportCommentWithBetterIndex>().HasKey(v => v.CommentId);
            modelBuilder.Entity<ReportConfigWithBetterIndex>().HasKey(v => v.ConfigId);

            //Relationships
            modelBuilder.Entity<ReportCommentWithBetterIndex>()
              .HasOne(d => d.Report)
              .WithMany(r => r.Comments)
              .HasForeignKey(d => d.ReportId);

            modelBuilder.Entity<ReportWithBetterIndex>()
            .HasOne(r => r.Config)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.ConfigId);

            //Add indexes
            modelBuilder.Entity<ReportWithBetterIndex>()
            .HasIndex(p => p.Name)
            .IncludeProperties(p => new
            {
                p.ReportId,
                p.Status
            })
            .HasFilter("[IsArchived] = 0");

            //Keyless type for report table
            modelBuilder.Entity<ReportLightWithBetterIndex>(eb =>
            {
            eb.HasNoKey();
            eb.ToTable("[dbo].[ReportsWithBetterIndex]");
            });

        }

    }
}
