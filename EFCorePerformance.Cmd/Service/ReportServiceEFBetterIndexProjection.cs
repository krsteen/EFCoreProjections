using EFCorePerformance.Cmd.DapperModel;
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

        public new async Task<string> GetLightListAsJsonAsync()
        {
            var reports = await GetReportQueryable(false)
              .Where(r => r.IsArchived == false)            
              .OrderBy(r => r.Id)
              .Skip(Constants.DEFAULT_SKIP)
              .Take(Constants.DEFAULT_TAKE)
                .Select(r => new ReportListItemDto(r.Id, r.Name, r.Status))
              .ToListAsync();          

            return Serialize(reports);
        }       
    }
}
