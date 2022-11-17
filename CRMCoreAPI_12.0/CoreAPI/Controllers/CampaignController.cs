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
    public class CampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        public CampaignController(ICampaignService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _campaignService = services;
        }

        /// <summary>
        /// Providing listing for campaigns
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="campaignName"></param>
        /// <param name="campaignOwnerIds"></param>
        /// <param name="campaignType"></param>
        /// date from column is used as proposed date from 
        /// <param name="campaignDateFrom"></param>
        /// date to column is used as propoed date to
        /// <param name="campaignDateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CampaignListing")]
        public async Task<IActionResult> CampaignListing(string statusIDs, string campaignName, string campaignOwnerIds, string campaignType, string campaignDateFrom, string campaignDateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var expenseData = _campaignService.GetCampaigns(CompanyId, UserId, statusIDs, campaignName, campaignOwnerIds, campaignType, campaignDateFrom, campaignDateTo, sortBy, sortExp, pageSize, pageNumber, langCode, searchCondition);
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
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostCampaignFieldsGet")]
        public async Task<IActionResult> PostCampaignGet(string moduleName, string subModuleCode)
        {
            string msg = _campaignService.GetCustomFieldForCampaign(CompanyId, UserId, moduleName, subModuleCode);
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
        [Route("SaveCampaign")]
        public async Task<IActionResult> PostCampaign([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _campaignService.SaveCampaign(postString, CompanyId, UserId);
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
        [Route("ConvertToLeadCampaign")]
        public async Task<IActionResult> CampaignConvertToLead([FromBody] string postString)
        {
            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _campaignService.SaveConvertToLeadCampaign(postString, CompanyId, UserId);
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
        [Route("SaveContactGroup")]
        public async Task<IActionResult> SaveContactGroup([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _campaignService.SaveGroupContactCampaign(postString, CompanyId, UserId);
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
        [Route("SaveEmailScheduler")]
        public async Task<IActionResult> SaveEmailScheduler([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _campaignService.SaveEmailScheduler(postString, CompanyId, UserId);
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
        [Route("AddGroupEmailTemplate")]
        public async Task<IActionResult> AddGroupEmailTemplate([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _campaignService.AddGroupEmailTemplate(postString, CompanyId, UserId);
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