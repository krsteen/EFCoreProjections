using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceEFBasicIndex : ReportServiceBase, IReportService
    {
        readonly bool useBadLazyLoad;
        readonly bool useNoTracking;

        public ReportServiceEFBasicIndex(bool useBadLazyLoad, bool useNoTracking)
            : base()
        {         
            this.useBadLazyLoad = useBadLazyLoad;
            this.useNoTracking = useNoTracking;
        }

        IQueryable<ReportWithBasicIndex> GetReportQueryable(bool anyIncludes)
        {
            var reportQueryable = Db.ReportsWithBasicIndex.AsQueryable();

            if (useNoTracking)
            {
                reportQueryable = reportQueryable.AsNoTracking();
            }

            if (anyIncludes)
            {
                reportQueryable = reportQueryable.Include(r => r.Config);

                if (useBadLazyLoad == false)
                {
                    reportQueryable = reportQueryable.Include(r => r.Comments);
                }
            }


            return reportQueryable;
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            var reportQueryable = GetReportQueryable(true);

            var report = await reportQueryable.SingleOrDefaultAsync(r => r.Id == id && r.IsArchived == false);

            if (report != null)
            {
                if (useBadLazyLoad)
                {
                    await AddCommentsInAnIncrediblyLazyWay(report);
                }

                return Serialize(report);
            }

            return "{}";
        }

        //Worst lazy load method ever!!
        async Task AddCommentsInAnIncrediblyLazyWay(ReportWithBasicIndex report)
        {
            report.Comments = await Db.ReportCommentsWithBasicIndex.Where(rc => rc.ReportId == report.Id).ToListAsync();
        }

        public async Task<string> GetLightListAsJsonAsync(string nameLike = null)
        {
            var reports = await GetReportQueryable(false)
                .If(nameLike != null, c=> c.Where(r=> r.Name.Contains(nameLike)))
              .Where(r => r.IsArchived == false)
              .OrderBy(r => r.Id)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE)
              .ToListAsync();

            var reportsDto = reports.Select(r => new { r.Id, r.Name, r.Status });

            return Serialize(reportsDto);
        }

        public async Task<string> GetDetailedListAsJsonAsync(string nameLike = null)
        {
            var reportQueryable = GetReportQueryable(true);

            var reports = await reportQueryable
                .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
                  .Where(r => r.IsArchived == false)
            .OrderBy(r => r.Id)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE)
            .ToListAsync();

            foreach (var currentReport in reports)
            {
                await AddCommentsInAnIncrediblyLazyWay(currentReport);
            }

            return Serialize(reports);
        }
    }
}
