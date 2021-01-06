using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model.EF
{
    public class ReportWithBetterIndex : Report
    {
        public List<ReportCommentWithBetterIndex> Comments { get; set; }
      
        public ReportConfigWithBetterIndex Config { get; set; }
    }
}
