namespace EFCorePerformance.Cmd.DapperModel
{
    public class ReportListItemDto
    {
        public ReportListItemDto(int id, string name, string status)
        {
            ReportId = id;
            Name = name;
            Status = status;
        }

        public int ReportId { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
    }
}
