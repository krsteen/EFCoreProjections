using EFCorePerformance.Cmd.Model.Dapper;
using EFCorePerformance.Cmd.Model.EF;

namespace EFCorePerformance.Cmd.Dto
{
    public class ReportCommentDto
    {
        public ReportCommentDto(ReportCommentWithBasicIndex comment)
        {
            CommentId = comment.CommentId;
            ReportId = comment.ReportId;
            Comment = comment.Comment;
        }

        public ReportCommentDto(ReportComment comment)
        {
            CommentId = comment.CommentId;
            ReportId = comment.ReportId;
            Comment = comment.Comment;
        }

        public ReportCommentDto(ReportCommentDapper comment)
        {
            CommentId = comment.CommentId;
            ReportId = comment.ReportId;
            Comment = comment.Comment;
        }

        public int CommentId { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }
    }
}
