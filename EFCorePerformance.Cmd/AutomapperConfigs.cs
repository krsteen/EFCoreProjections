using AutoMapper;
using EFCorePerformance.Cmd.Dto;
using EFCorePerformance.Cmd.Model;

namespace EFCorePerformance.Cmd
{
    public class AutomapperConfigs : Profile
    {
        public AutomapperConfigs() {
            CreateMap<Report, ReportDto>();

            CreateMap<Report, ReportListItemDto>();

            CreateMap<ReportConfig, ReportConfigDto>();

            CreateMap<ReportComment, ReportCommentDto>();
        }

    }
}
