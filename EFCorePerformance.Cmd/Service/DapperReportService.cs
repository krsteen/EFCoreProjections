using Dapper;
using EFCorePerformance.Cmd.Dto;
using EFCorePerformance.Cmd.Model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class DapperReportService : ServiceBase, IReportService
    {

        public DapperReportService()
            : base()
        {

        }

        public async Task<ReportResponse> GetReportByIdAsync(int reportId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"SELECT r.ReportId, r.Name as ReportName, r.Description as ReportDescription, r.IsArchived, r.Status, r.ConfigId,
                            cnf.ConfigId, cnf.Name as ConfigName, cnf.Description as ConfigDescription, cnf.VeryUsefulInformation,
                            cm.CommentId, cm.Comment
                            FROM [dbo].[Reports] r";

                query = AddJoins(query);
                query = AddIdWhere(query);

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,

                      (report, config, comment) =>
                      {
                          if (!reportDictionary.TryGetValue(report.ReportId, out ReportDapper reportEntry))
                          {
                              reportEntry = report;
                              reportEntry.Config = config;
                              reportEntry.Comments = new List<ReportCommentDapper>();
                              reportDictionary.Add(reportEntry.ReportId, reportEntry);                                                         
                          }

                          reportEntry.Comments.Add(comment);

                          return reportEntry;
                      },

                    new { Id = reportId },
                      splitOn: "ConfigId, CommentId");

                var reportsDistinct = reports.Distinct().SingleOrDefault();
                var reportDto = Mapper.Map<ReportDto>(reportsDistinct);

                var result = new ReportResponse(1, Serialize(reportDto));

                return result;
            }
        }

        public async Task<ReportResponse> GetDetailedReportListAsync(string nameFilter = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var reportDictionary = new Dictionary<int, ReportDapper>();

                var query = @"WITH ctepaging AS (SELECT r.ReportId, r.Name as ReportName, r.Description as ReportDescription, r.IsArchived, r.Status,
                            r.ConfigId
                            FROM [dbo].[Reports] r";

                query = nameFilter == null ? AddArchivedWhere(query) : AddNameWhere(query);
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                query += "), ctejoin as (";
                query += " SELECT p.* ";
                query += ", cnf.Name as ConfigName, cnf.Description as ConfigDescription, cnf.VeryUsefulInformation";
                query += ", cm.CommentId, cm.Comment";
                query += " FROM ctepaging p";
                query = AddJoins(query, "p");
                query += ") SELECT * FROM ctejoin ";

                var reports = await connection.QueryAsync<ReportDapper, ReportConfigDapper, ReportCommentDapper, ReportDapper>(query,
                     (report, config, comment) =>
                     {
                         if (!reportDictionary.TryGetValue(report.ReportId, out ReportDapper reportEntry))
                         {
                             reportEntry = report;
                             reportEntry.Config = config;
                             reportEntry.Comments = new List<ReportCommentDapper>();
                             reportDictionary.Add(reportEntry.ReportId, reportEntry);                                                    
                         }

                         reportEntry.Comments.Add(comment);

                         return reportEntry;
                     },
                     param: nameFilter == null ? null : new { Name = $"{nameFilter}%" },
                      splitOn: "ConfigId, CommentId"

                     );


                var reportsDto = Mapper.Map<List<ReportDto>>(reports.Distinct());
                return new ReportResponse(reportsDto.Count, Serialize(reportsDto));
            }
        }

        public async Task<ReportResponse> GetLightReportListAsync(string nameFilter = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT r.ReportId, r.Name, r.Status FROM [dbo].[Reports] r";
                query = nameFilter == null ? AddArchivedWhere(query) : AddNameWhere(query);
                query = AddPaging(query, Constants.DEFAULT_SKIP, Constants.DEFAULT_TAKE);

                await connection.OpenAsync();

                var reports = await connection.QueryAsync<ReportListItemDto>(query,
                    param: nameFilter == null ? null : new { Name = $"{nameFilter}%" }
                    );

                var reportsDto = Mapper.Map<List<ReportListItemDto>>(reports);
                var result = new ReportResponse(reportsDto.Count(), Serialize(reportsDto));

                return result;
            }
        }

        string AddJoins(string baseQuery, string aliasToJoinWith = "r")
        {
            return baseQuery += $" LEFT JOIN [dbo].[ReportConfigs] cnf ON {aliasToJoinWith}.ConfigId = cnf.ConfigId LEFT JOIN [dbo].[ReportComments] cm ON {aliasToJoinWith}.ReportId = cm.ReportId ";
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
            return baseQuery += $" WHERE r.[IsArchived] = 0 AND r.[Name] LIKE @Name";
        }
    }
}
