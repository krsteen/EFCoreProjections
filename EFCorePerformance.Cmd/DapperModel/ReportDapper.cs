using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePerformance.Cmd.DapperModel
{
    [Table("ReportsWithBetterIndex")]
    public class ReportDapper
    {
        public int ReportId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsArchived { get; set; }

        public string Status { get; set; }

        public int ConfigId { get; set; }

        public List<ReportCommentDapper> Comments { get; set; }


        public ReportConfigDapper Config { get; set; }
    }
}
