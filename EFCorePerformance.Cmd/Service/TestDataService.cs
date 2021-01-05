using EFCorePerformance.Cmd.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EFCorePerformance.Cmd.Service
{
    public class TestDataService : ServiceBase
    {
        public TestDataService()
            : base()
        {
        }

        static string LongRandomText()
        {
            var bogusText = "";

            for (int i = 0; i < 15; i++)
            {
                bogusText += Guid.NewGuid().ToString();
            }

            return bogusText;
        }

        static string CreateDeleteFrom(string tableName)
        {
            return $"DELETE FROM [dbo].[{tableName}]";
        }

        public async Task ResetDatabaseAndPopulateWithTestData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(CreateDeleteFrom("ReportCommentsWithBasicIndex"));
                await connection.ExecuteAsync(CreateDeleteFrom("ReportsWithBasicIndex"));
                await connection.ExecuteAsync(CreateDeleteFrom("ReportConfigsWithBasicIndexes"));

                await connection.ExecuteAsync(CreateDeleteFrom("ReportCommentsWithBetterIndex"));
                await connection.ExecuteAsync(CreateDeleteFrom("ReportsWithBetterIndex"));
                await connection.ExecuteAsync(CreateDeleteFrom("ReportConfigsWithBetterIndexes"));
            }

            var reportStatusIndex = 0;
        

            for (int reportConfigCounter = 1; reportConfigCounter < 100; reportConfigCounter++)
            {
                //Create report config
                var newConfigWithBasicIndex = new ReportConfigWithBasicIndex() { Name = $"Report config basic index {reportConfigCounter}", Description = $"Description for report config basic index {reportConfigCounter}", VeryUsefulInformation = LongRandomText() };
                Db.ReportConfigsWithBasicIndexes.Add(newConfigWithBasicIndex);

                var newConfigWithBetterIndex = new ReportConfigWithBetterIndex() { Name = $"Report config better index {reportConfigCounter}", Description = $"Description for report config better index {reportConfigCounter}", VeryUsefulInformation = LongRandomText() };
                Db.ReportConfigsWithBetterIndexes.Add(newConfigWithBetterIndex);

                for (var reportCounter = 1; reportCounter < 1000; reportCounter++)
                {
                    bool isArcived = reportCounter % 10 == 0;

                    string reportStatus = GetReportStatus(reportStatusIndex);

                    //Create report

                    var newReportWithBasicIndex = new ReportWithBasicIndex()
                    {                       
                        Name = $"Report basic index {reportCounter}",
                        Description = $"Description for report basic index {reportCounter}",
                        IsArchived = isArcived,
                        ConfigId = reportConfigCounter,
                        Status = reportStatus,
                        Comments = new List<ReportCommentWithBasicIndex>(),
                        Config = newConfigWithBasicIndex
                    };

                    Db.ReportsWithBasicIndex.Add(newReportWithBasicIndex);

                    var newReportWithBetterIndex = new ReportWithBetterIndex()
                    {                       
                        Name = $"Report better index {reportCounter}",
                        Description = $"Description for report better index {reportCounter}",
                        IsArchived = isArcived,
                        ConfigId = reportConfigCounter,
                        Status = reportStatus,
                        Comments = new List<ReportCommentWithBetterIndex>(),
                        Config = newConfigWithBetterIndex
                    };

                    Db.ReportsWithBetterIndex.Add(newReportWithBetterIndex);

                    //Create report comments
                    for (var reportCommentCounter = 1; reportCommentCounter < 16; reportCommentCounter++)
                    {
                        var commentText = $"Report {reportCounter} was a really crappy report";
                        newReportWithBasicIndex.Comments.Add(new ReportCommentWithBasicIndex() { Comment = commentText });
                        newReportWithBetterIndex.Comments.Add(new ReportCommentWithBetterIndex() { Comment = commentText });
                        reportCommentCounter++;
                    }

                    if (reportStatusIndex == 3)
                    {
                        reportStatusIndex = 0;
                    }
                    else
                    {
                        reportStatusIndex++;
                    }

                    reportCounter++;
                }

                await Db.SaveChangesAsync();
            }
        }

        static string GetReportStatus(int statusIndex)
        {
            if (statusIndex == 0)
            {
                return "draft";
            }
            else if (statusIndex == 1)
            {
                return "approved";
            }
            else if (statusIndex == 2)
            {
                return "revised";
            }
            else if (statusIndex == 3)
            {
                return "archived";
            }

            return null;
        }

    }
}

