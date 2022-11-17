using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public interface IVendorService
    {
        string GetVendors(long companyId, long loggedInUserId, string statusIDs, string createdBy, string vendorName, string phoneNumber, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition);
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldVendor(long companyId, long userID, string moduleName, string subModuleCode);
            /// </summary>
            /// <param name="VendorData"></param>
            /// <returns>Returns the fields in json string format</returns>
         string SaveVendor(string VendorData, long CompanyId, long UserId);
    }   
}
