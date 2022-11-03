using EFCoreProjections.Cmd.Dto;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
{
    public interface IReportService
    {
        Task<ReportResponse> GetListAsync(string nameFilter);      
    }
}
