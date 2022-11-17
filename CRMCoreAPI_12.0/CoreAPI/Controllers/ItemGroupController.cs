using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ItemGroupController : BaseController
    {
        private readonly IitemsGroupService _iitemGroupService;
        private IConfiguration configuration;
        public ItemGroupController(IitemsGroupService iitemGroupService, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _iitemGroupService = iitemGroupService;
            configuration = config;
        }

        [HttpGet]
        [Route("ItemGroupListing")]
        public async Task<IActionResult> ItemGroupListing(string sortBy, string sortExp, int pageSize, int pageNumber)
        {
            try
            {
                var expenseData = _iitemGroupService.GetItems(CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        //[HttpPost]
        //[Route("SaveItem")]
        //public async Task<IActionResult> SaveItem([FromBody] string postString)
        //{
        //  if (postString == null)
        //    {
        //        return Content("Failure", "application/json");
        //    }
        //    string msg = _iitemGroupService.SaveItem(postString);
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

    }
}