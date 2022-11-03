namespace EFCoreProjections.Cmd.Dto
{
    public class ReportResponse
    {
        public ReportResponse(int itemCount, string resultAsJson)
        {
            ItemCount = itemCount;
            ResultAsJson = resultAsJson;
        }

        public int ItemCount { get; set; }

        public string ResultAsJson { get; set; }
    }
}
