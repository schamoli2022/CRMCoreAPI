using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Get product listing
    /// </summary>
    public interface ITerritoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TerritoryName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        string GetTerritory(string TerritoryName, string ParentTerritoryName, string UserName, int? TerritoryID, long CompanyID, string CreatedAt, string ModifiedAt, string LangCode, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber,string searchCondition);

        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="TerritoryName"></param>
        /// <returns></returns>

        //string ValidateData(string ProductData);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        ///   /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldTerritory(long companyId, long userID, string moduleName, string subModuleCode);
        /// </summary>
        /// <param name="TerritoryData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveTerritory(string TerritoryData, long CompanyId, long UserId);
    }
}