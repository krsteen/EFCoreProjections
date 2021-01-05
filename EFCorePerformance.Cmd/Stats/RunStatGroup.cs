using System;
using System.Collections.Generic;
using System.Text;

namespace EFCorePerformance.Cmd.Stats
{
    public class RunStatGroup
    {
        public RunStatGroup(string name, List<RunStats> stats)
        {
            Name = name;
            Stats = stats;
        }

        public string Name { get; set; }

        public List<RunStats> Stats { get; set; }
    }
}
