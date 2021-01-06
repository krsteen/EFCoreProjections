using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model.EF
{
    public class ReportConfigWithBetterIndex : ReportConfig
    {
        public List<ReportWithBetterIndex> Reports { get; set; }
    }
}
