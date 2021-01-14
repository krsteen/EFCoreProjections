using EFCorePerformance.Cmd.DapperModel;
using EFCorePerformance.Cmd.Dto;
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
            if (useNoTracking)
            {
                Db.ChangeTracker.QueryTrackingBehavior = useNoTracking ? QueryTrackingBehavior.NoTracking : QueryTrackingBehavior.TrackAll;
            }

            var reportQueryable = Db.ReportsWithBetterIndex
               
                          .If(anyIncludes, x => x.Include(r => r.Config))
                          .If(anyIncludes && useBadLazyLoad == false, r => r.Include(r => r.Comments));

            return reportQueryable.Where(r => r.IsArchived == false);
        }

        public async Task<ReportResponse> GetReportByIdAsync(int id)
        {
            var reportQueryable = GetReportQueryable(true);

            reportQueryable = reportQueryable.Where(r => r.ReportId == id);
            reportQueryable = reportQueryable.TagWith(QueryTag("Report by Id"));
            var report = await reportQueryable.SingleOrDefaultAsync();

            if (report != null)
            {
                var reportDto = new ReportDto(report);
                var result = new ReportResponse(1, Serialize(reportDto));

                return result;
            }

            return new ReportResponse(0, "");
        }

        public async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false)
                   .If(nameLike != null, c => c.Where(r => r.Name == nameLike))
                   .TagWith(QueryTag("Report list light"))
              .OrderBy(r => r.ReportId)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE);

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = reports.Select(r => new ReportListItemDto(r.ReportId, r.Name, r.Status)).ToList();

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }

        public async Task<ReportResponse> GetDetailedReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(true);

            reportsQueryable = reportsQueryable
                 .If(nameLike != null, c => c.Where(r => r.Name == nameLike))
                 .TagWith(QueryTag("Detailed list light"))
            .OrderBy(r => r.ReportId)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE);

            var reports = await reportsQueryable.ToListAsync();

            var reportDtos = reports.Select(r => new ReportDto(r)).ToList();

            var result = new ReportResponse(reportDtos.Count, Serialize(reportDtos));

            return result;
        }
    }
}
