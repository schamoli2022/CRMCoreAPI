using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ForecastController : BaseController
    {
        private readonly IForecastService _Forecasts;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public ForecastController(IForecastService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Forecasts = services;
        }

        /// <summary>
        /// Get Quotation listing
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Forecast")]
        public async Task<IActionResult> Forecast(int? forecast_id, string ForecastName, string statusIds, string  forecastCreatedDate, string created_by, string forecastDateTo, byte? isPaged, string sortBy, string sortExp, int? pageSize, int? pageNum)
        {
            try
            {
                var expenseData = _Forecasts.GetForecast(forecast_id, UserId, ForecastName, statusIds, CompanyId, forecastCreatedDate, created_by, forecastDateTo, isPaged, sortBy, sortExp, pageSize, pageNum);
                
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("postForecastFieldsGet")]
        public async Task<IActionResult> postForecastFields(string moduleName, string subModuleCode)
        {

            string msg = _Forecasts.GetcustomFieldForecast(CompanyId, moduleName, subModuleCode, UserId);
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
        [Route("SaveForecast")]
        public async Task<IActionResult> PostForecast([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Forecasts.SaveForecast(postString, CompanyId, UserId);
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