using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="accountName"></param>
        /// <param name="companyName"></param>
        /// <param name="leadOwners"></param>
        /// <param name="dealName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        string GetPurchaseOrders(long companyId, long loggedInUserId, string statusIDs, string accountName, string companyName, string leadOwners, string dealName, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldPurchaseOrders(long companyId, long userID, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="purchaseOrderData"></param>
        /// <returns>Returns the fields in json string format</returns>
        //string SavePurchaseOrder(string purchaseOrderData);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="purchaseOrderData"></param>
        /// <returns></returns>
        string ValidateData(string purchaseOrderData);

        string approveRejectPurchaseOrder(string postString, long CompanyId, long UserId);
    }
}
