
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.AccountService;


namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Account Interface
    /// </summary>
    public interface IDealService
    {
        /// <summary>
        /// Get All Deal listing
        /// </summary>
        /// <returns></returns>
        string GetDeal(string Dealname, int? statusID, int? createdBy, string createdAt, string closingDate, long companyID,long loggedInUserId, int? sourceId, string subModuleCode, string sortBy,string sortExp,int pagesize, int pagenumber, string searchCondition);


        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="DealData"></param>
        /// <returns></returns>

        //string ValidateData(string DealData);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldDeal(long companyId, long userID, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="DealData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveDeal(string DealData, long CompanyId, long UserId);
        string GetUnifiedDeal(long contactId, long companyId, long userId);
        string RevertAction(string RevertData, long CompanyId, long UserId);
        string PrimaryContactForDeal(long CompanyId, long? sourceid, string submodulecode);
    }
}
