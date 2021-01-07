using Dapper;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ClearDbCacheService : ServiceBase
    {
        public ClearDbCacheService()
            : base()
        {
        }

      

        public async Task ClearCache()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync("DBCC DROPCLEANBUFFERS");
                await connection.ExecuteAsync("DBCC FREEPROCCACHE");              
            }
        }
    }
}

