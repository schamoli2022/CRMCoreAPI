using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using CRMCoreAPI.Common;
using Newtonsoft.Json;

namespace CRMCoreAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly string connectionString = null;
        public ItemService(IConfiguration configuration)
        {
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string getItemList(string item, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    dynamicParameters.Add("@STATUS_ID", statusId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@CATALOGUE", item);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var CampaignListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_ITEM_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return CampaignListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string getItemListForAssociate(string itemName, string ItemUniqueName, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, long? sourceId, string requestType,string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    dynamicParameters.Add("@STATUS_ID", statusId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@ITEMNAME", itemName);
                    dynamicParameters.Add("@ITEMUNIQUENAME", ItemUniqueName);
                    dynamicParameters.Add("@REQUEST_TYPE", requestType);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var CampaignListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_RELATED_ITEM_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return CampaignListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string getItemTypeList(string itemType, long companyId, long userId, long? statusId, string subModuleCode, string moduleName, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    dynamicParameters.Add("@STATUS_ID", statusId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@ITEM_TYPE", itemType);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var CampaignListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_ITEM_TYPE_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return CampaignListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        /// <summary>
        /// Check item quantity its available in stock or not
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public string CheckItemQuantity(long companyId, long loggedInUserId, long itemId)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@ID", itemId);
                    var returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_CHECK_ITEM_QUANTITY", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string SaveProductPrice(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", "[" + Convert.ToString(sys.postString) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var result = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_AP_SAVE_ACCOUNT_PRODUCT_PRICE", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
    }
}
