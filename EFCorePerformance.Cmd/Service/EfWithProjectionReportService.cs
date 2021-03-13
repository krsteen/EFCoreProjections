using EFCorePerformance.Cmd.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class EfWithProjectionReportService : EfReportService, IReportService
    {    

        public EfWithProjectionReportService()
            : base(true)
        {
            
        }      

        public new async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            var reportsQueryable = GetReportQueryable(false)
                 .TagWith(QueryTag("EF Projection - Report list light"))
                   .If(nameLike != null, c => c.Where(r => r.Name.Contains(nameLike)))
              .OrderBy(r => r.ReportId)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE)
                .Select(r => new ReportListItemDto() { ReportId = r.ReportId, Name = r.Name, Status = r.Status });         

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }       
    }
}
