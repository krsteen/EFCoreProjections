using Dapper;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace EFCoreProjections.Cmd.Service
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
                //await connection.ExecuteAsync("CHECKPOINT");
                //await connection.ExecuteAsync("DBCC DROPCLEANBUFFERS");
                await connection.ExecuteAsync("DBCC FREEPROCCACHE");              
            }
        }
    }
}

