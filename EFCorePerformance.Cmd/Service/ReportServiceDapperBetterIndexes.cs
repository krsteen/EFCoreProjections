﻿using System.Threading.Tasks;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceDapperBetterIndexes : ReportServiceDapperBase, IReportService
    {
        public ReportServiceDapperBetterIndexes()
            : base(false)
        {
        }

        public async Task<string> GetAsJsonAsync(int id)
        {

            return await base.GetAsJsonInternalAsync(id);        
        }

        public async Task<string> GetDetailedListAsJsonAsync()
        {
            return await base.GetDetailedListAsJsonInternalAsync();
            
        }

        public async Task<string> GetLightListAsJsonAsync()
        {
            return await base.GetLightListAsJsonInternalAsync();
        } 

    }
}
