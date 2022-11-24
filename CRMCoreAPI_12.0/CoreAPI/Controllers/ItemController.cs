using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMCoreAPI.Services;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("ItemTypeListing")]
        public async Task<IActionResult> ItemTypeListing(string itemType, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var expenseData = _itemService.getItemTypeList(itemType, CompanyId, UserId, statusId, subModuleCode, moduleName, sortBy, sortExp, pageSize, pageNumber, langCode,searchCondition);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("ItemListing")]
        public async Task<IActionResult> ItemListing(string item, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            try
            {
                var expenseData = _itemService.getItemList(item, CompanyId, UserId, statusId, subModuleCode, moduleName, sortBy, sortExp, pageSize, pageNumber, langCode,searchCondition);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("getItemListForAssociate")]
        public async Task<IActionResult> getItemListForAssociate(string itemName, string ItemUniqueName, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, long? id, string requestType, string searchCondition)
        {
            try
            {
                var expenseData = _itemService.getItemListForAssociate(itemName, ItemUniqueName, CompanyId, UserId, statusId, subModuleCode, moduleName, sortBy, sortExp, pageSize, pageNumber, langCode, id, requestType, searchCondition);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)    
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// Check item quantity its available in stock or not
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet, Route("CheckItemQuantity")]
        public async Task<IActionResult> CheckItemQuantity(long itemId)
        {
            try
            {
                var data = _itemService.CheckItemQuantity(CompanyId, UserId, itemId);
                return Content(data, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveProductPrice")]
        public async Task<IActionResult> SaveProductPrice([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _itemService.SaveProductPrice(postString, CompanyId, UserId);
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