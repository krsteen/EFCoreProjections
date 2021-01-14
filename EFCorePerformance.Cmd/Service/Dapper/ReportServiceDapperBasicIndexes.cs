using EFCorePerformance.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBasicIndexes : ReportServiceDapperBase, IReportService
    {
        public ReportServiceDapperBasicIndexes()
            : base(true)
        {
        }

        public async Task<ReportResponse> GetReportByIdAsync(int id)
        {
            return await base.GetReportByIdInternalAsync(id);        
        }

        public async Task<ReportResponse> GetDetailedReportListAsync(string nameLike = null)
        {
            return await base.GetDetailedReportListInternalAsync(nameLike);            
        }

        public async Task<ReportResponse> GetLightReportListAsync(string nameLike = null)
        {
            return await base.GetLightReportListInternalAsync(nameLike);
        } 
    }
}
