using System.Collections.Generic;

namespace EFCoreProjections.Cmd.Dto
{
    public class ReportDto
    {
        public ReportDto()
        {
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
