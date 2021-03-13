namespace EFCorePerformance.Cmd.Dto
{
    public class ReportCommentDto
    {
        public ReportCommentDto()
        {

        }             

        public int CommentId { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }
    }
}
