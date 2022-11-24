using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api")]
    [ApiController]
    public class TerritoryController : BaseController
    {
        private readonly ITerritoryService _Territory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public TerritoryController(ITerritoryService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Territory = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("TerritoryListing")]
        public async Task<IActionResult> TerritoryListing(string TerritoryName, string ParentTerritoryName, string UserName, int? TerritoryID, string CreatedAt, string ModifiedAt, string LangCode, string sortBy, string sortExp, int pageSize, int pageNumber, string searchCondition)
        {
            try
            {
                var expenseData = _Territory.GetTerritory(TerritoryName, ParentTerritoryName, UserName, TerritoryID, CompanyId, CreatedAt, ModifiedAt, LangCode, UserId, sortBy, sortExp, pageSize, pageNumber,searchCondition);
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
        /// This method is used for saving the Territory
        /// also validation method is called.
        /// </summary>
        /// <param name="TerritorylData">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostTerritoryGet")]
        public async Task<IActionResult> PostTerritory(string moduleName, string subModuleCode)
        {
            string msg = _Territory.GetcustomFieldTerritory(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the Territory data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTerritory")]
        public async Task<IActionResult> PostTerritory([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Territory.SaveTerritory(postString, CompanyId, UserId);
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
    }
}