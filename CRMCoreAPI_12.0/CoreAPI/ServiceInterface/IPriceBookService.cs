using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Get product listing
    /// </summary>
    public interface IPriceBookService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        string GetPriceBook(long? id,int ? PriceBookOwner, string PriceBookName, int ? PriceBookModel, string Description, int ? CreatedBy, string CreatedAt, string ModifiedAt, int ? StatusID, long CompanyID, string LangCode, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber,  string subModuleCode, string requestType, string searchCondition);

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
        ///   /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldPriceBook(long companyId, long userID, string moduleName, string subModuleCode);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="priceBookData"></param>
        /// <returns>Returns the fields in json string format</returns>
        string SavePriceBook(string priceBookData, long CompanyId, long UserId);
        /// <summary>
        /// Implementation of validation method
        /// </summary>
        /// <param name="priceBookData"></param>
        /// <returns></returns>
        string ValidateData(string priceBookData);

        string SavePricebookForMapping(string bookMappingData, long CompanyId, long UserId);

        /// <summary>
        /// Get Price book list for a product
        /// </summary>
        /// <param name="search"></param>
        /// <param name="refId"></param>
        /// <param name="productId"></param>
        /// <param name="CompanyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetPriceBookListingForProduct(string search, long? refId, long? productId, long companyId, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, string subModuleCode);


    }
}