namespace EFCorePerformance.Cmd.Model
{
    public class ReportComment
    {
        public int CommentId { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }

        public Report Report { get; set; }
    }
}
