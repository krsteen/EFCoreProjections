using EFCorePerformance.Cmd.Service;
using EFCorePerformance.Cmd.Stats;
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

            await RunTestsOnService(new ReportServiceEFBasicIndex(true, false), 0, "EF, Basic Index, Bad lazy load", 0, 1, 2);

            await RunTestsOnService(new ReportServiceEFBasicIndex(false, false), 1, "EF, Basic Index, Correct Include", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBasicIndex(true, true), 2, "EF, Basic Index, Correct Include, AsNoTracking", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBetterIndex(true, true), 3, "EF, Better Index, Correct Include, AsNoTracking", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBetterIndexProjection(), 4, "EF, Better Index, Correct Include, AsNoTracking, Projection", 1,2);

            await RunTestsOnService(new ReportServiceDapperBasicIndexes(), 5, "Dapper, Basic Indexed", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceDapperBetterIndexes(), 6, "Dapper, Better Indexed", 0, 1, 2, 3, 4);
          

            //Able to cause client side validaiton?

            //Same query without client side validation?       
          

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

        static void AddToSummary(IReportService service, string method, string jsonResult, double elapsed)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Summaries.Add($"{service.GetType().Name}, {method}: elapsed {(int)elapsed}ms, size {byteCount} ");
        }

        static void AddToStats(int serviceIndex, int testIndex, string testName, string method, double elapsed, string jsonResult)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Stats.Add(new RunStats(serviceIndex, testIndex, testName, method, (int)elapsed, byteCount));
        }

        static async Task RunTestsOnService(IReportService service, int serviceIndex, string scenarioName, params int[] testsToRun)
        {
            var clearCacheService = new ClearDbCacheService();
         
            LgService(service, "Starting");

            double elapsedTotal = 0;
            var spElapsed = Stopwatch.StartNew();

            var testsToRunHs = new HashSet<int>(testsToRun);

            if (testsToRunHs.Contains(0))
            {
                await clearCacheService.ClearCache();
                //Get single report
                var singleReportJson = await service.GetAsJsonAsync(1121);
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "by id", singleReportJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 0, scenarioName, "single item", spElapsed.Elapsed.TotalMilliseconds, singleReportJson);
            }

            if (testsToRunHs.Contains(1))
            {
                await clearCacheService.ClearCache();
                //get light report list
                spElapsed.Restart();
                var reporLightListJson = await service.GetLightListAsJsonAsync();
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "light list", reporLightListJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 1, scenarioName, "light list", spElapsed.Elapsed.TotalMilliseconds, reporLightListJson);

            }

            if (testsToRunHs.Contains(2))
            {
                await clearCacheService.ClearCache();
                //get light report list with search
                spElapsed.Restart();
                var reporLightListJson = await service.GetLightListAsJsonAsync("basic index 6");
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "light list with search", reporLightListJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 2, scenarioName, "light list with search", spElapsed.Elapsed.TotalMilliseconds, reporLightListJson);

            }

            if (testsToRunHs.Contains(3))
            {
                await clearCacheService.ClearCache();
                //get detailed report list
                spElapsed.Restart();
                var reportListJson = await service.GetDetailedListAsJsonAsync();
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "detailed list", reportListJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 3, scenarioName, "detailed list", spElapsed.Elapsed.TotalMilliseconds, reportListJson);

            }

            if (testsToRunHs.Contains(4))
            {
                await clearCacheService.ClearCache();
                //get detailed report list with search
                spElapsed.Restart();
                var reportListJson = await service.GetDetailedListAsJsonAsync("basic index 6");
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "detailed list with search", reportListJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 4, scenarioName, "detailed list with search", spElapsed.Elapsed.TotalMilliseconds, reportListJson);

            }

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

        static void LgService(IReportService service, string messageSuffix)
        {
            Lg($"{service.GetType().Name}: {messageSuffix}");
        }
    }
}
