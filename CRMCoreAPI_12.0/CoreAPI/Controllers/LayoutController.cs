using System;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api/FormLayout")]
    [ApiController]
    public class LayoutController : BaseController
    {
        private readonly ILayoutService _services;
       

        public LayoutController(ILayoutService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _services = services;
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
        public async Task<IActionResult> Get(long? id, string moduleName, string subModuleCode, string otherData,string langCode="")
        {
            string msg = _services.GetLayout(id, CompanyId, UserId, moduleName, subModuleCode, otherData,langCode);
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
        public async Task<IActionResult> Post([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveFieldData(postString, CompanyId, UserId);
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
        /// <summary>
        /// Manage listing layout and get available and selected fields
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListView")]
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

        [HttpPost]
        [Route("ListView")]
        public async Task<IActionResult> PostListLayout([FromBody] string postString)
        {

            if (string.IsNullOrWhiteSpace(postString))
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveListLayout(postString, CompanyId, UserId);
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
        [Route("Manage")]
        public async Task<IActionResult> PostPageLayout([FromBody] string postString)
        {

            if (string.IsNullOrWhiteSpace(postString))
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SavePageLayout(postString, CompanyId, UserId);
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
        /// This method is used to view the custom fields form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("View")]
        public async Task<IActionResult> View(long? id, string moduleName, string subModuleCode)
        {
            string msg = _services.View(id, CompanyId, UserId, moduleName, subModuleCode);
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
        /// Get Sub module list on basis of module id for manage the layout
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet,Route("GetSubModuleList")]
        public async Task<IActionResult> GetSubModuleList(long? id, string sortBy, string sortExp, int? pageSize, int? pageNum)
        {
            string msg = _services.GetSubModuleList(id, CompanyId, UserId, sortBy, sortExp, pageSize, pageNum);
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
        /// Get module list  for manage the layout
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetModuleList")]
        public async Task<IActionResult> GetModuleList()
        {
            string msg = _services.GetModuleList(CompanyId, UserId);
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
