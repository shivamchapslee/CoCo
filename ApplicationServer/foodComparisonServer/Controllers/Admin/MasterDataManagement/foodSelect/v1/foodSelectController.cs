using System.Data;
using System.Data.SqlClient;
using foodComparisonServer.Models.Admin.MasterDataManagement.foodSelect.v1;
using foodComparisonServer.Services.SQLGateway.v1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using foodComparisonServer.Models.Common.ResponseModels.v1;
using foodComparisonServer.Services.Common.Helpers.v1;

namespace foodComparisonServer.Controllers.Admin.MasterDataManagement.foodSelect.v1
{
    [Route("api/admin/mdm/food/v1")]
    [ApiController]
    public class foodSelectController : ControllerBase
    {
        #region GLOBAL VARIABLES
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private IConfiguration Configurations;
        private SQLGateway ObjSqlGateway;
        private Helper ObjHelper;
        private ApplicationResponseModel ObjResponse;
        private ApiResponse ObjApiReponse;
        #endregion

        #region CONSTRUCTOR
        public foodSelectController(IHostingEnvironment _HostingEnvironment, IConfiguration _Configuration, IHttpContextAccessor _HttpContextAccessor)
        {
            HostingEnvironment = _HostingEnvironment;
            Configurations = _Configuration;
            HttpContextAccessor = _HttpContextAccessor;
            ObjSqlGateway = new SQLGateway(HostingEnvironment, Configurations);
            ObjHelper = new Helper(HostingEnvironment, Configurations);
            ObjResponse = new ApplicationResponseModel();
            ObjApiReponse = new ApiResponse();
        }
        #endregion

        #region SELECT FOOD
        [HttpPost]
        [Route("get-food")]
        public IActionResult GetFood(foodSelectModel InputModel)
        {           
            SqlParameter[] parameters =
            {
                new SqlParameter("@number", InputModel.number)
            };
            DataTable dtResponse = ObjSqlGateway.bindDataTableParam("foodSelect", parameters);

            if (ObjHelper.CheckDBNullResponse(dtResponse))
            {
                ObjResponse.Response = 1;
                ObjResponse.Data = ObjHelper.ConvertTableToDictionary(dtResponse);
            }
            else
            {
                ObjResponse = ObjApiReponse.ResolveDbNullResponse();
            }
            return Ok(ObjResponse);

            //if (dtResponse != null)
            //{
            //    if (dtResponse.Rows.Count > 0)
            //    {
            //        if (dtResponse.Rows[0]["Response"].ToString() == "1")
            //        {
            //            return Ok(dtResponse.Rows[0].ToString());
            //        }
            //        else
            //        {
            //            return Ok("no table was returned.");
            //        }
            //    }
            //}
            //return Ok();
        }
        #endregion
    }
}