using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceBase : ServiceBase
    {           

        public ReportServiceBase()
            :base()
        {
                  
        }

        protected string Serialize(object whatToSerialize)
        {

            return JsonConvert.SerializeObject(whatToSerialize, Formatting.None,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
        }
    }
}
