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
    public class PriceBookController : BaseController
    {
        private readonly IPriceBookService _PriceBook;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public PriceBookController(IPriceBookService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _PriceBook = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PriceBookListing")]
        public async Task<IActionResult> PriceBookListing(long? id, int? PriceBookOwner, string PriceBookName, int? PriceBookModel, string Description, int? CreatedBy, string CreatedAt, string ModifiedAt, int? StatusID, string LangCode, string sortBy, string sortExp, int pageSize, int pageNumber, string subModuleCode, string requestType, string searchCondition)
        {
            try
            {
                var expenseData = _PriceBook.GetPriceBook(id,PriceBookOwner,  PriceBookName,  PriceBookModel,  Description, CreatedBy,  CreatedAt, ModifiedAt,StatusID, CompanyId, LangCode,UserId,sortBy,sortExp,pageSize,pageNumber, subModuleCode, requestType, searchCondition);
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
        /// This method is used for saving the PriceBook
        /// also validation method is called.
        /// </summary>
        /// <param name="PriceBooklData">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostPriceBookGet")]
        public async Task<IActionResult> PostPriceBook(string moduleName, string subModuleCode)
        {
            string msg = _PriceBook.GetcustomFieldPriceBook(CompanyId, UserId, moduleName, subModuleCode);
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
        [Route("SavePriceBookForMapping")]
        public async Task<IActionResult> SavePriceBookForMapping([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _PriceBook.SavePricebookForMapping(postString, CompanyId, UserId);
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
        [Route("SavePriceBook")]
        public async Task<IActionResult> PostPriceBook([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _PriceBook.SavePriceBook(postString, CompanyId, UserId);
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
        /// Get Price book list for a product
        /// </summary>
        /// <param name="search"></param>
        /// <param name="refId"></param>
        /// <param name="productId"></param>
        /// <param name="CompanyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PriceBookProduct")]
        public async Task<IActionResult> PriceBookListingForProduct(string search, long? refId, long? productId, string sortBy, string sortExp, int pageSize, int pageNumber, string subModuleCode)
        {
            try
            {
                var expenseData = _PriceBook.GetPriceBookListingForProduct(search, refId, productId, CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber, subModuleCode);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
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