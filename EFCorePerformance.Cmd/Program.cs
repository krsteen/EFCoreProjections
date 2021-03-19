using EFCorePerformance.Cmd.Dto;
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
        const int TEST_ITERATIONS = 100;

        static readonly List<string> Summaries = new List<string>();
        static readonly List<RunStats> Stats = new List<RunStats>();
        static readonly TestDataService TestDataService = new TestDataService();

        static readonly string WorkingFolder = $"D:\\Workspace\\EFCorePerformancev2\\{DateTime.Now.ToFileTime()}\\";

        static async Task Main()
        {
            //await ResetDatabase();

            Directory.CreateDirectory(WorkingFolder);        

            await RunTestsOnService(new EfReportService(useNoTracking: false), 0, "EF Core", 0, 1, 2);

            await RunTestsOnService(new EfReportService(useNoTracking: true), 1, "EF Core AsNoTracking()", 0, 1, 2);

            await RunTestsOnService(new EfWithProjectionReportService(), 2, "EF Core Projection AsNoTracking()", 2);          

            await RunTestsOnService(new DapperReportService(), 3, "Dapper", 0, 1, 2);            

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

            ReportResponse reportResponse = null;

            if (testsToRunHs.Contains(0))
            {
                await clearCacheService.ClearCache();

                spElapsed.Restart();

                for (var testCount = 1; testCount <= TEST_ITERATIONS; testCount++)
                {
                    reportResponse = await service.GetReportByIdAsync(idOfActiveReport);
                }                    

                spElapsed.Stop();

                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;

                AddToSummary(service, "single report by id", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS);
                AddToStats(serviceIndex, 0, scenarioName, "single report by id", spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(1))
            {
                await clearCacheService.ClearCache();

                spElapsed.Restart();

                for (var testCount = 1; testCount <= TEST_ITERATIONS; testCount++)
                {
                    reportResponse = await service.GetDetailedReportListAsync(Constants.REPORT_NAME_SEARCH);
                }                   
               
                spElapsed.Stop();

                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "detailed list with search", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS);
                AddToStats(serviceIndex, 1, scenarioName, "detailed list with search, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }

            if (testsToRunHs.Contains(2))
            {
                await clearCacheService.ClearCache();

                spElapsed.Restart();

                for (var testCount = 1; testCount <= TEST_ITERATIONS; testCount++)
                {
                    reportResponse = await service.GetLightReportListAsync(Constants.REPORT_NAME_SEARCH);
                }                 

                spElapsed.Stop();
                elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
                AddToSummary(service, "light list with search", reportResponse.ResultAsJson, spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS);
                AddToStats(serviceIndex, 2, scenarioName, "light list with search, limit to 100 items", spElapsed.Elapsed.TotalMilliseconds / TEST_ITERATIONS, reportResponse.ResultAsJson, reportResponse.ItemCount);
            }          

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

        static void AddToStats(int serviceIndex, int testIndex, string testName, string method, double elapsed, string jsonResult, int itemCount)
        {
            var byteCount = Encoding.UTF8.GetByteCount(jsonResult);
            Stats.Add(new RunStats(serviceIndex, testIndex, testName, method, (int)elapsed, itemCount, byteCount));

            File.WriteAllTextAsync($"{WorkingFolder}_{serviceIndex}_{testIndex}_{testName.Replace(", ", "")}_{method.Replace(" ", "")}.json", jsonResult);
        }
    }
}
