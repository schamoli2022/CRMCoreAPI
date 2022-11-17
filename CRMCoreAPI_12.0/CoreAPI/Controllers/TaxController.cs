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
    public class TaxController : BaseController
    {
        private readonly ITaxService _taxService;
        private IConfiguration configuration;
        public TaxController(ITaxService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _taxService = services;
            configuration = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTaxCode")]
        public async Task<IActionResult> SaveTaxCode([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _taxService.SaveTaxCode(postString, CompanyId, UserId);
            try
            {
                return Ok(msg);
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
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveTaxByCode")]
        public async Task<IActionResult> SaveTax([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _taxService.SaveTax(postString, CompanyId, UserId);
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
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetTaxByCodeId")]
        public async Task<IActionResult> GetTaxByCodeId(long? id)
        {
            try
            {
                var TaxDetails = await _taxService.GetTaxByCodeId(CompanyId, UserId, id);
                return Content(TaxDetails, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetTaxByTaxCode")]
        public async Task<IActionResult> GetTaxByCode(string taxCode)
        {
            try
            {
                var TaxDetails = await _taxService.GetTaxByCode(CompanyId, UserId, taxCode);
                return Content(TaxDetails, "application/json");
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
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllTaxCode")]
        public async Task<IActionResult> GetAllTaxCode(long? id)
        {
            try
            {
                var TaxDetails = await _taxService.GetAllTaxCode(CompanyId, UserId, id);
                return Content(TaxDetails, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}