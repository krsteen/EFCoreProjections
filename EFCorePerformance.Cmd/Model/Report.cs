using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportBase
    {
        public int ReportId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public bool IsArchived { get; set; }

        [MaxLength(32)]
        public string Status { get; set; }

        public int ConfigId { get; set; }

    }
    public class Report : ReportBase
    {
        public List<ReportComment> Comments { get; set; }

        public ReportConfig Config { get; set; }
    }

    public class ReportDapper : ReportBase
    {
        public string ReportName { get { return Name; } set { Name = value; } }

        public string ReportDescription { get { return Description; } set { Description = value; } }

        public List<ReportCommentDapper> Comments { get; set; }

        public ReportConfigDapper Config { get; set; }
    }
}
