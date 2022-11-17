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
    public class ProductController : BaseController
    {
        private readonly IProductService _products;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ProductController(IProductService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _products = services;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// id is used as source id to get product which are exist under the particular category like lead, deal etc.
        /// <param name="id"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="statusId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProductListing")]
        public async Task<IActionResult> ProductListing(string productName, int? id, string subModuleCode, int? statusId, string requestType, string sortBy, string sortExp, int pageSize, int pageNumber, string productUniqueId, string searchCondition)
        {
            try
            {
                var expenseData = _products.GetProducts(productName, CompanyId, UserId, id, subModuleCode, statusId, requestType, sortBy, sortExp, pageSize, pageNumber, productUniqueId,searchCondition);
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
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostProductFieldsGet")]
        public async Task<IActionResult> PostProductFields(string moduleName, string subModuleCode)
        {
            string msg = _products.GetcustomFieldProduct(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the Product data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProduct")]
        public async Task<IActionResult> PostProduct([FromBody] string postString)
        {
            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _products.SaveProduct(postString, CompanyId, UserId);
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
        [Route("SaveProductForMapping")]
        public async Task<IActionResult> SaveMappingProduct([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _products.SaveProductForMapping(postString, CompanyId, UserId);
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
        /// Get Product Type Listing
        /// </summary>
        /// <param name="productTypeName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProductTypeListing")]
        public async Task<IActionResult> ProductTypeListing(string productTypeName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string searchCondition)
        {
            try
            {
                var expenseData = _products.getProductTypeList(productTypeName, CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber,searchCondition);
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
        /// Save Product Type
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProductType")]
        public async Task<IActionResult> SaveProductType([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _products.SaveProductType(postString, CompanyId, UserId);
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

        [HttpGet, Route("GetProductTypeTax")]
        public async Task<IActionResult> GetProductTypeTax(long id)
        {
            try
            {
                var expenseData = _products.GetProductTypeTax(CompanyId, UserId, id);
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
        /// Get tax by product or country or state or zipcode
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        /// <param name="zipcode"></param>
        /// <param name="countryId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetTaxByProduct")]
        public async Task<IActionResult> GetTaxByProduct(string state, string zipcode,string district, long countryId, long projectId)
        {
            try
            {
                var data = _products.GetTaxByProduct(CompanyId, UserId, state, zipcode, district, countryId, projectId);
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