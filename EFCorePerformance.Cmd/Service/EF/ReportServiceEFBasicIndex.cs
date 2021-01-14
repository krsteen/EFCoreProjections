using EFCorePerformance.Cmd.DapperModel;
using EFCorePerformance.Cmd.Dto;
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
            //DISABLE TRACKING
            if (useNoTracking)
            {
                Db.ChangeTracker.QueryTrackingBehavior = useNoTracking ? QueryTrackingBehavior.NoTracking : QueryTrackingBehavior.TrackAll;
            }

            //Could also use Db.AsNoTracking()....



            //COOL TRICK FROM EFORL, INSTEAD OF:
            //if (anyIncludes)
            //{
            //    reportQueryable = reportQueryable.Include(r => r.Config);

            //    if (useBadLazyLoad == false)
            //    {
            //        reportQueryable = reportQueryable.Include(r => r.Comments);
            //    }
            //}

            //USE:
            var reportQueryable = Db.ReportsWithBasicIndex
                .If(anyIncludes, x => x.Include(r => r.Config))
                .If(anyIncludes && useBadLazyLoad == false, r => r.Include(r => r.Comments));

            //TO AVOID JOINS, ADD .AsSplitQuery()

            return reportQueryable.Where(r => r.IsArchived == false);
        }

        public async Task<ReportResponse> GetReportByIdAsync(int id)
        {
            var reportQueryable = GetReportQueryable(true);

            //DONT PUT A .ToList, .FirstOrDefault()

            reportQueryable = reportQueryable.Where(r => r.ReportId == id);           

            var report = await reportQueryable.SingleOrDefaultAsync();

            if (report != null)
            {
                if (useBadLazyLoad)
                {
                    await AddCommentsInAnIncrediblyLazyWay(report);
                }

                var reportDto = new ReportDto(report);

                var result = new ReportResponse(1, Serialize(reportDto));

                return result;
            }

            return new ReportResponse(0, "");
        }

        //Worst lazy load method ever!!
        async Task AddCommentsInAnIncrediblyLazyWay(ReportWithBasicIndex report)
        {
            report.Comments = await Db.ReportCommentsWithBasicIndex.Where(rc => rc.ReportId == report.ReportId).ToListAsync();
        }

        public async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false)
               .If(nameLike != null, c => c.Where(r => r.Name == nameLike))
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
                 .If(nameLike != null, c => c.Where(r => r.Name == nameLike));

            reportsQueryable = reportsQueryable.OrderBy(r => r.ReportId)
            .Skip(Constants.DEFAULT_SKIP)
            .Take(Constants.DEFAULT_TAKE);

            var reports = await reportsQueryable.ToListAsync();

            if (useBadLazyLoad)
            {
                foreach (var currentReport in reports)
                {
                    await AddCommentsInAnIncrediblyLazyWay(currentReport);
                }
            }

            var reportDtos = reports.Select(r=> new ReportDto(r)).ToList();

            var result = new ReportResponse(reportDtos.Count, Serialize(reportDtos));

            return result;
        }
    }
}
