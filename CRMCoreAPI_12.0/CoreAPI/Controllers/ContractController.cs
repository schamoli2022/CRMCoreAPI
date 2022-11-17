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
    public class ContractController : BaseController
    {
        private readonly IContractService _contracts;
        public ContractController(IContractService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _contracts = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ContractListing")]
        public async Task<IActionResult> ContractListing(string contract_Name, string sortBy, string sortExp, int pageSize, int pageNumber, long userId, string clientIds, string resellerIds, string ProductIds, string contractIds, string companyName, string contractName, string billToClientName,string searchCondition)
        {
            try
            {
                var expenseData = _contracts.GetContracts(CompanyId, contract_Name, UserId, sortBy, sortExp, pageSize, pageNumber, userId, clientIds, resellerIds, ProductIds, contractIds, companyName, contractName, billToClientName, searchCondition);
                return Content(expenseData, "application/json");
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
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PostContractFieldsGet")]
        public async Task<IActionResult> PostContractGet(string moduleName, string subModuleCode)
        {
            string msg = _contracts.GetCustomFieldForContract(CompanyId, UserId, moduleName, subModuleCode);
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
        /// This method is used for saving the Purchase order data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostContract")]
        public async Task<IActionResult> PostContract([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _contracts.SaveContract(postString, CompanyId, UserId);
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