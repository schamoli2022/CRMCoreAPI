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
    [Route("api")]
    [ApiController]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _ActivityService;

        public ActivityController(IActivityService ActivityService, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _ActivityService = ActivityService;
        }


        [HttpGet]
        [Route("ActivityListing")]
        public async Task<IActionResult> ActivityListing(string moduleCode, int? id, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode)
        {
            try
            {
                var expenseData = _ActivityService.getActivities(CompanyId, UserId, moduleCode, id, subModuleCode, sortBy, sortExp, pageSize, pageNumber, langCode);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] string postString) 
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _ActivityService.Addtask(postString, CompanyId, UserId);
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
        [Route("SaveScheduleCall")]
        public async Task<IActionResult> SaveScheduleCall([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _ActivityService.SaveScheduleCall(postString, CompanyId, UserId);
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
        [Route("updateActivity")]
        public async Task<IActionResult> updateActivity(long? id)
        {
            try
            {
                //UploadFile();
                var expenseData = _ActivityService.updateActivity(id);
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