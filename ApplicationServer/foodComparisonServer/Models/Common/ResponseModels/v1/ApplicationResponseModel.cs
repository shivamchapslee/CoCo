using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foodComparisonServer.Models.Common.ResponseModels.v1
{
    public class ApplicationResponseModel
    {
        public int Response { get; set; }
        public object Data { get; set; }
        public string Code { get; set; }
        public string Sys_Message { get; set; }
    }
}
