using foodComparisonServer.Models.Common.ResponseModels.v1;
using System;
using System.Data;

namespace foodComparisonServer.Services.Common.Helpers.v1
{
    public class ApiResponse
    {
        ApplicationResponseModel ObjResponse = new ApplicationResponseModel();
        public ApplicationResponseModel ResolveDbException(DataRow drResponseRow)
        {
            try
            {
                ObjResponse.Response = Convert.ToInt32(drResponseRow["Response"].ToString());
                ObjResponse.Code = drResponseRow["Response_Code"] != null ? drResponseRow["Response_Code"].ToString() : null;
                ObjResponse.Sys_Message = drResponseRow["Sys_Message"] != null ? drResponseRow["Sys_Message"].ToString() : null;
            }
            catch (Exception)
            {
                ObjResponse.Response = -1;
                ObjResponse.Code = "REQUEST_FAILED";
                //ObjResponse.Sys_Message = Resources.Common.DbResponseNullError;
            }
            return ObjResponse;           
        }
        public ApplicationResponseModel ResolveDbNullResponse()
        {
            ObjResponse.Response = -1;
            ObjResponse.Code = "REQUEST_FAILED";
            //ObjResponse.Sys_Message = Resources.Common.DbResponseNullError;
            return ObjResponse;
        }
    }
}
