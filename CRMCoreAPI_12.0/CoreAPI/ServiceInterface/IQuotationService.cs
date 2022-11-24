using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuotationService
    {
        /// <summary>
        /// this function return the quotation listing  on the basis of passed parameters
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="title"></param>
        /// <param name="quotationNumber"></param>
        /// sourceId is used for mapping. By this id we find those quotation which are relate to this source id
        /// <param name="sourceId"></param>
        /// submodule is used with source id to identify submodule of source id.
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="statuses"></param>
        /// <param name="quotationTypeCode"></param>
        /// <returns>return JSON string </returns>
        string GetQuotes(long companyId, long loggedInUserId, string startDate, string endDate, string title, string quotationNumber, int? sourceId, string subModuleCode, string sortBy, string sortExp, int pageSize, int pageNumber, string statuses, string quotationTypeCode,long? dealId, string searchCondition);
        
        /// <summary>
        /// Return the custom fields of table to bind in form on the basis of passed parameters those companyId, moduleName, SubmoduleCode
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>return JSON String  as data</returns>
        string GetcustomFieldQuotation(long companyId, string moduleName, string subModuleCode,long UserId);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="quotationData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveQuotation(string quotationData, long CompanyId, long UserId);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="quotationData"></param>
        /// <returns></returns>
        string ValidateData(string quotationData);

        /// <summary>
        /// Convert order item to other
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string ConvertOrder(string postString, long CompanyId, long UserId);

        string QuotationDetails(long? id, long? companyId, long? loggedInUserId, string moduleCode, string subModuleCode);

        string FeedBackDetails(long? companyId, long? loggedInUserId, long? invoiceId);

        string GetUnifiedOrderListing(long contactId, string type_code, long? companyId, long? userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string QuoteToProject(string postString, long CompanyId, long UserId);

        string  GetQuoteProductbyDeal(long companyId, long userId, long? sourceid);

        string GetAddressdata(long CompanyId, long UserId, long? addressid, long? sourceid);
    }
}
