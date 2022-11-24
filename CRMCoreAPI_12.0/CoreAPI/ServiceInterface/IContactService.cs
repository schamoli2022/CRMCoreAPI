using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface IContactService
    {
        string GetContacts(string First_Name, string Last_Name, string job_title, string email_id, string business_phone, string mobile_phone, string fax, string preferred_contact, long company_id, long loggedInUserId, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, int? statusId, string requestType, string searchCondition,string AccountType, int? clientId);
        /// <summary>
        /// Used to get custom field that is in custom field table
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        /// 
        string GetcustomFieldContact(long company_id, string moduleName, string subModuleCode, long userId);
        
        string SaveContactWithMapping(string postString, long CompanyId, long UserId);
        string SaveExistingContactWithMapping(string postString, long CompanyId, long UserId);

        string SaveContact(string postString, long CompanyId, long UserId);
        string SaveUnifiedContact(string postString, long CompanyId, long UserId);

        string getSingleContact(long id, long companyId, long userId);
        string getUnifiedContact(string mobileNum, long? companyId, long? userId);
        string GetGroupContacts(int? sourceId, long company_id, string subModuleCode, long userId);

        string SaveContactImport(string postString, long CompanyId, long UserId,string SubModule);
        string SaveClientInfo(string postString, long CompanyId, long UserId);

        string DeleteClientContact(string postString, long CompanyId);
        string UserRole(long CompanyId,long userId,int usertype);

        string SaveContactRoleTitle(string postString, long CompanyId, long UserId);
        string SaveContactAsPrimary(string postString, long CompanyId, long UserId);
    }
}
