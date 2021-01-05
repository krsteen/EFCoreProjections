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


        //WITH INDEXING

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupReportsWithBasicIndexes(modelBuilder);
            SetupReportsWithBetterIndexes(modelBuilder);
        }

        void SetupReportsWithBasicIndexes(ModelBuilder modelBuilder)
        {
            //Primary keys
            modelBuilder.Entity<ReportWithBasicIndex>().HasKey(v => v.Id);          
            modelBuilder.Entity<ReportCommentWithBasicIndex>().HasKey(v => v.Id);
            modelBuilder.Entity<ReportConfigWithBasicIndex>().HasKey(v => v.Id);

            //Relationships
            modelBuilder.Entity<ReportCommentWithBasicIndex>()
              .HasOne(d => d.Report)
              .WithMany(r=> r.Comments)
              .HasForeignKey(d => d.ReportId);

            modelBuilder.Entity<ReportWithBasicIndex>()
            .HasOne(r => r.Config)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.ConfigId);
        }

        void SetupReportsWithBetterIndexes(ModelBuilder modelBuilder)
        {
            //Primary keys
            modelBuilder.Entity<ReportWithBetterIndex>().HasKey(v => v.Id);
            modelBuilder.Entity<ReportCommentWithBetterIndex>().HasKey(v => v.Id);
            modelBuilder.Entity<ReportConfigWithBetterIndex>().HasKey(v => v.Id);

            //Relationships
            modelBuilder.Entity<ReportCommentWithBetterIndex>()
              .HasOne(d => d.Report)
              .WithMany(r => r.Comments)
              .HasForeignKey(d => d.ReportId);

            modelBuilder.Entity<ReportWithBetterIndex>()
            .HasOne(r => r.Config)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.ConfigId);

            //Todo: Add indexes

        }

    }
}
