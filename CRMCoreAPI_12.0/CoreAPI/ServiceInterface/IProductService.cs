using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Get product listing
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sourceId"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="statusId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        string GetProducts(string productName, long companyId, long loggedInUserId, int? sourceId, string subModuleCode, int? statusId, string requestType, string sortBy, string sortExp, int pageSize, int pageNumber, string productUniqueId, string searchCondition);

        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="ProductData"></param>
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
        string GetcustomFieldProduct(long companyId, long userId, string moduleName, string subModuleCode);
        /// </summary>
        /// <param name="ProductData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SaveProduct(string ProductData, long CompanyId, long UserId);

        string SaveProductForMapping(string productMappingData, long CompanyId, long UserId);

        string getProductTypeList(string productTypeName, long companyId, long loggedInUserId, string sortBy, string sortExp, int? pageSize, int? pageNumber, string searchCondition);

        /// <summary>
        /// Save Product Type
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        string SaveProductType(string postData, long CompanyId, long UserId);

        string GetProductTypeTax(long companyId, long loggedInUserId, long id);

        /// <summary>
        /// Get tax by product or country or state or zipcode
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        /// <param name="zipcode"></param>
        /// <param name="countryId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        string GetTaxByProduct(long companyId, long userId, string state, string zipcode,string district, long countryId, long projectId);
    }
}
