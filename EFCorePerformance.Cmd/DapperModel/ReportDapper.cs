using System.Collections.Generic;

namespace EFCorePerformance.Cmd.DapperModel
{
    public class ReportDapper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsArchived { get; set; }

        public string Status { get; set; }

        public int ConfigId { get; set; }

        //public List<ReportCommentWithBetterIndex> Comments { get; set; }


        //public ReportConfigWithBetterIndex Config { get; set; }
    }
}
