using System;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportCommentWithBasicIndex
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public string Comment { get; set; }
       
        public ReportWithBasicIndex Report { get; set; }
    }
}
