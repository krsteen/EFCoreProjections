using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportsServiceBetterIndexesAndDapper : ServiceBase, IReportsService
    {
        public ReportsServiceBetterIndexesAndDapper()
            :base()
        {
        }

        public async Task<ReportWithBasicIndex> GetById(int id)
        {
            var report = await _db.ReportsWithBasicIndex
                .Include(r => r.Config)
                //.Include(r => r.Comments)
                .SingleOrDefaultAsync();

            await AddCommentsInAnIncrediblyLazyWay(report);

            return report;
        }

        //Worst lazy load method ever!!
        async Task AddCommentsInAnIncrediblyLazyWay(ReportWithBasicIndex report)
        {
            report.Comments = await _db.ReportCommentsWithBasicIndex.Where(rc => rc.ReportId == report.Id).ToListAsync();           
        }

        public async Task<List<ReportWithBasicIndex>> ReportList()
        {
            var reports = await _db.ReportsWithBasicIndex
              .Include(r => r.Config)
              //.Include(r => r.Comments)
              .ToListAsync();

            foreach(var currentReport in reports)
            {
                await AddCommentsInAnIncrediblyLazyWay(currentReport);
            }

            return reports;
        }
    }
}
