using EFCorePerformance.Cmd.Service;
using EFCorePerformance.Cmd.Stats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd
{
    class Program
    {
        static readonly List<string> Summaries = new List<string>();
        static readonly List<RunStats> Stats = new List<RunStats>();
        static readonly TestDataService TestDataService = new TestDataService();

        static string WorkingFolder = $"D:\\Workspace\\EFCorePerformance\\{DateTime.Now.ToFileTime()}\\";

        static async Task Main(string[] args)
        {
            //await ResetDatabase();

            Directory.CreateDirectory(WorkingFolder);

            await RunTestsOnService(new ReportServiceEFBasicIndex(useBadLazyLoad: true, useNoTracking: false), 0, "EF Basic index Home made lazy load", 0, 3, 4);

            await RunTestsOnService(new ReportServiceEFBasicIndex(useBadLazyLoad: false, useNoTracking: false), 1, "EF Basic index Include", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBasicIndex(useBadLazyLoad: false, useNoTracking: true), 2, "EF Basic index Include AsNoTracking", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBetterIndex(useBadLazyLoad: false, useNoTracking: true), 3, "EF Better index Include AsNoTracking", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceEFBetterIndexProjection(), 4, "EF Better index Include AsNoTracking Projection", 1, 2);

            await RunTestsOnService(new ReportServiceDapperBasicIndexes(), 5, "Dapper basic index", 0, 1, 2, 3, 4);

            await RunTestsOnService(new ReportServiceDapperBetterIndexes(), 6, "Dapper better index", 0, 1, 2, 3, 4);

            StatCsvWriter.Write(Stats, WorkingFolder);

            foreach (var curSummaryItem in Summaries)
            {
                Lg(curSummaryItem, true);
            }
        }

     
     
        static async Task RunTestsOnService(IReportService service, int serviceIndex, string scenarioName, params int[] testsToRun)
        {
            var idOfActiveReport = await TestDataService.GetReportIdToSearchFor();

            var clearCacheService = new ClearDbCacheService();

            LgService(service, "Starting");

            var spElapsed = new Stopwatch();

            double elapsedTotal = 0;           

            var testsToRunHs = new HashSet<int>(testsToRun);

            if (testsToRunHs.Contains(0))
            {
                await clearCacheService.ClearCache();

                spElapsed.Restart();
                var reportResponse = await service.GetAsJsonAsync(idOfActiveReport);
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "by id", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 0, scenarioName, "single item", spElapsed.Elapsed.TotalMilliseconds, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(1))
            {
                await clearCacheService.ClearCache();
               
                spElapsed.Restart();
                var reportResponse = await service.GetLightListAsJsonAsync();
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "light list", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 1, scenarioName, "light list, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(2))
            {
                await clearCacheService.ClearCache();
              
                spElapsed.Restart();
                var reportResponse = await service.GetLightListAsJsonAsync(Constants.REPORT_NAME_SEARCH);
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "light list with search", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 2, scenarioName, "light list with search, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(3))
            {
                await clearCacheService.ClearCache();
               
                spElapsed.Restart();
                var reportResponse = await service.GetDetailedListAsJsonAsync();
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "detailed list", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 3, scenarioName, "detailed list, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(4))
            {
                await clearCacheService.ClearCache();
           
                spElapsed.Restart();
                var reportListJson = await service.GetDetailedListAsJsonAsync(Constants.REPORT_NAME_SEARCH);
                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "detailed list with search", reportListJson.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds);
                AddToStats(serviceIndex, 4, scenarioName, "detailed list with search, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds, reportListJson.ResultAsJson, reportListJson.ItemCount);
            }

            LgService(service, $"Completed in {(int)elapsedTotal}");
        }

        static async Task ResetDatabase()
        {
            throw new NotImplementedException("Hell no!");
           
            await TestDataService.ResetDatabaseAndPopulateWithTestData();

            Lg("Summary");
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

        static void AddToSummary(IReportService service, string method, string jsonResult, double elapsed)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Summaries.Add($"{service.GetType().Name}, {method}: elapsed {(int)elapsed}ms, size {byteCount} ");
        }

        static void AddToStats(int serviceIndex, int testIndex, string testName, string method, double elapsed, string jsonResult, int itemCount)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Stats.Add(new RunStats(serviceIndex, testIndex, testName, method, (int)elapsed, itemCount, byteCount));

            File.WriteAllTextAsync($"{WorkingFolder}_{serviceIndex}_{testIndex}_{testName.Replace(", ", "")}_{method.Replace(" ", "")}.json", jsonResult);
        }

    }
}
