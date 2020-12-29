using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportWithBasicIndex
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsArchived { get; set; }

        public string Status { get; set; }     

        public List<ReportCommentWithBasicIndex> Comments { get; set; }

        public int ConfigId { get; set; }
        public ReportConfigWithBasicIndex Config { get; set; }
    }
}
