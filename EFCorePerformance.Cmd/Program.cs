using EFCorePerformance.Cmd.Service;
using EFCorePerformance.Cmd.Stats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd
{
    class Program
    {
        static List<string> Summaries = new List<string>();
        static List<RunStats> Stats = new List<RunStats>();

        static async Task Main(string[] args)
        {
            //await ResetDatabase();
            await CallAllMethodsOfService(new ReportsServiceBasicIndexEF(), 0);
            await CallAllMethodsOfService(new ReportsServiceBetterIndexesAndDapper(), 1);

            StatCsvWriter.Write(Stats);

            foreach (var curSummaryItem in Summaries)
            {
                Lg(curSummaryItem, true);
            }
        }

        static async Task ResetDatabase()
        {
            var testDataService = new TestDataService();
            await testDataService.ResetDatabaseAndPopulateWithTestData();

            Lg("Summary");
        }

        static void AddToSummary(IReportsService service, string method, string jsonResult, double elapsed)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Summaries.Add($"{service.GetType().Name}, {method}: elapsed {(int)elapsed}ms, size {byteCount} ");


        }

        static void AddToStats(IReportsService service, int serviceIndex, int testIndex, string method, double elapsed, string jsonResult)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Stats.Add(new RunStats(serviceIndex, testIndex, service.GetType().Name, method, (int)elapsed, byteCount));
        }

        static async Task CallAllMethodsOfService(IReportsService service, int serviceIndex)
        {
            LgService(service, "Starting");

            double elapsedTotal = 0;
            var spElapsed = Stopwatch.StartNew();

            //Get single report
            var singleReportJson = await service.GetAsJsonAsync(1121);
            spElapsed.Stop();

            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;

            //LgResult("Fetched single report", singleReportJson, spElapsed.Elapsed.TotalMilliseconds);

            AddToSummary(service, "by id", singleReportJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToStats(service, serviceIndex, 0, "single item", spElapsed.Elapsed.TotalMilliseconds, singleReportJson);


            //Get report list
            spElapsed.Restart();
            var reportListJson = await service.GetListAsJsonAsync();
            spElapsed.Stop();

            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;

            //LgResult("Fetched list of reports", reportListJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToSummary(service, "list", reportListJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToStats(service, serviceIndex, 1, "list", spElapsed.Elapsed.TotalMilliseconds, reportListJson);
            LgService(service, $"Completed in {(int)elapsedTotal}");
        }

        static void Lg(string message, bool skipVerticalSpace = false)
        {
            if (skipVerticalSpace == false)
            {
                Console.WriteLine("");
            }

            Console.WriteLine(message);
        }

        static void LgResult(string message, string jsonResult, double elapsedMs)
        {
            Lg($"{message}");
            Console.WriteLine("");
            Lg("Result:", true);
            Lg(jsonResult, true);
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Lg($"Elapsed: {(int)elapsedMs} ms, size: {byteCount} bytes");
            Console.WriteLine("");
        }

        static void LgService(IReportsService service, string messageSuffix)
        {
            Lg($"{service.GetType().Name}: {messageSuffix}");
        }
    }
}
