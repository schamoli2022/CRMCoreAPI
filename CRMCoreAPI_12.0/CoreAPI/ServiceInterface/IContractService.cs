using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContractService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetContracts(long companyId, string contract_Name, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long userId, string clientIds, string resellerIds, string ProductIds, string contractIds, string companyName, string contractName, string billToClientName, string searchCondition);
        /// <summary>
        /// Used to get custom field that is in custom field table
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetCustomFieldForContract(long companyId, long userId, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="contractData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveContract(string contractData, long CompanyId, long UserId);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="contractData"></param>
        /// <returns></returns>
        string ValidateData(string contractData);
    }
}
