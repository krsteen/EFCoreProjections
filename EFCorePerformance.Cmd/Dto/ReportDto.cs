using EFCorePerformance.Cmd.Model;
using EFCorePerformance.Cmd.Model.Dapper;
using EFCorePerformance.Cmd.Model.EF;
using System.Collections.Generic;
using System.Linq;

namespace EFCorePerformance.Cmd.Dto
{
    public class ReportDto
    {
        public ReportDto(Report report)
        {
            ReportId = report.ReportId;
            Name = report.Name;
            Description = report.Description;
            IsArchived = report.IsArchived;
            Status = report.Status;
            ConfigId = report.ConfigId;
            Config = new ReportConfigDto(report.Config);
            Comments = report.Comments.Select(c=> new ReportCommentDto(c)).ToList();
        }

        public ReportDto(ReportWithBetterIndex report)
        {
            ReportId = report.ReportId;
            Name = report.Name;
            Description = report.Description;
            IsArchived = report.IsArchived;
            Status = report.Status;
            ConfigId = report.ConfigId;
            Config = new ReportConfigDto(report.Config);
            Comments = report.Comments.Select(c => new ReportCommentDto(c)).ToList();
        }

        public ReportDto(ReportDapper report)
        {
            ReportId = report.ReportId;
            Name = report.Name;
            Description = report.Description;
            IsArchived = report.IsArchived;
            Status = report.Status;
            ConfigId = report.ConfigId;
            Config = new ReportConfigDto(report.Config);
            Comments = report.Comments.Select(c => new ReportCommentDto(c)).ToList();
        }

        public int ReportId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool IsArchived { get; set; }
        
        public string Status { get; set; }

        public int ConfigId { get; set; }

        public ReportConfigDto Config { get; set; }

        public List<ReportCommentDto> Comments { get; set; }
    }
}
