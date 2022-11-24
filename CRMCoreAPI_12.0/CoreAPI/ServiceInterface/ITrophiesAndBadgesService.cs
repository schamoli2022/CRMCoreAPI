using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public interface ITrophiesAndBadgesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="trophyOrBadgeName"></param>
        /// <param name="action"></param>
        /// TYPE - there is only two type type exist in datatvle that in TROPHY and BADGE 
		/// if you pass TROPHY or BADGE then it give data accordingly. 
        /// <param name="type"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        string getTrohpiesOrBadges(long companyId, long loggedInUserId, string trophyOrBadgeName, string action, string type,string status_Ids, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);

        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="TrophiesOrBadgestData"></param>
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
        string GetcustomFieldgetTrohpiesOrBadges(long companyId, long userId, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="TrophiesAndBadges"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveTrophiesAndBadges(string TrophiesAndBadgesData, long CompanyId, long UserId);
    }
}
