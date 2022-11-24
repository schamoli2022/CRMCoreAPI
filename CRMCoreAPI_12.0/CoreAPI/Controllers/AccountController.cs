using System;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _services;

        public AccountController(IAccountService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserData")]
        public IActionResult GetUserData()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }
        [HttpGet]
        [Route("GetValues")]
        public IActionResult GetValues()
        {
            return new JsonResult("Data by CC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AccountListing")]
        public async Task<IActionResult> AccountListing(string ClientName, string EmailId, string UserPhone, int TotalProject, DateTime CreatedAt, string StatusName, int StatusId, string Avatar, string StatusCode, int LeadId, string crmtype, int parentId, int TotalChild)
        {               
            try
            {
                var expenseData = _services.GetAccounts(UserId, CompanyId, ClientName, EmailId, UserPhone, TotalProject, CreatedAt, StatusName, StatusId, Avatar, StatusCode, LeadId, crmtype, parentId, TotalChild);
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
        [Route("GetAccountTypes")]
        public async Task<IActionResult> GetAccountTypes()
        {
            try
            {
                var data = _services.GetAccountsType();
                return Content(data, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveAccountInfo")]
        public async Task<IActionResult> SaveAccountInfo([FromBody] string postString)
        {

            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveAccountInfo(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveAccountAddress")]
        public async Task<IActionResult> SaveAccountAddress([FromBody] string postString)
        {

            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveAccountAddress(postString, CompanyId, UserId);
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
        [Route("AccountAddressListing")]
        public async Task<IActionResult> AccountAddressListing(string moduleCode, int? sourceId, string subModuleCode)
        {
            try
            {
                var expenseData = _services.AccountAddressListing(CompanyId, UserId, moduleCode, sourceId, subModuleCode);
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
