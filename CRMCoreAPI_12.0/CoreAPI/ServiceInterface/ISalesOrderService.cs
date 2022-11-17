using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Service
{
    public interface ISalesOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="OrderSubject"></param>
        /// <param name="DealName"></param>
        /// <param name="SalesOwner"></param>
        /// <param name="LeadOwner"></param>
        /// <param name="ContactName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="AccountName"></param>
        /// <param name="leadName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// sourceId used to get sales order listing according to the mapping with respect to sub module
        /// <param name="sourceId"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="isPaged"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        string GetSalesOrder(int? Status, string OrderSubject, string DealName, string SalesOwner, string LeadOwner, string ContactName, string dateFrom, string dateTo, string AccountName, string leadName, long companyId, long loggedInUserId, int? sourceId, string subModuleCode, string sortBy, string sortExp, byte? isPaged, int? pageSize, int? pageNumber,long? dealId, string searchCondition);

        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="OrderSubject"></param>
        /// <returns></returns>

        //string ValidateData(string ProductData);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="userId"></param>
        /// <returns>Returns the fields in json string format</returns>

        string GetcustomFieldSalesOrder(long companyId, string moduleName, string subModuleCode, long userId);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="priceBookData"></param>
        /// <returns>Returns the fields in json string format</returns>
        //string SaveSalesOrder(string SalesOrderData);
    }
}
