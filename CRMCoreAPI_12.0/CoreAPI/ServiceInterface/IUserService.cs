
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TALYGEN;
using static CRMCoreAPI.Services.AccountService;


namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Account Interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get All Account listing
        /// </summary>
        /// <returns></returns>
        List<RoleClass> GetAllRoles_Privilege_Allowed(long userId, long companyId, string userType, string controller, string action);
        List<RoleClass> GetAllRoles_Privileges(long userId, long companyId, string userType);
    }
}
