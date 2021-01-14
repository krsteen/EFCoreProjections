using EFCorePerformance.Cmd.DapperModel;
using EFCorePerformance.Cmd.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceEFBetterIndexProjection : ReportServiceEFBetterIndex, IReportService
    {    

        public ReportServiceEFBetterIndexProjection()
            : base(false, true)
        {
            
        }      

        public new async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false)
                 .TagWith(QueryTag("Report list light"))
                   .If(nameLike != null, c => c.Where(r => r.Name == nameLike))
              .OrderBy(r => r.ReportId)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE)
                .Select(r => new ReportListItemDto(r.ReportId, r.Name, r.Status));         

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = reports.Select(r => new ReportListItemDto(r.ReportId, r.Name, r.Status)).ToList();

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }       
    }
}
