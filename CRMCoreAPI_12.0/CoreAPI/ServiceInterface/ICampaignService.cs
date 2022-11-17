using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICampaignService
    {
        /// <summary>
        /// This method is used to get listing of campaigns
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="campaignName"></param>
        /// <param name="campaignOwnerIds"></param>
        /// <param name="campaignType"></param>
        /// <param name="campaignDateFrom"></param>
        /// <param name="campaignDateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        string GetCampaigns(long companyId, long loggedInUserId, string statusIDs, string campaignName, string campaignOwnerIds, string campaignType, string campaignDateFrom, string campaignDateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);

        /// <summary>
        /// Used to get custom field that is in custom field table
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetCustomFieldForCampaign(long companyId, long userId, string moduleName, string subModuleCode);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="campaignData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveCampaign(string campaignData, long CompanyId, long UserId);
        /// <summary>
        /// This method is used for Convert to Lead From Campaign in Contact Tab
        /// </summary>
        /// <param name="campaignData"></param>
        /// <returns>Returns the fields in json string format</returns>
        /// <returns></returns>
        string SaveConvertToLeadCampaign(string campaignData, long CompanyId, long UserId);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="campaignData"></param>
        /// <returns></returns>
        string ValidateData(string campaignData);
        /// <summary>
        /// This method is used for Add Group Contact Name
        /// </summary>
        /// <param name="groupData"></param>
        /// <returns>Returns the fields in json string format</returns>
        /// <returns></returns>
        string SaveGroupContactCampaign(string groupData, long CompanyId, long UserId);
        string AddGroupEmailTemplate(string addemailtemplate, long CompanyId, long UserId);
        string SaveEmailScheduler(string schedulerData, long CompanyId, long UserId);
    }
}
