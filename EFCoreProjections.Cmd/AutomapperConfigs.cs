using AutoMapper;
using EFCoreProjections.Cmd.Dto;
using EFCoreProjections.Cmd.Model;

namespace EFCoreProjections.Cmd
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
