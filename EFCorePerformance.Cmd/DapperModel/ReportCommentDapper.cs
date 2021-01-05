using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePerformance.Cmd.DapperModel
{
    [Table("ReportCommentsWithBetterIndex")]
    public class ReportCommentDapper
    {
        public int CommentId { get; set; }     

        public string Comment { get; set; }
    }
}
