using EFCorePerformance.Cmd.Dto;
using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class EfReportService : ServiceBase, IReportService
    {
        protected readonly bool useNoTracking;

        public EfReportService(bool useNoTracking)
            : base()
        {
            this.useNoTracking = useNoTracking;
        }     

        public async Task<ReportResponse> GetReportByIdAsync(int id)
        {
            var reportQueryable = GetReportQueryable();

            reportQueryable = reportQueryable.Where(r => r.ReportId == id);
            reportQueryable = reportQueryable.TagWith(QueryTag("Report by Id"));

            var report = await reportQueryable.SingleOrDefaultAsync();

            if (report != null)
            {
                var reportDto = Mapper.Map<ReportDto>(report);
                var result = new ReportResponse(1, Serialize(reportDto));

                return result;
            }

            return new ReportResponse(0, "");
        }

        public async Task<ReportResponse> GetDetailedReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false);

            reportsQueryable = reportsQueryable
                 .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
                 .TagWith(QueryTag("Detailed list light"))
            .OrderBy(r => r.ReportId)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE);

            var reports = await reportsQueryable.ToListAsync();

            var reportDtos = Mapper.Map<List<ReportDto>>(reports);

            var result = new ReportResponse(reportDtos.Count(), Serialize(reportDtos));

            return result;
        }

        public async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false)
                   .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
                   .TagWith(QueryTag("Report list light"))
              .OrderBy(r => r.ReportId)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE);

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }

      

        protected IQueryable<Report> GetReportQueryable(bool withIncludes = true)
        {
            if (useNoTracking)
            {
                Db.ChangeTracker.QueryTrackingBehavior = useNoTracking ? QueryTrackingBehavior.NoTracking : QueryTrackingBehavior.TrackAll;
            }

            var reportQueryable = Db.Reports
                        .If(withIncludes, x=> x.Include(r => r.Config))
                        .If(withIncludes, x => x.Include(r => r.Comments));

            return reportQueryable.Where(r => r.IsArchived == false);
        }
    }
}
