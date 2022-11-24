using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Service;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _Invoice;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public InvoiceController(IInvoiceService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Invoice = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="OrderSubject"></param>
        /// <param name="DealName"></param>
        /// <param name="SalesOwner"></param>
        /// <param name="LeadOwner"></param>
        /// <param name="ContactName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="AccountName"></param>
        /// <param name="leadName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// id (or sourceId) used to get sales order listing according to the mapping with respect to sub module
        /// <param name="id"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="isPaged"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("InvoiceListing")]
        public async Task<IActionResult> InvoiceListing(int? Status,string OrderSubject,string DealName, string SalesOwner, string LeadOwner, string ContactName, string dateFrom, string dateTo, string AccountName, string leadName, int? statusID, int? id,string subModuleCode, string sortBy, string sortExp, byte? isPaged, int? pageSize, int? pageNumber, string searchCondition)
        {
            try
            {
                var expenseData = _Invoice.GetInvoice(Status, OrderSubject, DealName, SalesOwner, LeadOwner, ContactName, dateFrom, dateTo, AccountName, leadName,statusID, CompanyId, UserId, id, subModuleCode, sortBy, sortExp, isPaged, pageSize, pageNumber, searchCondition);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        //[HttpGet]
        //[Route("postInvoiceFieldsGet")]
        //public async Task<IActionResult> ManageLeadDetailsNotes(long companyId, string moduleName, string subModuleCode, long userId)
        //{

        //    string msg = _Invoice.GetcustomFieldInvoice(companyId, moduleName, subModuleCode, userId);
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
        [HttpGet]
        [Route("PostInvoiceGet")]
        public async Task<IActionResult> PostInvoice(string moduleName, string subModuleCode)
        {
            string msg = _Invoice.GetcustomFieldInvoice(CompanyId, moduleName, subModuleCode, UserId);
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
        //[HttpPost]
        //[Route("SaveInvoice")]
        //public async Task<IActionResult> PostInvoice([FromBody] string postString)
        //{
        //    if (postString == null)
        //    {
        //        return Content("Failure", "application/json");
        //    }
        //    string msg = _Invoice.SaveInvoice(postString);
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
    }
}