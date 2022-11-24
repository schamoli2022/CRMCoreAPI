using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="moduleId"></param>
        /// <param name="sourceId"></param>
        /// <param name="sourceTable"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        string getActivities(long CompanyId, long UserId, string moduleCode, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode);

        /// <summary>
        /// add task api for save Task in Activity Tab
        /// </summary>
        /// <param name="addtask"></param>
        /// <returns></returns>
       string Addtask(string addtask, long CompanyId, long UserId);

       string SaveScheduleCall(string saveScheduleCall, long CompanyId, long UserId);

    
        string updateActivity(long? id);



    }
}
