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
    public class QuotationController : BaseController
    {
        private readonly IQuotationService _Quotes;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public QuotationController(IQuotationService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Quotes = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="title"></param>
        /// <param name="quotationNumber"></param>
        /// id (sourceId) is used for mapping. By this id we find those quotation which are relate to this source id
        /// <param name="id"></param>
        /// submodule is used with source id to identify submodule of source id.
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="statuses"></param>
        /// <param name="quotationTypeCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QuotesListing")]
        public async Task<IActionResult> QuotesListing(string startDate, string endDate, string title, string quotationNumber, int? id, string subModuleCode, string sortBy, string sortExp, int pageSize, int pageNumber, string statuses, string quotationTypeCode, long? dealId, string searchCondition)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(quotationNumber) || quotationNumber.Length == 0)
                    quotationNumber = null;
                if (string.IsNullOrWhiteSpace(startDate) || startDate.Length == 0)
                    startDate = null;
                if (string.IsNullOrWhiteSpace(endDate) || endDate.Length == 0)
                    endDate = null;
                if (string.IsNullOrWhiteSpace(quotationTypeCode) || quotationTypeCode.Length == 0)
                    quotationTypeCode = null;
                if (string.IsNullOrWhiteSpace(statuses) || statuses.Length == 0)
                    statuses = null;
                var expenseData = _Quotes.GetQuotes(CompanyId, UserId, startDate, endDate, title, quotationNumber, id, subModuleCode, sortBy, sortExp, pageSize, pageNumber, statuses, quotationTypeCode, dealId, searchCondition);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("PostQuoteFieldsGet")]
        public async Task<IActionResult> postQuoteFields(string moduleName, string subModuleCode)
        {
            string msg = _Quotes.GetcustomFieldQuotation(CompanyId, moduleName, subModuleCode, UserId);
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
        /// This method is used for saving the quotation data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrder")]
        public async Task<IActionResult> PostQuotation([FromBody] string postString)
        {
            string msg = _Quotes.ValidateData(postString);
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            msg = _Quotes.SaveQuotation(postString, CompanyId, UserId);
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
        /// Convert order item to other
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConvertOrder")]
        public async Task<IActionResult> ConvertOrder([FromBody] string postString)
        {
            string msg = "";
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            msg = _Quotes.ConvertOrder(postString, CompanyId, UserId);
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
        [Route("QuotationDetails")]
        public async Task<IActionResult> QuotationDetails(long? id, string moduleCode, string subModuleCode)
        {
            try
            {
                var result = _Quotes.QuotationDetails(id, CompanyId, UserId, moduleCode, subModuleCode);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetFeedBack")]
        public async Task<IActionResult> GetFeedBack(long? invoiceId)
        {
            try
            {
                var result = _Quotes.FeedBackDetails(CompanyId, UserId, invoiceId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetUnifiedOrderListing")]
        public async Task<IActionResult> GetUnifiedOrderListing(long contactId,string type_code)
        {
            
            try
            {
                string msg = _Quotes.GetUnifiedOrderListing(contactId, type_code, CompanyId, UserId);
                return Content(msg, "application/json");
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
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost, Route("ConvertQuoteToProject")]
        public async Task<IActionResult> ConvertQuoteToProject([FromBody] string postString)
        {
            string msg = "";
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            msg = _Quotes.QuoteToProject(postString, CompanyId, UserId);
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

        [HttpGet, Route("GetQuoteProductbyDeal")]
        public async Task<IActionResult> GetQuoteProductbyDeal(long companyId, long userId, long? sourceid)
        {

            try
            {
                string msg = _Quotes.GetQuoteProductbyDeal(companyId, userId, sourceid);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetAddressdata")]
        public async Task<IActionResult> GetAddressdata(long? addressid, long? sourceid)
        {

            try
            {
                string msg = _Quotes.GetAddressdata(CompanyId, UserId, addressid, sourceid);
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