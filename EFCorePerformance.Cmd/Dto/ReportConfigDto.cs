using EFCorePerformance.Cmd.Model.Dapper;
using EFCorePerformance.Cmd.Model.EF;

namespace EFCorePerformance.Cmd.Dto
{
    public class ReportConfigDto
    {
        public ReportConfigDto(ReportConfigWithBasicIndex config)
        {
            ConfigId = config.ConfigId;
            Name = config.Name;
            Description = config.Description;
            VeryUsefulInformation = config.VeryUsefulInformation;
        }

        public ReportConfigDto(ReportConfigWithBetterIndex config)
        {
            ConfigId = config.ConfigId;
            Name = config.Name;
            Description = config.Description;
            VeryUsefulInformation = config.VeryUsefulInformation;
        }

        public ReportConfigDto(ReportConfigDapper config)
        {
            ConfigId = config.ConfigId;
            Name = config.Name;
            Description = config.Description;
            VeryUsefulInformation = config.VeryUsefulInformation;
        }

        public int ConfigId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string VeryUsefulInformation { get; set; }
       
    }
}
