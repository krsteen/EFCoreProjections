using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBasicIndexes : ReportServiceDapperBase, IReportService
    {
        public ReportServiceDapperBasicIndexes()
            : base(true)
        {
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            return await base.GetAsJsonInternalAsync(id);        
        }

        public async Task<string> GetDetailedListAsJsonAsync(string nameLike = null)
        {
            return await base.GetDetailedListAsJsonInternalAsync(nameLike);            
        }

        public async Task<string> GetLightListAsJsonAsync(string nameLike = null)
        {
            return await base.GetLightListAsJsonInternalAsync(nameLike);
        } 
    }
}
