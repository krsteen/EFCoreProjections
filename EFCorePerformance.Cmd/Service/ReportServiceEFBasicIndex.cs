using EFCorePerformance.Cmd.DapperModel;
using EFCorePerformance.Cmd.Model.EF;
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

            return reportQueryable.Where(r=>r.IsArchived == false);
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            var reportQueryable = GetReportQueryable(true);

            var report = await reportQueryable.SingleOrDefaultAsync(r => r.ReportId == id);

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
                .If(nameLike != null, c=> c.Where(r=> r.Name.StartsWith(nameLike)))             
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
                .If(nameLike != null, c => c.Where(r => r.Name.StartsWith(nameLike)))              
            .OrderBy(r => r.ReportId)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE)
            .ToListAsync();

            if (useBadLazyLoad)
            {
                foreach (var currentReport in reports)
                {
                    await AddCommentsInAnIncrediblyLazyWay(currentReport);
                }
            }

            return Serialize(reports);
        }
    }
}
