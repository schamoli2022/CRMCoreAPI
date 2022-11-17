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


    [Route("api")]
    [ApiController]
    public class DealController : BaseController
    {
        private readonly IDealService _Deals;

        public DealController(IDealService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Deals = services;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Dealname"></param>
        /// <param name="statusID"></param>
        /// <param name="createdBy"></param>
        /// <param name="createdAt"></param>
        /// <param name="closingDate"></param>
        /// <param name="companyID"></param>
        /// <param name="id">id is used as sourceId to get deals accordingly which are mapped to another modules 
        /// with respect to submoduleCode variable</param>
        /// <param name="subModuleCode">submoduleCode pointed to the particular module 
        /// like lead, deal, purchase order etc </param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pagesize"></param>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DealListing")]
        public async Task<IActionResult> DealListing(string Dealname, int? statusID, int? createdBy, string createdAt, string closingDate, int? id, string subModuleCode, string sortBy, string sortExp, int pagesize, int pagenumber,string searchCondition)
        {
            try
            {
                var expenseData = _Deals.GetDeal(Dealname, statusID, createdBy, createdAt, closingDate, CompanyId,UserId,id, subModuleCode, sortBy, sortExp, pagesize, pagenumber, searchCondition);
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
        /// This method is used for saving the Deal
        /// also validation method is called.
        /// </summary>
        /// <param name="DeallData">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostDealGet")]
        public async Task<IActionResult> PostDeal(string moduleName, string subModuleCode)
        {
            string msg = _Deals.GetcustomFieldDeal(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the Deal data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostDeal")]
        public async Task<IActionResult> PostDeal([FromBody] string postString)
        {
            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Deals.SaveDeal(postString, CompanyId, UserId);
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

        [HttpGet, Route("GetUnifiedDealDetail")]
        public async Task<IActionResult> GetUnifiedDeal(long contactId)
        {
            if (contactId <= 0)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Deals.GetUnifiedDeal(contactId, CompanyId, UserId);
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
        [Route("RevertAction")]
        public async Task<IActionResult> RevertAction([FromBody] string postString)
        {
            string msg = _Deals.RevertAction(postString, CompanyId, UserId);
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
        [Route("PrimaryContactForDeal")]
        public async Task<IActionResult> PrimaryContactForDeal(long? sourceid, string submodulecode)
        {
            try
            {
                var data = _Deals.PrimaryContactForDeal(CompanyId, sourceid, submodulecode);
                return Content(data, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

    }
}
