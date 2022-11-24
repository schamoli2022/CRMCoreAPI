using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IItemService
    {
        string getItemTypeList(string itemType, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);

        string getItemList(string item, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);

        string getItemListForAssociate(string itemName, string ItemUniqueName, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, long? sourceId, string requestType,string searchCondition);

        /// <summary>
        /// Check item quantity its available in stock or not
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        string CheckItemQuantity(long companyId, long userId, long itemId);

        string SaveProductPrice(string postString, long CompanyId, long UserId);

    }
}
