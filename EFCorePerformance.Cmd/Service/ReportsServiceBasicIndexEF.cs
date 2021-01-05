using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportsServiceBasicIndexEF : ServiceBase, IReportsService
    {
        public ReportsServiceBasicIndexEF()
            :base()
        {
        }
      

        public async Task<string> GetAsJsonAsync(int id)
        {
            var report = await Db.ReportsWithBasicIndex
                .Include(r => r.Config)
                //.Include(r => r.Comments)
                .SingleOrDefaultAsync(r=> r.Id == id);

            if(report != null)
            {
                await AddCommentsInAnIncrediblyLazyWay(report);
                return Serialize(report);
            }

            return "{}";
        }

        //Worst lazy load method ever!!
        async Task AddCommentsInAnIncrediblyLazyWay(ReportWithBasicIndex report)
        {
            report.Comments = await Db.ReportCommentsWithBasicIndex.Where(rc => rc.ReportId == report.Id).ToListAsync();           
        }

        public async Task<string> GetListAsJsonAsync()
        {
            var reports = await Db.ReportsWithBasicIndex
              .Include(r => r.Config)
              //.Include(r => r.Comments)
              .OrderBy(r=> r.Id)
              .Skip(100)
              .Take(20)
              .ToListAsync();

            foreach(var currentReport in reports)
            {
                await AddCommentsInAnIncrediblyLazyWay(currentReport);
            }

            return Serialize(reports);
        }
    }
}
