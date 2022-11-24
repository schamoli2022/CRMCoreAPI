using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CRMCoreAPI.ServiceInterface;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GoodInwardsController : BaseController
    {
        private readonly IGoodInwardsService _goodInwards;
        public GoodInwardsController(IGoodInwardsService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _goodInwards = services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getPurchaseOrderProductDetails")]
        public async Task<IActionResult> getPurchaseOrderProductDetails(long? sourceId, string moduleName, string subModuleCode, string purchaseOrderIds)
        {
            try
            {
                var expenseData = _goodInwards.getPurchaseOrderProductDetails(CompanyId, UserId, sourceId, moduleName, subModuleCode, purchaseOrderIds);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost]
        [Route("SaveGoodInward")]
        public async Task<IActionResult> PostGoodInward([FromBody] string postString)
        {            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _goodInwards.SaveGoodInwards(postString, CompanyId, UserId);
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
        [Route("GoodInwardListing")]
        public async Task<IActionResult> GoodInwardListing(string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition)
        {
            try
            {
                var expenseData = _goodInwards.GetGoodInward(CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber, totalRecords, searchCondition);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("changestausInwards")]
        public async Task<IActionResult> commonApproveRejectInwards([FromBody] string postString)
        { 
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _goodInwards.commonApproveRejectInwards(postString, CompanyId, UserId);
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
        [Route("GetGoodInwardAssociatedProducts")]
        public async Task<IActionResult> GetGoodInwardAssociatedProducts(string moduleName, string subModuleCode, long id)
        {
            try
            {
                var expenseData = _goodInwards.getGoodInwardAssociatedProducts(CompanyId, UserId, moduleName, subModuleCode, id);
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
        [Route("GetGoodInwardForPurchaseOrder")]
        public async Task<IActionResult> GetGoodInwardForPurchaseOrder(string moduleName, string subModuleCode, long sourceId)
        {
            try
            {
                var expenseData = _goodInwards.getGoodInwardProductsForPO(CompanyId, UserId, moduleName, subModuleCode, sourceId);
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
        [Route("GetGoodInwardForVendor")]
        public async Task<IActionResult> GetGoodInwardForPurchaseVendor(string moduleName, string subModuleCode, long sourceId)
        {
            try
            {
                var expenseData = _goodInwards.getGoodInwardProductsForVendor(CompanyId, UserId, moduleName, subModuleCode, sourceId);
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