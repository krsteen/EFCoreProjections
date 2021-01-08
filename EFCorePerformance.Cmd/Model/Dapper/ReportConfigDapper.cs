using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePerformance.Cmd.Model.Dapper
{
    [Table("ReportConfigsWithBetterIndexes")]
    public class ReportConfigDapper : ReportConfig
    {
        public string ConfigName { get { return Name; } set { Name = value; } }

        public string ConfigDescription { get { return Description; } set { Description = value; } }
    }
}
