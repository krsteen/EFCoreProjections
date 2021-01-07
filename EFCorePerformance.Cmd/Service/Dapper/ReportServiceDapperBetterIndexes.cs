using EFCorePerformance.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBetterIndexes : ReportServiceDapperBase, IReportService
    {
        public ReportServiceDapperBetterIndexes()
            : base(false)
        {
        }

        public async Task<ReportResponse> GetAsJsonAsync(int id)
        {
            return await base.GetAsJsonInternalAsync(id);        
        }

        public async Task<ReportResponse> GetDetailedListAsJsonAsync(string nameLike = null)
        {
            return await base.GetDetailedListAsJsonInternalAsync(nameLike);
            
        }

        public async Task<ReportResponse> GetLightListAsJsonAsync(string nameLike = null)
        {
            return await base.GetLightListAsJsonInternalAsync(nameLike);
        } 

    }
}
