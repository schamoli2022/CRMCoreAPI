using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Common;
using CRMCoreAPI.ServiceInterface;
using CRMCoreAPI.ServiceModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{

    public class GoodInwardsService : IGoodInwardsService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public GoodInwardsService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        public string getPurchaseOrderProductDetails(long companyId, long userId, long? sourceId, string moduleName, string subModuleCode, string purchaseOrderIds)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@PRIMARY_KEY_VALUE", purchaseOrderIds);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_CRM_GET_PURCHASE_ORDER_PRODCUCTS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string SaveGoodInwards(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                Dictionary<string, dynamic> jsonData = new Dictionary<string, dynamic>();
                jsonData["data"] = tempJson.data;
                jsonData["ProductDetails"] = tempJson.ProductDetails;

                if (jsonData.Count() > 0)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", Convert.ToString(JsonConvert.SerializeObject(jsonData)));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_GOOD_INWARD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(leadId);
                        if (returnData != null)
                        {
                            //var returnobj = JsonConvert.DeserializeObject(Convert.ToString(leadId));
                            //string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            //string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);
                            //if (code == "1" && chkOperation == "INSERT")
                            //{
                            //    var objCommon = new CommonClass(configuration);
                            //    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_DEAL_INSERT.ToString());

                            //    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_DEAL_INSERT.ToString(), "CRM_DEAL_INSERT_FULL", title);
                            //}
                        }
                        return returnData;
                    }
                }
                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }           
            catch (Exception ex)
            {
                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
        }
        public string GetGoodInward(long companyId, long loggedUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    //CRMData dData = new CRMData();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", "EN");
                    dynamicParameters.Add("@TOTAL_RECORDS", totalRecords);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICES_CRM_GET_GOODINWARD_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string commonApproveRejectInwards(string postString, long CompanyId, long UserId)
         {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUES", postedData.ids.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@MODULE_NAME", "CRM");
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@APPROVALTYPE", postedData.approvaltype.Value);
                    dynamicParameters.Add("@VIEW_MODE", postedData.viewmode.Value);
                    dynamicParameters.Add("@LOCATION_ID", postedData.locationid.Value);
                    var taskId = sqlConnection.Query<string>("TALYGEN_SERVICE_UPDATE_INWARD_STATUS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (taskId != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = taskId
                        });

                        return JsonConvert.SerializeObject(errorMessage);
                    }
                    else
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Failure"
                        });
                        return JsonConvert.SerializeObject(errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    errorMessage.Add(new ListString()
                    {
                        Status = "Failure"
                    });
                    return JsonConvert.SerializeObject(errorMessage);
                }
            }
        }  

      

        public string getGoodInwardAssociatedProducts(long companyId, long userId, string moduleName, string subModuleCode, long id)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_NAME", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", id);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_CRM_GET_GIN_PRODUCT_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string getGoodInwardProductsForPO(long companyId, long userId, string moduleName, string subModuleCode, long purchaseOrderId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", purchaseOrderId);
                    string ginProducts = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_GIN_DETAILS_FOR_PO", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return ginProducts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        public string getGoodInwardProductsForVendor(long companyId, long userId, string moduleName, string subModuleCode, long purchaseOrderId)
        { 
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", purchaseOrderId);
                    string ginProducts = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_GIN_LISTING_FOR_VENDOR", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return ginProducts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
