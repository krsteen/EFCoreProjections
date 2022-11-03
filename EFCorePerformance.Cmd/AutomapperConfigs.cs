using AutoMapper;
using EFCoreProjections.Cmd.Dto;
using EFCoreProjections.Cmd.Model;

namespace EFCoreProjections.Cmd
{
    public class AutomapperConfigs : Profile
    {
        public AutomapperConfigs() {

            CreateMap<Report, ReportDto>();

            CreateMap<ReportDapper, ReportDto>();

            CreateMap<Report, ReportListItemDto>();



            CreateMap<ReportConfig, ReportConfigDto>();

            CreateMap<ReportConfigDapper, ReportConfigDto>();



            CreateMap<ReportComment, ReportCommentDto>();

            CreateMap<ReportCommentDapper, ReportCommentDto>();
        }

    }
}
