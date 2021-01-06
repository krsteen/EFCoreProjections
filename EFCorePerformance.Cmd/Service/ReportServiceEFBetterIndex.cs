using EFCorePerformance.Cmd.DapperModel;
using EFCorePerformance.Cmd.Model.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceEFBetterIndex : ReportServiceBase, IReportService
    {
        protected readonly bool useBadLazyLoad;
        protected readonly bool useNoTracking;

        public ReportServiceEFBetterIndex(bool useBadLazyLoad, bool useNoTracking)
            : base()
        {
            this.useBadLazyLoad = useBadLazyLoad;
            this.useNoTracking = useNoTracking;
        }

        protected IQueryable<ReportWithBetterIndex> GetReportQueryable(bool anyIncludes)
        {
            var reportQueryable = Db.ReportsWithBetterIndex.AsQueryable();          

            if (anyIncludes)
            {
                reportQueryable = reportQueryable.Include(r => r.Config);

                if (useBadLazyLoad == false)
                {
                    reportQueryable = reportQueryable.Include(r => r.Comments);
                }
            }

            if (useNoTracking)
            {
                reportQueryable = reportQueryable.AsNoTracking();
            }

            return reportQueryable.Where(r => r.IsArchived == false);
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            var reportQueryable = GetReportQueryable(true);

            var report = await reportQueryable.SingleOrDefaultAsync(r => r.ReportId == id);

            if (report != null)
            {            

                return Serialize(report);
            }

            return "{}";
        }

        public async Task<string> GetLightListAsJsonAsync(string nameLike = null)
        {
            var reports = await GetReportQueryable(false)
                 .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
              .Where(r => r.IsArchived == false)
              .OrderBy(r => r.ReportId)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE)
              .ToListAsync();

            var reportsDto = reports.Select(r => new ReportListItemDto ( r.ReportId, r.Name, r.Status ));

            return Serialize(reportsDto);
        }

        public async Task<string> GetDetailedListAsJsonAsync(string nameLike = null)
        {
            var reportQueryable = GetReportQueryable(true);

            var reports = await reportQueryable
                 .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
                  .Where(r => r.IsArchived == false)
            .OrderBy(r => r.ReportId)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE)
            .ToListAsync();

            return Serialize(reports);
        }
    }
}
