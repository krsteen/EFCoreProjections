using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportWithBetterIndex
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public bool IsArchived { get; set; }

        [MaxLength(32)]
        public string Status { get; set; }     

        public List<ReportCommentWithBetterIndex> Comments { get; set; }

        public int ConfigId { get; set; }
        public ReportConfigWithBetterIndex Config { get; set; }
    }
}
