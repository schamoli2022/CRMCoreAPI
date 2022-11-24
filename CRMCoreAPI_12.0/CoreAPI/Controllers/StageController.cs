using System;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class StageController : BaseController
    {
    
            private readonly IStageService _Stages;
            public StageController(IStageService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
                _Stages = services;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="companyId"></param>
            /// <returns></returns>
            [HttpGet]
            [Route("StageListing")]
            public async Task<IActionResult> StageListing(string stagename, string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition)
            {
                try
                {

                //if (string.IsNullOrWhiteSpace(stagename) || stagename.Length == 0)
                //    sortBy = null;

                var expenseData = _Stages.GetStages(CompanyId, UserId, stagename, sortBy, sortExp, pageSize, pageNumber, totalRecords,searchCondition);
                    //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                    return Content(expenseData, "application/json");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return NotFound();
                }
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostStageFieldsGet")]
        public async Task<IActionResult> PostStageFields(string moduleName, string subModuleCode)
        {
            string msg = _Stages.GetcustomFieldStage(CompanyId, UserId, moduleName, subModuleCode);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// This method is used for saving the Purchase order data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveStage")]
        public async Task<IActionResult> PostStage([FromBody] string postString)
        { 
           
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
              string msg = _Stages.SaveStage(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }


        [HttpPost]
        [Route("udpateDealStage")]
        public async Task<IActionResult> udpateDealStage([FromBody] string postString)
        {            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Stages.updateDealStage(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("getStageMappListing")]
        public async Task<IActionResult> StageMapListing(int id, string moduleName, string subModuleCode)
        {
            try
            {   
                var expenseData = _Stages.GetMappingStages(id, CompanyId, UserId, moduleName, subModuleCode);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}