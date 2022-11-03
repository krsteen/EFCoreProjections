namespace EFCoreProjections.Cmd.Model
{

    public class ReportCommentBase
    {
        public int CommentId { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }

    }

    public class ReportComment : ReportCommentBase
    {
        public Report Report { get; set; }
    }

   
}
