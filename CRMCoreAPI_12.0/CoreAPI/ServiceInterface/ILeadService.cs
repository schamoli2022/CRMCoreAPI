
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.LeadService;


namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Lead Interface
    /// </summary>
    public interface ILeadService
    {
        /// <summary>
        /// Implementation of Lead listing method
        /// </summary>
        /// <returns></returns>
        string GetLeads(long companyId, long viewerId, string clientName, string statusIds, string clientIds, string companyName, string channelIds, string leadOwnerIds, string leadDateFrom, string leadDateTo, byte isPaged, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNum, char crmtype,string searchCondition, bool isExport);

        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="leadData"></param>
        /// <returns></returns>
        string ValidateData(string leadData);

        /// <summary>
        /// Implementation of save lead
        /// </summary>
        /// <param name="leadData"></param>
        /// <returns></returns>
        string SaveLead(string leadData, long CompanyId, long UserId);
        string SaveVdeskLead(string leadData);


        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldLead(long companyId,long userId, string moduleName, string subModuleCode);

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
        //string SaveListLayout(string postData);

        /// <summary>
        /// Save Page layout into TALYGEN_CUSTOM_FIELD_mapping table
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        //string SavePageLayout(string postData);

        /// <summary>
        /// Converts lead to the account and created a mapping of contacts
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        string convertLeadToAccount(string postData, long CompanyId, long UserId);
        /// <summary>
        /// close lead
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string CloseLead(string postString, long CompanyId, long UserId);
        string ChangeOwner(string postString, long CompanyId, long UserId);
        string GetExportDetail(long companyId, string moduleName, string subModuleCode);
        string GetLeadImport(string batchid);
        string SaveLeadImport(string postString, long CompanyId, long UserId);
        string GetUnifiedLead(long contactId, long companyId, long userId);
        string SaveUnifiedLead(string postString, long CompanyId, long UserId);

        string ReopenLead(string postString, long companyId, long userId);

        string GetAllLeadOwnerPermission(long companyId, long userId, string subModuleCode, long? leadOwner, long? leadId);

        string saveAdditionalOwnerPermission(string postString, long companyId, long userId);

        string saveAdditionalDealOwnerPermission(string postString, long companyId, long userId);
        string additionalLeadOwnerListing(long id, long companyId, long userId, string subModuleCode);

        string DeleteAttachment(long id, long sourceid);
        string DeleteAccountBeforeDeal(string postString, long CompanyId, long UserId);
    }
}
