using EFCorePerformance.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public interface IReportService
    {  
        Task<ReportResponse> GetAsJsonAsync(int id);
        Task<ReportResponse> GetDetailedListAsJsonAsync(string nameLike = null);
        Task<ReportResponse> GetLightListAsJsonAsync(string nameLike = null);
    }
}
