using EFCoreProjections.Cmd.Dto;
using EFCoreProjections.Cmd.Service;
using EFCoreProjections.Cmd.Stats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd
{
    class Program
    {
        const int TEST_ITERATIONS = 1;

        static readonly List<string> Summaries = new List<string>();
        static readonly List<RunStats> Stats = new List<RunStats>();
        static readonly TestDataService TestDataService = new TestDataService();

        static readonly string WorkingFolder = $"C:\\Appl\\Workspace\\EFCoreProjections\\{DateTime.Now.ToFileTime()}\\";

        static async Task Main()
        {
            //await ResetDatabase();

            Directory.CreateDirectory(WorkingFolder);          

            await RunTestsOnService(new EfReportServiceWithoutProjection(), "EF Core WITHOUT projection");

            await RunTestsOnService(new EfReportServiceWithProjection(), "EF Core WITH projection ALL");

            await RunTestsOnService(new EfReportServiceWithProjectionNotArchived(), "EF Core WITH projection ALL Non Archived");


            StatCsvWriter.Write(Stats, WorkingFolder);

            foreach (var curSummaryItem in Summaries)
            {
                Lg(curSummaryItem, true);
            }
        }


        static async Task RunTestsOnService(IReportService service, string scenarioName)
        {
            var idOfActiveReport = await TestDataService.GetReportIdToSearchFor();

            var clearCacheService = new ClearDbCacheService();

            LgService(service, "Starting");

            var spElapsed = new Stopwatch();

            double elapsedTotal = 0;

            ReportResponse reportResponse = null;


            await clearCacheService.ClearCache();

            spElapsed.Restart();

            for (var testCount = 1; testCount <= TEST_ITERATIONS; testCount++)
            {
                reportResponse = await service.GetListAsync(Constants.REPORT_NAME_SEARCH);
            }

            spElapsed.Stop();
            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
            AddToSummary(service, "List with filter", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS);
            AddToStats(scenarioName, "List with filter", spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS, reportResponse.ResultAsJson, reportResponse.ItemCount);


            LgService(service, $"Completed in {(int)elapsedTotal}");
        }

        static async Task ResetDatabase()
        {
            await TestDataService.ResetDatabaseAndPopulateWithTestData();
        }

        static void Lg(string message, bool skipVerticalSpace = false)
        {
            if (skipVerticalSpace == false)
            {
                Console.WriteLine("");
            }

            Console.WriteLine(message);
        }

        static void LgService(IReportService service, string messageSuffix)
        {
            Lg($"{service.GetType().Name}: {messageSuffix}");
        }

        static void AddToSummary(IReportService service, string method, string jsonResult, double elapsed)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Summaries.Add($"{service.GetType().Name}, {method}: elapsed {(int)elapsed}ms, size {byteCount} ");
        }

        static void AddToStats(string testName, string method, double elapsed, string jsonResult, int itemCount)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Stats.Add(new RunStats(testName, method, (int)elapsed, itemCount, byteCount));

            File.WriteAllTextAsync($"{WorkingFolder}_{testName.Replace(", ", "")}_{method.Replace(" ", "")}.json", jsonResult);
        }
    }
}
