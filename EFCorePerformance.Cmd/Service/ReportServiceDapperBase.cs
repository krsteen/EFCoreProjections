using Dapper;
using EFCorePerformance.Cmd.DapperModel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBase : ReportServiceBase
    {
        protected string PartOfTableName;

        public ReportServiceDapperBase(bool useBasicIndexed)
            : base()
        {
            if (useBasicIndexed)
            {
                PartOfTableName = "Basic";
            }
            else
            {
                PartOfTableName = "Better";
            }
        }

        protected async Task<string> GetAsJsonInternalAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.Id as ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.Id as ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.Id as CommentId, cm.Comment
                            FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";
               
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

        protected async Task<string> GetDetailedListAsJsonInternalAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.Id as ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.Id as ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.Id as CommentId, cm.Comment
                            FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";

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

        public async Task<string> GetLightListAsJsonInternalAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT r.Id, r.Name, r.Status FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";           
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                await connection.OpenAsync();

                var reports = await connection.QueryAsync<ReportListItemDapper>(query);

                return Serialize(reports);
            }
        }

     

        string AddJoins(string baseQuery)
        {
            return baseQuery += " INNER JOIN [dbo].[ReportConfigsWith" + PartOfTableName + "Indexes] cnf ON r.ConfigId = cnf.Id INNER JOIN [dbo].[ReportCommentsWith" + PartOfTableName + "Index] cm ON r.Id = cm.ReportId ";
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
