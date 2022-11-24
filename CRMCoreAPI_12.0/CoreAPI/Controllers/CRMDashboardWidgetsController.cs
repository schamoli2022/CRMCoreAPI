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
    [Route("api/CRMDashboard")]
    [ApiController]
    public class CRMDashboardWidgetsController : BaseController
    {
        private readonly IWidgetsService _widget;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="widget"></param>  
        public CRMDashboardWidgetsController(IWidgetsService widget, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _widget = widget;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(long userTypeID, DateTime? fromDate, DateTime? toDate, string langCode,string reportType)
        {
            try
            {
               
                var list = _widget.getWidgetsList(CompanyId, UserId, userTypeID, fromDate, toDate, langCode, reportType);
                return Content(list, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        //[HttpGet]
        //[Route("api/GetWidgets")]
        //public async Task<IActionResult> GetTickets(long userId, long companyId, string moduleName)
        //{
        //    try
        //    {
        //        var list = _widget.GetTickets(userId, companyId, moduleName);
        //        return Content(list, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return NotFound();
        //    }
        //}

    }
}