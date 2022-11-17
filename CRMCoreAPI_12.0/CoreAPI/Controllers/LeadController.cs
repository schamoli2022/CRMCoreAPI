using System;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class LeadController : BaseController
    {
        private readonly ILeadService _services;
        private IConfiguration configuration;
        private readonly ILogger<LeadController> _logger;


        public LeadController(ILeadService services, IConfiguration config, IHttpContextAccessor httpContextAccessor, ILogger<LeadController> logger) : base(httpContextAccessor)
        {
            _services = services;
            configuration = config;
            _logger = logger;
        }

        /// <summary>
        /// This method is used for calling the method which fetches the list of leads 
        /// on the basis of parameters passed.
        /// </summary>
        /// <param name="clientName">Client Name parameter is passed for 
        /// fetching the records for a particular Client and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="statusIds">Status Ids parameter is passed for 
        /// fetching the records for the status selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="clientIds">Client Ids parameter is passed for 
        /// fetching the records for the clients selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="companyName">Company Name parameter is passed for 
        /// fetching the records for the company name passed and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="channelIds">channel Ids parameter is passed for 
        /// fetching the records for the status selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadOwnerIds">Lead Owner Ids parameter is passed for 
        /// fetching the records for the lead owners selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadDateFrom">Lead Date From parameter is passed for 
        /// fetching the records for the date from selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadDateTo">Lead Date to parameter is passed for 
        /// fetching the records for the date to selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="isPaged">is Paged parameter is passed for 
        /// fetching the records for a particular company</param>
        /// <param name="id">id is used as sourceId to get leads accordingly which are mapped to another modules 
        /// with respect to submoduleCode variable</param>
        /// <param name="subModuleCode">submoduleCode pointed to the particular module 
        /// like lead, deal, purchase order etc and used with id or sourceId</param>
        /// <param name="sortBy">Sort By parameter is passed for 
        /// sorting the records fetched by the value passed in the parameter.</param>
        /// <param name="sortExp">Sort By parameter is passed for 
        /// sorting the records fetched in the ascending or descending order 
        /// depending ton the parameter passed.</param>
        /// <param name="pageSize">Page Size parameter is passed for 
        /// fetching the number of records on a particular page. By default the
        /// records on a page are displayed on the basis of configuration setting</param>
        /// <param name="pageNum">Page Number parameter is passed for 
        /// mentioning the page number if it is 1,2 or so on./param>
        /// <param name="crmtype">CRM Type parameter is passed for 
        /// fetching the records of Lead(l), opportunity(o), propect(p)</param>
        /// <returns>returns Json data</returns>
        [HttpGet]
        [Route("LeadListing")]
       // [TypeFilter(typeof(CustomAuthorizeFilter),Arguments=new object[] { "CRM","Index"})]
        public async Task<IActionResult> LeadListing(string clientName, string statusIds, string clientIds, string companyName, string channelIds, string leadOwnerIds, string leadDateFrom, string leadDateTo, byte isPaged, int? id, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNum, char crmtype,string searchCondition, bool isExport=false)
        {               
            try
            {
               //string clientid =CurrentUser.client_id;
              // companyId = CurrentUser.CompanyId;
               // viewerId = CurrentUser.UserId;
             // string usertype= CurrentUser.usertype;
                if (string.IsNullOrWhiteSpace(clientIds) || clientIds.Length == 0)
                    clientIds = null;
                if (string.IsNullOrWhiteSpace(statusIds) || statusIds.Length == 0)
                    statusIds = null;
                if (string.IsNullOrWhiteSpace(channelIds))
                    channelIds = null;                         
                if (leadOwnerIds == "")
                    leadOwnerIds = null;

                if (string.IsNullOrWhiteSpace(sortBy) || sortBy.Length == 0)                   
                    sortBy = null;

                if (string.IsNullOrWhiteSpace(sortBy) || sortBy.Length == 0)
                    sortExp = null;

                if (clientName == "")
                    clientName = null;

                if (companyName == "")
                    companyName = null;
                //var expenseData = _services.GetLeads(companyId, viewerId, clientName, statusIds, clientIds, companyName, channelIds, leadOwnerIds, null, null,
                //isPaged, id, subModuleCode, sortBy, sortExp, pageSize, pageNum,crmtype, searchCondition, isExport);
                _logger.LogInformation("data apptempt step 1" + DateTime.Now.ToString());
                var expenseData = _services.GetLeads(CompanyId, UserId, clientName, statusIds, clientIds, companyName, channelIds, leadOwnerIds, null, null,
               isPaged, id, subModuleCode, sortBy, sortExp, pageSize, pageNum, crmtype, searchCondition, isExport);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData); 
                _logger.LogInformation("data apptempt step 2" + DateTime.Now.ToString());
                //   return Content("{ \"value1\", \"value2\" }", "application/json");
                return Content(expenseData, "application/json");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content(ex.Message, "application/json");
               // return NotFound();
            }
        }

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        [HttpGet]
        [Route("PostLeadGet")]
        public async Task<IActionResult> PostLead(string moduleName,string subModuleCode)
        {
            string msg = _services.GetcustomFieldLead(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the lead
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveLead")]        
        public async Task<IActionResult> PostLead([FromBody] string postString)
        {
            
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveLead(postString, CompanyId, UserId);
            try
            {             
                return Content(msg,"application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveVdeskLead")]
        public async Task<IActionResult> PostLeadFromVdesk([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveVdeskLead(postString);
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
        /// Manage listing layout and get available and selected fields
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ManageListLayout")]
        public async Task<IActionResult> ListLayoutFields(string moduleName, string subModuleCode)
        {
            string msg = _services.GetListLayoutFields(CompanyId, UserId, moduleName, subModuleCode);
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
        //[Route("PostListLayout")]
        //public async Task<IActionResult> PostListLayout([FromBody] string postString)
        //{

        //    if (string.IsNullOrWhiteSpace(postString))
        //    {
        //        return Content("Failure", "application/json");
        //    }
        //    string msg = _services.SaveListLayout(postString);
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

        //[HttpPost]
        //[Route("PostPageLayout")]
        //public async Task<IActionResult> PostPageLayout([FromBody] string postString)
        //{

        //    if (string.IsNullOrWhiteSpace(postString))
        //    {
        //        return Content("Failure", "application/json");
        //    }
        //    string msg = _services.SavePageLayout(postString);
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
        [Route("ConvertLead")]
        public async Task<IActionResult> ConvertLead([FromBody] string postString)
        {

            if (string.IsNullOrWhiteSpace(postString))
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.convertLeadToAccount(postString, CompanyId, UserId);
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
        ///  close lead
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CloseLead")]
        public async Task<IActionResult> CloseLead([FromBody] string postString)
        {
            string msg = _services.CloseLead(postString, CompanyId, UserId);
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
        [Route("ChangeOwner")]
        public async Task<IActionResult> ChangeOwner([FromBody] string postString) 
        {
            string msg = _services.ChangeOwner(postString, CompanyId, UserId);
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
        [Route("ExportLead")]
        public async Task<IActionResult> GetExport(string moduleName, string subModuleCode)
        {
            string msg = _services.GetExportDetail(CompanyId, moduleName, subModuleCode);
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
        [Route("Importlead")]
        public async Task<IActionResult> GetLeadImport(string batchid)
        {
            try
            {
                var expenseData = _services.GetLeadImport( batchid);
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
        [Route("SaveImportlead")]
        public async Task<IActionResult> PostImportlead([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveLeadImport(postString, CompanyId, UserId);
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

        [HttpGet, Route("GetUnifiedLeadDetail")]
        public async Task<IActionResult> GetUnifiedLead(long contactId)
        {            
            try
            {
                string msg = _services.GetUnifiedLead(contactId, CompanyId, UserId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveUnifiedLead")]
        public async Task<IActionResult> SaveUnifiedLead([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveUnifiedLead(postString, CompanyId, UserId);
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
        [Route("ReopenLead")]
        public async Task<IActionResult> ReopenLead([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.ReopenLead(postString, CompanyId , UserId);
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
        [HttpGet, Route("GetAllLeadOwnerPermission")]
        public async Task<IActionResult> GetAllLeadOwnerPermission(string subModuleCode, long? leadOwner, long? leadId)
        {
            try
            {
                string msg = _services.GetAllLeadOwnerPermission(CompanyId, UserId, subModuleCode, leadOwner, leadId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost]
        [Route("saveAdditionalOwnerPermission")]
        public async Task<IActionResult> saveAdditionalOwnerPermission([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.saveAdditionalOwnerPermission(postString, CompanyId, UserId);
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
        
        [HttpGet, Route("additionalLeadOwnerListing")]
        public async Task<IActionResult> additionalLeadOwnerListing(long id, string subModuleCode)
        {
            try
            {
                string msg = _services.additionalLeadOwnerListing(id, CompanyId, UserId, subModuleCode);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("DeleteAttachment")]
        public async Task<IActionResult> DeleteAttachment(long id, long sourceid)
        {
            try
            {
                string msg = _services.DeleteAttachment(id, sourceid);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost, Route("DeleteAccountBeforeDeal")]
        public async Task<IActionResult> DeleteAccountBeforeDeal([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.DeleteAccountBeforeDeal(postString, CompanyId, UserId);
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
        [Route("saveAdditionalDealOwnerPermission")]
        public async Task<IActionResult> saveAdditionalDealOwnerPermission([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.saveAdditionalDealOwnerPermission(postString, CompanyId, UserId);
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
