using EFCorePerformance.Cmd.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFCorePerformance.Cmd.Service
{
    public class ReportServiceBase : ServiceBase
    {
        protected bool ConvertToDto;      

        public ReportServiceBase(bool convertToDto)
            :base()
        {
            ConvertToDto = convertToDto;          
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
