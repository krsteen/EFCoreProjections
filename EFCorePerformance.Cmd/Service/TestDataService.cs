using EFCorePerformance.Cmd.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EFCorePerformance.Cmd.Service
{
    public class TestDataService : ServiceBase
    {
        public TestDataService()
            : base()
        {
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

                //Reseed identity columns
                await connection.ExecuteAsync(CreateReseedFrom("ReportCommentsWithBasicIndex"));
                await connection.ExecuteAsync(CreateReseedFrom("ReportsWithBasicIndex"));
                await connection.ExecuteAsync(CreateReseedFrom("ReportConfigsWithBasicIndexes"));
                await connection.ExecuteAsync(CreateReseedFrom("ReportCommentsWithBetterIndex"));
                await connection.ExecuteAsync(CreateReseedFrom("ReportsWithBetterIndex"));
                await connection.ExecuteAsync(CreateReseedFrom("ReportConfigsWithBetterIndexes"));


                var reportStatusIndex = 0;
                var totalReportCounter = 1;               

                for (int reportConfigCounter = 1; reportConfigCounter < 100; reportConfigCounter++)
                {                
                    var sbReportConfig = new StringBuilder();
                    sbReportConfig.AppendLine(ConfigInsert("ReportConfigsWithBasicIndexes", reportConfigCounter, $"Report config basic index {reportConfigCounter}", $"Description for report config basic index {reportConfigCounter}", LongRandomText()));
                    sbReportConfig.AppendLine(ConfigInsert("ReportConfigsWithBetterIndexes", reportConfigCounter, $"Report config basic index {reportConfigCounter}", $"Description for report config basic index {reportConfigCounter}", LongRandomText()));
                    await connection.ExecuteAsync(sbReportConfig.ToString());

                
                    for (var innerReportCounter = 1; innerReportCounter < 1000; innerReportCounter++)
                    {
                        bool isArcived = innerReportCounter % 5 == 0;

                        string reportStatus = GetReportStatus(reportStatusIndex);

                        //Create report
                        var sbReport = new StringBuilder();

                        sbReport.AppendLine(ReportInsert("ReportsWithBasicIndex", $"Report basic index {totalReportCounter}", $"Description for report basic index {totalReportCounter}", isArcived, reportStatus, reportConfigCounter));
                        sbReport.AppendLine(ReportInsert("ReportsWithBetterIndex", $"Report basic index {totalReportCounter}", $"Description for report basic index {totalReportCounter}", isArcived, reportStatus, reportConfigCounter));
                        await connection.ExecuteAsync(sbReport.ToString());
                     

                        //Create report comments
                        var sbComment = new StringBuilder();

                        for (var reportCommentCounter = 1; reportCommentCounter < 10; reportCommentCounter++)
                        {
                            var commentText = $"Report {totalReportCounter} was a really crappy report";

                            sbComment.AppendLine(CommentInsert("ReportCommentsWithBasicIndex", totalReportCounter, commentText));
                            sbComment.AppendLine(CommentInsert("ReportCommentsWithBetterIndex", totalReportCounter, commentText));
                        }

                        await connection.ExecuteAsync(sbComment.ToString());

                        if (reportStatusIndex == 3)
                        {
                            reportStatusIndex = 0;
                        }
                        else
                        {
                            reportStatusIndex++;
                        }
                        
                        totalReportCounter++;
                    }

                    await Db.SaveChangesAsync();
                }

            }
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

        static string CreateReseedFrom(string tableName)
        {
            return $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
        }    

        static string ConfigInsert(string tableName, int id, string name, string description, string veryUsefulInfo)
        {
            //ReportConfigsWithBasicIndexes
            return $"INSERT INTO [dbo].[{tableName}] ([Name],[Description],[VeryUsefulInformation]) VALUES ('{name}', '{description}', '{veryUsefulInfo}');";
        }

        static string ReportInsert(string tableName, string name, string description, bool isArchived, string status, int configId)
        {
            var isarchivedBit = isArchived ? 1 : 0;
            return $"INSERT INTO [dbo].[{tableName}] ([Name],[Description],[IsArchived],[Status],[ConfigId]) VALUES ('{name}', '{description}', {isarchivedBit}, '{status}', {configId});";
        }

        static string CommentInsert(string tableName, int reportId, string comment)
        {
            return $"INSERT INTO [dbo].[{tableName}] ([ReportId],[Comment]) VALUES ({reportId}, '{comment}');";
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

