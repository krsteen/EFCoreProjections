using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EFCorePerformance.Cmd.Stats
{
    public static class StatCsvWriter
    {
        public static void Write(List<RunStats> stats, string workingFolder)
        {
            var configuration = new CsvConfiguration(cultureInfo: new System.Globalization.CultureInfo("en-US"))
            {
                Delimiter = ";"
            };

            using (var writer = new StreamWriter($"{workingFolder}report.csv"))
            using (var csv = new CsvWriter(writer, configuration))
            {
                csv.WriteRecords(stats.OrderBy(s=> s.TestIndex).ThenBy(s=> s.ServiceIndex));
            }
        }
    }
}
