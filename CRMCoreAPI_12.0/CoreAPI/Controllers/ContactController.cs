using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ContactController : BaseController
    {
        private readonly IContactService _Contact;


        public ContactController(IContactService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Contact = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="First_Name"></param>
        /// <param name="Last_Name"></param>
        /// <param name="job_title"></param>
        /// <param name="email_id"></param>
        /// <param name="business_phone"></param>
        /// <param name="mobile_phone"></param>
        /// <param name="fax"></param>
        /// <param name="preferred_contact"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// id is used as a source id to get contact id for particular module like is mapped with lead, deal etc.
        /// <param name="id"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ContactListing")]
        public async Task<IActionResult> ContactListing(string First_Name, string Last_Name, string job_title, string email_id, string business_phone, string mobile_phone, string fax, string preferred_contact, int? id, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, int? statusId, string requestType, string searchCondition,string AccountType,int? clientId)
        {
            try
            {
                var expenseData = _Contact.GetContacts(First_Name,  Last_Name,  job_title,  email_id,  business_phone,  mobile_phone,  fax,  preferred_contact, CompanyId,  UserId, id, subModuleCode,  sortBy,  sortExp,  pageSize,  pageNumber, statusId, requestType, searchCondition,AccountType, clientId);
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
        [Route("ContactGroupListing")]
        public async Task<IActionResult> ContactGroupListing(int? id, string subModuleCode)
        {
            try
            {
                var expenseData = _Contact.GetGroupContacts(id, CompanyId, subModuleCode, UserId);
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
        [Route("PostContactGet")]
        public async Task<IActionResult> PostContact(string moduleName, string subModuleCode)
        {
            string msg = _Contact.GetcustomFieldContact(CompanyId, moduleName, subModuleCode, UserId);
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
        [Route("SaveContactWithMapping")]
        public async Task<IActionResult> saveContactWithMapping([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContactWithMapping(postString, CompanyId, UserId);
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
        [Route("SaveExistingContactWithMapping")]
        public async Task<IActionResult> saveExistingContactWithMapping([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveExistingContactWithMapping(postString, CompanyId, UserId);
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
        [Route("SaveContact")]
        public async Task<IActionResult> saveContact([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContact(postString, CompanyId, UserId);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSingleContactDetails")]
        public async Task<IActionResult> getSingleContact(long id)
        {
            if (id <= 0)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.getSingleContact(id, CompanyId, UserId);
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
        [Route("SaveImportContact")]
        public async Task<IActionResult> PostImportContact([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContactImport(postString, CompanyId, UserId,"");
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
        [HttpGet, Route("GetUnifiedContactDetail")]
        public async Task<IActionResult> getUnifiedContact(string mobileNum)
        {
            if (String.IsNullOrEmpty(mobileNum))
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.getUnifiedContact(mobileNum, CompanyId, UserId);
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
        [Route("SaveUnifiedContact")]
        public async Task<IActionResult> SaveUnifiedContact([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveUnifiedContact(postString, CompanyId, UserId);
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
        [Route("SaveClientInfo")]
        public async Task<IActionResult>SaveClientInfo([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveClientInfo(postString, CompanyId, UserId);
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

        [HttpPost, Route("DeleteClient")]
        public async Task<IActionResult> DeleteClientContact([FromBody] string postString)
        {
            string msg = _Contact.DeleteClientContact(postString, CompanyId);
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
        [HttpGet, Route("UserRoleList")]

        public async Task<IActionResult> UserRoleList(int userType)
        {
            try
            {
                var expenseData = _Contact.UserRole(CompanyId,UserId, userType);
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
        [Route("SaveImportAccounts")]
        public async Task<IActionResult> PostImportAccounts([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContactImport(postString, CompanyId, UserId, "CRM_ACCOUNTS");
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
        [Route("SaveContactRoleTitle")]
        public async Task<IActionResult> SaveContactRoleTitle([FromBody] string postString)
        {

            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContactRoleTitle(postString, CompanyId, UserId);
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
        [Route("SaveContactAsPrimary")]
        public async Task<IActionResult> SaveContactAsPrimary([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Contact.SaveContactAsPrimary(postString, CompanyId, UserId);
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