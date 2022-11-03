using EFCoreProjections.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
{
    public interface IReportService
    {
        Task<ReportResponse> GetByIdAsync(int id);
        Task<ReportResponse> GetByNameAsync(string name);
        Task<ReportResponse> GetListAsync(string nameFilter);      
    }
}
