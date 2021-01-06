using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model.EF
{
    public class ReportWithBasicIndex : Report
    {
        public List<ReportCommentWithBasicIndex> Comments { get; set; }
       
        public ReportConfigWithBasicIndex Config { get; set; }
    }
}
