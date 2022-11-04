using EFCoreProjections.Cmd.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
{
    public class EfReportServiceWithProjectionNotArchived : ServiceBase, IReportService
    {    

        public EfReportServiceWithProjectionNotArchived()
            : base()
        {
            
        }

        public async Task<ReportResponse> GetByIdAsync(int id)
        {
            var reportsQueryable =
                     Db.Reports
                     .TagWith(QueryTag("Report by id WITH projection NON ARCHIVED"))
                     .Include(r => r.Config)
                     .Include(r => r.Comments)
                     .Where(r => r.ReportId == id && !r.IsArchived)
                     .AsNoTracking()
                     .Select(r => new ReportListItemDto() { ReportId = r.ReportId, Name = r.Name, Status = r.Status, ConfigName = r.Config.Name });

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }

        public async Task<ReportResponse> GetByNameAsync(string name)
        {
            var reportsQueryable =
                      Db.Reports
                      .TagWith(QueryTag("Report by name WITH projection NON ARCHIVED"))
                      .Include(r => r.Config)
                      .Include(r => r.Comments)
                      .Where(r => r.Name == name && !r.IsArchived)
                      .AsNoTracking()
                      .Select(r => new ReportListItemDto() { ReportId = r.ReportId, Name = r.Name, Status = r.Status, ConfigName = r.Config.Name });

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }

        public async Task<ReportResponse> GetListAsync(string nameLike)
        {
            var reportsQueryable =
                    Db.Reports
                    .TagWith(QueryTag("Report List WITH projection NON ARCHIVED"))
                    .Include(r => r.Config)
                    .Include(r => r.Comments)
                    .Where(r => r.IsArchived == false && r.Name.StartsWith(nameLike))
                    .OrderBy(r => r.ReportId)
                    .Skip(Constants.DEFAULT_SKIP)
                    .Take(Constants.DEFAULT_TAKE)
                    .AsNoTracking()
                    .Select(r => new ReportListItemDto() { ReportId = r.ReportId, Name = r.Name, Status = r.Status, ConfigName = r.Config.Name });

            var reports = await reportsQueryable.ToListAsync();

            var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);

            var result = new ReportResponse(reportsDto.Count, Serialize(reportsDto));

            return result;
        }
    }
}
