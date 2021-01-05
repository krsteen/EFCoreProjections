using Dapper;
using EFCorePerformance.Cmd.DapperModel;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportsServiceBetterIndexesAndDapper : ServiceBase, IReportsService
    {
        public ReportsServiceBetterIndexesAndDapper()
            : base()
        {
        }

        public async Task<string> GetAsJsonAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT * FROM [dbo].[ReportsWithBetterIndex] WHERE [Id] = @Id";

                await connection.OpenAsync();

                var report = await connection.QuerySingleAsync<ReportDapper>(query, new { Id = id });

                return Serialize(report);
            }           
        }

        public async Task<string> GetListAsJsonAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = $"SELECT * FROM [dbo].[ReportsWithBetterIndex] ORDER BY [Id] OFFSET 100 ROWS FETCH NEXT 20 ROWS ONLY";

                await connection.OpenAsync();

                var reports = await connection.QueryAsync<ReportDapper>(query);

                return Serialize(reports);
            }
        }
    }
}
