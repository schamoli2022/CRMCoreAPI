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
    public class VendorController : BaseController
    {
        private readonly IVendorService _vendorService;
        public VendorController(IVendorService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _vendorService = services;
        }

        [HttpGet]
        [Route("VendorListing")]
        public async Task<IActionResult> VendorListing(string statusIDs, string createdBy, string vendorName, string phoneNumber, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var expenseData = _vendorService.GetVendors(CompanyId, UserId, statusIDs, createdBy, vendorName, phoneNumber, dateFrom, dateTo, sortBy, sortExp, pageSize, pageNumber, langCode,searchCondition);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// This method is used for saving the Vendor
        /// also validation method is called.
        /// </summary>
        /// <param name="VendorlData">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostVendorGet")]
        public async Task<IActionResult> PostVendor(string moduleName, string subModuleCode)
        {
            string msg = _vendorService.GetcustomFieldVendor(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the Vendor data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveVendor")]
        public async Task<IActionResult> PostVendor([FromBody] string postString)
        {
            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _vendorService.SaveVendor(postString, CompanyId, UserId);
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