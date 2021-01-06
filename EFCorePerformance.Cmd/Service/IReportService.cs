using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public interface IReportService
    {  
        Task<string> GetAsJsonAsync(int id);
        Task<string> GetDetailedListAsJsonAsync(string nameLike = null);
        Task<string> GetLightListAsJsonAsync(string nameLike = null);
    }
}
