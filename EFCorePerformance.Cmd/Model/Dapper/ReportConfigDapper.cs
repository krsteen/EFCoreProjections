namespace EFCorePerformance.Cmd.Model
{
    public class ReportConfigDapper : ReportConfigBase
    {
        public string ConfigName { get { return Name; } set { Name = value; } }

        public string ConfigDescription { get { return Description; } set { Description = value; } }
    }
}
