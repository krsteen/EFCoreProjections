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

        public async Task<string> GetDetailedListAsJsonAsync()
        {
            return await base.GetDetailedListAsJsonInternalAsync();            
        }

        public async Task<string> GetLightListAsJsonAsync()
        {
            return await base.GetLightListAsJsonInternalAsync();
        } 
    }
}
