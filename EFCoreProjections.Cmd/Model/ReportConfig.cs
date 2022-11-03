﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreProjections.Cmd.Model
{
    public class ReportConfigBase
    {
        public int ConfigId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public string VeryUsefulInformation { get; set; }
    }
    public class ReportConfig : ReportConfigBase
    {
        public List<Report> Reports { get; set; }
    }    
}
