using EFCorePerformance.Cmd.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public interface IReportsService
    {
        Task<ReportWithBasicIndex> GetById(int id);

        Task<List<ReportWithBasicIndex>> ReportList();
    }
}
