using EFCorePerformance.Cmd.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

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

