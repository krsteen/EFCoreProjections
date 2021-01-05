using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePerformance.Cmd.DapperModel
{
    [Table("ReportConfigsWithBetterIndexes")]
    public class ReportConfigDapper
    {        
        public int ConfigId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string VeryUsefulInformation { get; set; }
    }
}
