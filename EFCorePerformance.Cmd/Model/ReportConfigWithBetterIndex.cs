using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportConfigWithBetterIndex
    {
        public int Id { get; set; }       

        public string Name { get; set; }

        public string Description { get; set; }

        public string VeryUsefulInformation { get; set; }

        public List<ReportWithBetterIndex> Reports { get; set; }

    }
}
