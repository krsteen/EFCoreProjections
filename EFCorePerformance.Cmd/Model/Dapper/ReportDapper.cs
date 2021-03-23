using System.Collections.Generic;

namespace EFCorePerformance.Cmd.Model
{
    public class ReportDapper : ReportBase
    {
        public string ReportName { get { return Name; } set { Name = value; } }

        public string ReportDescription { get { return Description; } set { Description = value; } }

        public List<ReportCommentDapper> Comments { get; set; }

        public ReportConfigDapper Config { get; set; }
    }
}
