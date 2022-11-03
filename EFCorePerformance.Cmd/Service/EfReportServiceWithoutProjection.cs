using EFCoreProjections.Cmd.Dto;
using EFCoreProjections.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
{
    public class EfReportServiceWithoutProjection : ServiceBase, IReportService
    {
        public EfReportServiceWithoutProjection()
            : base()
        {

        }       

        public async Task<ReportResponse> GetListAsync(string nameLike)
        {
            var reportsQueryable =
                    Db.Reports
                    .TagWith(QueryTag("Report List WITHOUT projection"))
                    .Include(r => r.Config)
                    .Include(r => r.Comments)                    
                    .Where(r => r.IsArchived == false && r.Name.StartsWith(nameLike)) 
                    .OrderBy(r => r.ReportId)
                    .Skip(Constants.DEFAULT_SKIP)
                    .Take(Constants.DEFAULT_TAKE)
                    .AsNoTracking();

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }       
    }
}
