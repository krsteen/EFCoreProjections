namespace EFCorePerformance.Cmd.Model
{
    public class ReportCommentWithBetterIndex
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }

        public ReportWithBetterIndex Report { get; set; }
    }
}
