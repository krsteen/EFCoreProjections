using Dapper;
using EFCorePerformance.Cmd.DapperModel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBetterIndexes : ReportServiceBase, IReportService
    {
        public ReportServiceDapperBetterIndexes(bool convertToDto)
            : base(convertToDto)
        {
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.Id as ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.Id as ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.Id as CommentId, cm.Comment
                            FROM [dbo].[ReportsWithBetterIndex] r";
               
                query = AddJoins(query);
                query = AddWhere(query);               

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,

                      (report, config, comment) => {

                          if (!reportDictionary.TryGetValue(report.ReportId, out ReportDapper reportEntry))
                          {
                              reportEntry = report;
                              reportEntry.Comments = new List<ReportCommentDapper>();
                              reportDictionary.Add(reportEntry.ReportId, reportEntry);
                          }

                          reportEntry.Config = config;
                          reportEntry.Comments.Add(comment);
                          return reportEntry;
                      },
                    
                    new { Id = id },
                      splitOn: "ConfigId, CommentId");
              
                return Serialize(reports.Distinct().SingleOrDefault());
            }           
        }

        public async Task<string> GetDetailedListAsJsonAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.Id as ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.Id as ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.Id as CommentId, cm.Comment
                            FROM [dbo].[ReportsWithBetterIndex] r";

                query = AddJoins(query);
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);             

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,
                     (report, config, comment) => {

                         if (!reportDictionary.TryGetValue(report.ReportId, out ReportDapper reportEntry))
                         {
                             reportEntry = report;
                             reportEntry.Comments = new List<ReportCommentDapper>();
                             reportDictionary.Add(reportEntry.ReportId, reportEntry);
                         }

                         reportEntry.Config = config;
                         reportEntry.Comments.Add(comment);
                         return reportEntry;
                     },
                      splitOn: "ConfigId, CommentId"
                     );

                return Serialize(reports.Distinct().ToList());
            }
        }

        public async Task<string> GetLightListAsJsonAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT r.Id, r.Name, r.Status FROM [dbo].[ReportsWithBetterIndex] r";           
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                await connection.OpenAsync();

                var reports = await connection.QueryAsync<ReportListItemDapper>(query);

                return Serialize(reports);
            }
        }

     

        string AddJoins(string baseQuery)
        {
            return baseQuery += " INNER JOIN [dbo].[ReportConfigsWithBetterIndexes] cnf ON r.ConfigId = cnf.Id INNER JOIN [dbo].[ReportCommentsWithBetterIndex] cm ON r.Id = cm.ReportId ";
        }

        string AddPaging(string baseQuery, int skip, int take)
        {
            return baseQuery += $" ORDER BY r.[Id] OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
        }

        string AddWhere(string baseQuery)
        {
            return baseQuery += " WHERE r.[Id] = @Id";
        }
    }
}
