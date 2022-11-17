using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMCoreAPI.Services;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TrophiesAndBadgesController : BaseController
    {
        private readonly ITrophiesAndBadgesService _trophiesAndBadgesService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trophiesAndBadgesService"></param>
        public TrophiesAndBadgesController(ITrophiesAndBadgesService trophiesAndBadgesService, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _trophiesAndBadgesService = trophiesAndBadgesService;
        }

        [HttpGet]
        [Route("TrophiesAndBadgesListing")]
        public async Task<IActionResult> TrophiesAndBadgesListing(string trophyOrBadgeName, string action, string type, string status_Ids, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var purchaseOrderData = _trophiesAndBadgesService.getTrohpiesOrBadges(CompanyId, UserId, trophyOrBadgeName, action, type, status_Ids, sortBy, sortExp, pageSize, pageNumber, langCode,searchCondition);
                return Content(purchaseOrderData, "application/json");
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
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostTrophiesAndBadgesGet")]
        public async Task<IActionResult> PostTrophiesAndBadges(string moduleName, string subModuleCode)
        {
            string msg = _trophiesAndBadgesService.GetcustomFieldgetTrohpiesOrBadges(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the TrophiesAndBadges data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostTrophiesAndBadges")]
        public async Task<IActionResult> PostTrophiesAndBadges([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _trophiesAndBadgesService.SaveTrophiesAndBadges(postString, CompanyId, UserId);
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