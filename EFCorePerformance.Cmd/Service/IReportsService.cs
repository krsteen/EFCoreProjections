using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public interface IReportsService
    {  
        Task<string> GetAsJsonAsync(int id);

        Task<string> GetListAsJsonAsync();
    }
}
