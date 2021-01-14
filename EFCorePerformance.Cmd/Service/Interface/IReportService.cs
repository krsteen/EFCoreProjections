using EFCorePerformance.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public interface IReportService
    {  
        Task<ReportResponse> GetReportByIdAsync(int id);
        Task<ReportResponse> GetDetailedReportListAsync(string nameFilter = null);
        Task<ReportResponse> GetLightReportListAsync(string nameFilter = null);
    }
}
