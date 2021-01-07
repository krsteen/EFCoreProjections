using Dapper;
using EFCorePerformance.Cmd.Dto;
using EFCorePerformance.Cmd.Model.Dapper;
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

        protected async Task<ReportResponse> GetAsJsonInternalAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.CommentId, cm.Comment
                            FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";

                query = AddJoins(query);
                query = AddIdWhere(query);

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,

                      (report, config, comment) =>
                      {

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

                var reportDto = new ReportDto(reports.Distinct().SingleOrDefault());

                var result = new ReportResponse(1, Serialize(reportDto));

                return result;
            }
        }

        protected async Task<ReportResponse> GetDetailedListAsJsonInternalAsync(string nameLike = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.ReportId, r.Name, r.Description, r.IsArchived, r.Status,
                            cnf.ConfigId, cnf.Name, cnf.Description, cnf.VeryUsefulInformation,
                            cm.CommentId, cm.Comment
                            FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";

                query = AddJoins(query);
                query = nameLike == null ? AddArchivedWhere(query) : AddNameWhere(query);
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,
                     (report, config, comment) =>
                     {

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
                     param: nameLike == null ? null : new { Name = $"{nameLike}" },
                      splitOn: "ConfigId, CommentId"

                     );

                var reportsDistinct = reports.Distinct().ToList();
                return new ReportResponse(reportsDistinct.Count, Serialize(reportsDistinct));
            }
        }

        public async Task<ReportResponse> GetLightListAsJsonInternalAsync(string nameLike = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT r.ReportId, r.Name, r.Status FROM [dbo].[ReportsWith" + PartOfTableName + "Index] r";
                query = nameLike == null ? AddArchivedWhere(query) : AddNameWhere(query);
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                await connection.OpenAsync();

                var reports = await connection.QueryAsync<ReportListItemDapper>(query,
                    param: nameLike == null ? null : new { Name = $"{nameLike}" }
                    );

                var result = new ReportResponse(reports.Count(), Serialize(reports));

                return result;
            }
        }

        string AddJoins(string baseQuery)
        {
            return baseQuery += " INNER JOIN [dbo].[ReportConfigsWith" + PartOfTableName + "Indexes] cnf ON r.ConfigId = cnf.ConfigId INNER JOIN [dbo].[ReportCommentsWith" + PartOfTableName + "Index] cm ON r.ReportId = cm.ReportId ";
        }

        string AddPaging(string baseQuery, int skip, int take)
        {
            return baseQuery += $" ORDER BY r.[ReportId] OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
        }

        string AddArchivedWhere(string baseQuery)
        {
            return baseQuery += " WHERE r.[IsArchived] = 0";
        }

        string AddIdWhere(string baseQuery)
        {
            return baseQuery += " WHERE r.[IsArchived] = 0 AND r.[ReportId] = @Id";
        }

        string AddNameWhere(string baseQuery)
        {
            return baseQuery += $" WHERE r.[IsArchived] = 0 AND r.[Name] = @Name";
        }
    }
}
