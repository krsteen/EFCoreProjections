﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;

namespace EFCoreProjections.Cmd.Stats
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
                csv.WriteRecords(stats);
            }
        }
    }
}
