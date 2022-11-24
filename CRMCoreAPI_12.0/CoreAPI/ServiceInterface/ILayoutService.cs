namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Layout Interface
    /// </summary>
    public interface ILayoutService
    {
               /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetLayout(long? id, long companyId,long userId, string moduleName, string subModuleCode, string otherData, string langCode = "");

        /// <summary>
        /// Implementation of save lead
        /// </summary>
        /// <param name="leadData"></param>
        /// <returns></returns>
        string SaveFieldData(string leadData, long CompanyId, long UserId);
        /// <summary>
        ///  This method is used for fetching the available and selected custom fields for list layout
        ///  basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetListLayoutFields(long companyId, long userId, string moduleName, string subModuleCode);

        /// <summary>
        /// Save list layout into TALYGEN_CUSTOM_FIELD_COMPANY_VIEWPORT table
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        string SaveListLayout(string postData, long CompanyId, long UserId);

        /// <summary>
        /// Save Page layout into TALYGEN_CUSTOM_FIELD_mapping table
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        string SavePageLayout(string postData, long CompanyId, long UserId);

        /// <summary>
        /// This method is used to view the custom fields form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string View(long? id, long companyId, long userId, string moduleName, string subModuleCode);

        /// <summary>
        /// Get Sub module list on basis of module id for manage the layout
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        string GetSubModuleList(long? id, long companyId, long userId, string sortBy, string sortExp, int? pageSize, int? pageNum);

        /// <summary>
        ///  Get module list  for manage the layout
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetModuleList(long companyId, long userId);
    }
}
