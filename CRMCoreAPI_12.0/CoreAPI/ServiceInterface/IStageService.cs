using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.StageService;

namespace CRMCoreAPI.ServiceInterface
{
    
       /// <summary>
          /// Default Lead Interface
          /// </summary>
        public interface IStageService
        {
        /// <summary>
        /// Get All Lead listing
        /// </summary>
        /// <returns></returns>
        /// 
        string GetStages(long companyId, long loggedUserId, string stagename, string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition);

        string GetMappingStages(int sourceId, long companyId, long loggedUserId, string moduleName, string subModuleCode);

        /// <summary>
        /// Return the custom fields of table to bind in form on the basis of passed parameters those companyId, moduleName, SubmoduleCode
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>return JSON String  as data</returns>
        string GetcustomFieldStage(long companyId, long userId, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="stageData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveStage(string stageData, long CompanyId, long UserId);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="stageData"></param>
        /// <returns></returns>
        string ValidateData(string stageData);

        string updateDealStage(string postString, long CompanyId, long UserId);
    }
    
}
