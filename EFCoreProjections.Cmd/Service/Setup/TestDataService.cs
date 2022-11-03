using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
{
    public class TestDataService : ServiceBase
    {
        public TestDataService()
            : base()
        {
        }

        public async Task<int> GetReportIdToSearchFor()
        {
            return (await Db.Reports.Where(r => r.ReportId > 50000).FirstOrDefaultAsync()).ReportId;
        }


        public async Task ResetDatabaseAndPopulateWithTestData()
        {
            var randomCommentCount = new Random();

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(DeleteStatement("ReportComments"));
                await connection.ExecuteAsync(DeleteStatement("Reports"));
                await connection.ExecuteAsync(DeleteStatement("ReportConfigs"));            

                //Reseed identity columns
                await connection.ExecuteAsync(ReseedStatement("ReportComments"));
                await connection.ExecuteAsync(ReseedStatement("Reports"));
                await connection.ExecuteAsync(ReseedStatement("ReportConfigs")); 

                var reportStatusIndex = 0;
                var totalReportCounter = 1;               

                for (int reportConfigCounter = 1; reportConfigCounter < 200; reportConfigCounter++)
                {                
                    var sbReportConfig = new StringBuilder();
                    var randomText = LongRandomText();
                    sbReportConfig.AppendLine(ConfigInsert($"Report config {reportConfigCounter}", $"Description for report config {reportConfigCounter}", randomText));
                    await connection.ExecuteAsync(sbReportConfig.ToString());
                
                    for (var innerReportCounter = 1; innerReportCounter < 2000; innerReportCounter++)
                    {
                        bool isArcived = innerReportCounter % 5 == 0;

                        string reportStatus = GetReportStatus(reportStatusIndex);

                        //Create report
                        var sbReport = new StringBuilder();

                        sbReport.AppendLine(ReportInsert($"Report {totalReportCounter}", $"Description for report {totalReportCounter}", isArcived, reportStatus, reportConfigCounter));                   
                        await connection.ExecuteAsync(sbReport.ToString());                     

                        //Create report comments
                        var sbComment = new StringBuilder();

                        var commentCountForThisReport = randomCommentCount.Next(1, 30);

                        for (var reportCommentCounter = 1; reportCommentCounter < commentCountForThisReport; reportCommentCounter++)
                        {
                            var commentText = $"Report {totalReportCounter} was a really crappy report. {Guid.NewGuid().ToString().Substring(0, 5)}";
                            sbComment.AppendLine(CommentInsert(totalReportCounter, commentText));
                        }

                        if(sbComment.Length > 0)
                        {
                            await connection.ExecuteAsync(sbComment.ToString());
                        }                      

                        if (reportStatusIndex == 2)
                        {
                            reportStatusIndex = 0;
                        }
                        else
                        {
                            reportStatusIndex++;
                        }
                        
                        totalReportCounter++;
                    }                  
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

        static string DeleteStatement(string tableName)
        {
            return $"DELETE FROM [dbo].[{tableName}]";
        }

        static string ReseedStatement(string tableName)
        {
            return $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
        }    

        static string ConfigInsert(string name, string description, string veryUsefulInfo)
        {
            //ReportConfigsWithBasicIndexes
            return $"INSERT INTO [dbo].[ReportConfigs] ([Name],[Description],[VeryUsefulInformation]) VALUES ('{name}', '{description}', '{veryUsefulInfo}');";
        }

        static string ReportInsert(string name, string description, bool isArchived, string status, int configId)
        {
            var isarchivedBit = isArchived ? 1 : 0;
            return $"INSERT INTO [dbo].[Reports] ([Name],[Description],[IsArchived],[Status],[ConfigId]) VALUES ('{name}', '{description}', {isarchivedBit}, '{status}', {configId});";
        }

        static string CommentInsert(int reportId, string comment)
        {
            return $"INSERT INTO [dbo].[ReportComments] ([ReportId],[Comment]) VALUES ({reportId}, '{comment}');";
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

            return null;
        }

    }
}

