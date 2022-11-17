
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.AccountService;


namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Account Interface
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Get All Account listing
        /// </summary>
        /// <returns></returns>
        string GetAccounts(long UserID, long CompanyId, string ClientName, string EmailId, string UserPhone, int TotalProject, DateTime CreatedAt, string StatusName, int StatusId, string Avatar, string StatusCode, int LeadId, string crmtype, int parentId, int TotalChild);
        string SaveAccountInfo(string Accountdata, long CompanyId, long UserId);
        string GetAccountsType();
        string SaveAccountAddress(string postString, long CompanyId, long UserId);

        string AccountAddressListing(long CompanyId, long UserId, string moduleCode, int? sourceId, string subModuleCode);
    }
}
