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

            var clearCacheService = new ClearDbCacheService();

            //EF Detailed list, bad lazy load
            await clearCacheService.ClearCache();
            await CallAllMethodsOfService(new ReportServiceEF(false, true, false), 0, "EF, Basic Index, Bad lazy load, No DTO");

            //EF Detailed list, normal include
            await clearCacheService.ClearCache();
            await CallAllMethodsOfService(new ReportServiceEF(false, false, false), 2, "EF, Basic Index, Correct Include, No DTO");

            //EF Single by Id

            //Dapper single by Id

            //EF Light list, normal include, projection

            //EF Light list, normal include, projection, good indexing

            //Dapper Light list, good indexing

            await clearCacheService.ClearCache();
            await CallAllMethodsOfService(new ReportServiceEF(false, true, true), 3, "EF, Basic Index, Correct Include, No DTO, AsNoTracking");

            await CallAllMethodsOfService(new ReportServiceEF(true, true, true), 4, "EF, Basic Index, Correct Include, DTO, AsNoTracking");

            await clearCacheService.ClearCache();
            await CallAllMethodsOfService(new ReportServiceDapperBetterIndexes(true), 10, "Dapper, Basic Indexed, DTO");     

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

        static async Task CallAllMethodsOfService(IReportService service, int serviceIndex, string scenarioName)
        {
            LgService(service, "Starting");

            double elapsedTotal = 0;
            var spElapsed = Stopwatch.StartNew();

            //Get single report
            var singleReportJson = await service.GetAsJsonAsync(1121);
            spElapsed.Stop();
            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds; 
            AddToSummary(service, "by id", singleReportJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToStats(serviceIndex, 0, scenarioName, "single item", spElapsed.Elapsed.TotalMilliseconds, singleReportJson);           

            //get light report list
            spElapsed.Restart();
            var reporLightListJson = await service.GetLightListAsJsonAsync();
            spElapsed.Stop();
            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
            AddToSummary(service, "light list", reporLightListJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToStats(serviceIndex, 1, scenarioName, "light list", spElapsed.Elapsed.TotalMilliseconds, reporLightListJson);

            //get detailed report list
            spElapsed.Restart();
            var reportListJson = await service.GetDetailedListAsJsonAsync();
            spElapsed.Stop();
            elapsedTotal += spElapsed.Elapsed.TotalMilliseconds;
            AddToSummary(service, "heavy list", reportListJson, spElapsed.Elapsed.TotalMilliseconds);
            AddToStats(serviceIndex, 2, scenarioName, "detailed list", spElapsed.Elapsed.TotalMilliseconds, reportListJson);

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
