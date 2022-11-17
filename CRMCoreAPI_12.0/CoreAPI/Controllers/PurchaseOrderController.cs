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
    public class PurchaseOrderController : BaseController
    {
        private readonly  IPurchaseOrderService _puchaseServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="puchaseServices"></param>
        public PurchaseOrderController(IPurchaseOrderService puchaseServices, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _puchaseServices = puchaseServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="accountName"></param>
        /// <param name="companyName"></param>
        /// <param name="leadOwners"></param>
        /// <param name="dealName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PurchaseOrderListing")]
        public async Task<IActionResult> PurchaseOrderListing(string statusIDs, string accountName, string companyName, string leadOwners, string dealName, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var purchaseOrderData = _puchaseServices.GetPurchaseOrders(CompanyId, UserId, statusIDs, accountName, companyName, leadOwners, dealName, dateFrom, dateTo, sortBy, sortExp, pageSize, pageNumber, langCode,searchCondition);
                return Content(purchaseOrderData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// This method is used for saving the PurchaseOrders
        /// also validation method is called.
        /// </summary>
        /// <param name="PurchaseOrderslData">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostPurchaseOrdersGet")]
        public async Task<IActionResult> PostPurchaseOrders(string moduleName, string subModuleCode)
        {
            string msg = _puchaseServices.GetcustomFieldPurchaseOrders(CompanyId, UserId, moduleName, subModuleCode);
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
        //[HttpPost]
        //[Route("SavePurchaseOrder")]
        //public async Task<IActionResult> PostPurchaseOrder([FromBody] string postString)
        //{
        //    string msg=_puchaseServices.ValidateData(postString);
           
        //    if (postString == null)
        //    {
        //        return Content("Failure", "application/json");
        //    }
        //    msg = _puchaseServices.SavePurchaseOrder(postString);
        //    try
        //    {
        //        return Content(msg, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return NotFound();
        //    }
        //}


        [HttpPost]
        [Route("changestausPurchaseOrder")]
        public async Task<IActionResult> approveRejectPurchaseOrder([FromBody] string postString) 
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _puchaseServices.approveRejectPurchaseOrder(postString, CompanyId, UserId); 
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