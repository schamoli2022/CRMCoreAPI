namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Layout Interface
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetDDLData(long? id, long companyId, long userId, string moduleName, string type, string search,long? refId);
        string GetDDLDataBySubModule(long? id, long companyId, long userId, string moduleName, string subModuleName, string type, string search, long? refId);
        string GetDDLCommunicationModeData(long? id, long companyId, long userId, string moduleName, string type, string search);

        /// <summary>
        ///  Common Delete method to delete the data 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string CommonDeleteData(string postString, long CompanyId, long UserId);

        //
        /// <summary>
        string GetEmailData(long? id); 

        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string SendEmailData(string postString, long CompanyId, long UserId);

        /// <summary>
        /// Common method for fetching email log by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        string GetEmailLogById(long? id, long companyId, long loggedInUserId);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="id"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        string GetEmails(long? id,string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber);
        string GetScheduleEmails(long? id, string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber);
        string GetEmailTemplates(long? id, string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber);

        string SaveAttachment(string path, string attachmentType, long sourceId, string moduleName, string subModuleCode, string fileName, long userId,long companyId, string thumbNailPath = null);
        string GetTimelineData(long id, long companyId, long userId, string subModuleCode); 
        string GetEmailById(string ids,long companyId,long loggedInUserId);
        string AddEmailTemplate(string addemailtemplate, long CompanyId, long UserId);

        string SaveTax(string postString, long CompanyId, long UserId);

        /// <summary>
        /// Get Search criteria list for a user with resptect to module and sub module
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetSearchCriteriaListing(long companyId, long userId, string moduleName, string subModuleCode);

        /// <summary>
        /// Get field name on basis of module and submodule
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetFieldName(long companyId, long userId, string moduleName, string subModuleCode, string reqFrom = null);

        /// <summary>
        /// Get operator list with respet to data type
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        string GetOperatorList(long companyId, long userId, string dataType, long? customField);
        /// <summary>
        /// Save search filter data with resptect to user id to search the data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string SaveSearchFilter(string postString, long CompanyId, long UserId);

        /// <summary>
        /// get the search criteria by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        string GetSearchCriteriaById(long id,long companyId, long userId, string moduleName, string subModuleCode, int? isDelete);
        string GetExistOrNot(long companyId, string name, string type, long id = 0, long? refid = null);

        /// <summary>
        /// For fetching encrypted password
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string GetEncryptedPassword(long companyId, string password);
        string GetUnifiedProjectManagementListing(string mobileNum, long? companyId, long? userId);
        string GetUnifiedTicketingListing(string mobileNum, long? companyId, long? userId);
        string GetCallLogsListing(long refid, long? companyId, long? userId, string subModuleCode);

        string SaveIndustryType(string postString, long CompanyId, long UserId);
        string SaveConfiguringJsonData(string postString, long CompanyId, long UserId);

        string GetConfigNumberJsonData(long? id, long CompanyId);

        string SaveRuleFilter(string postString, long CompanyId, long UserId);
        string GetRuleCriteria(long companyId, long userId, string moduleName, string subModuleCode);
        string GetDuplicateLeads(long companyId, long userId, string moduleName, string subModuleCode,string ruleCondition);
    }
}
