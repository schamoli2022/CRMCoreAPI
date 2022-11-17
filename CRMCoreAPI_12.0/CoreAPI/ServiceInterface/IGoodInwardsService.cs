
namespace CRMCoreAPI.ServiceInterface
{
    public interface IGoodInwardsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        string getPurchaseOrderProductDetails(long companyId, long userId, long? sourceId, string moduleName, string subModuleCode, string purchaseOrderIds);

        /// <summary>
        /// RECEIVE A JSON STRING 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string SaveGoodInwards(string postString, long CompanyId, long UserId);
        string GetGoodInward(long companyId, long loggedUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition);

        string commonApproveRejectInwards(string postString, long CompanyId, long UserId);
        

        string getGoodInwardAssociatedProducts(long companyId, long userId, string moduleName, string subModuleCode, long id);

        string getGoodInwardProductsForPO(long companyId, long userId, string moduleName, string subModuleCode, long purchaseOrderId);
        string getGoodInwardProductsForVendor(long companyId, long userId, string moduleName, string subModuleCode, long purchaseOrderId); 
    }
}
