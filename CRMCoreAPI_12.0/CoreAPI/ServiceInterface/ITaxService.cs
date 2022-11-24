using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface ITaxService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        Task<string> SaveTaxCode(string postString, long CompanyId, long UserId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        Task<string> SaveTax(string postString, long CompanyId, long UserId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetTaxByCodeId(long companyId, long loggedInUserId, long? id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="taxCode"></param>
        /// <returns></returns>
        Task<string> GetTaxByCode(long companyId, long loggedInUserId, string taxCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetAllTaxCode(long companyId, long userId, long? id);
    }
}
