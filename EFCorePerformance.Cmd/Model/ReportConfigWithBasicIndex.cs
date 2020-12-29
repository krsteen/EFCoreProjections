using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportConfigWithBasicIndex
    {
        public int Id { get; set; }       

        public string Name { get; set; }

        public string Description { get; set; }

        public string VeryUsefulInformation { get; set; }

        public List<ReportWithBasicIndex> Reports { get; set; }

    }
}
