using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePerformance.Cmd.Model.Dapper
{
    [Table("ReportsWithBetterIndex")]
    public class ReportDapper : Report
    {
        public List<ReportCommentDapper> Comments { get; set; }


        public ReportConfigDapper Config { get; set; }
    }
}
