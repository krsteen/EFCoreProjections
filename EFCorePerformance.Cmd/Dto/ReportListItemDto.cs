namespace EFCorePerformance.Cmd.DapperModel
{
    public class ReportListItemDto
    {
        public ReportListItemDto(int id, string name, string status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
    }
}
