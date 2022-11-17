
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public ProductService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

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
        public string GetProducts(string productName, long companyId, long loggedInUserId, int? sourceId, string subModuleCode, int? statusId, string requestType, string sortBy, string sortExp, int pageSize, int pageNumber, string productUniqueId, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (productName == "")
                    {
                        productName = null;
                    }
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCT_NAME", productName);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@STATUS_ID", statusId);
                    dynamicParameters.Add("@REQUEST_TYPE", requestType);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize); 
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@PRODUCTUNIQUEID", productUniqueId);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_PRODUCT_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetcustomFieldProduct(long companyId, long userId, string moduleName, string subModuleCode)
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
                    var customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_FIELDS_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveProduct(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            dynamic data = null;
            try
            {
                foreach (var attr in sys)
                {
                    var newObj = Convert.ToString(attr.Value);

                    dynamic sysss = JsonConvert.DeserializeObject(newObj);

                    foreach (var name in sysss)
                    {
                        if (index == 1)
                        {
                            data = name.Value;
                            break;
                        }
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        //string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@json", Convert.ToString(data));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_PRODUCT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                        //return "true";
                        if (leadId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success"
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

        public string SaveProductForMapping(string productMappingData, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(productMappingData);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            List<object> productIds = new List<object>();
            foreach (var item in postedData.productIds)
            {
                if(postedData.subModuleCode.Value=="CRM_LEADS" || postedData.subModuleCode.Value == "CRM_DEALS")
                    productIds.Add(item);
                else
                   productIds.Add(Convert.ToInt64(item));
            }
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@PRODUCT_IDS", String.Join(",", productIds));
                    dynamicParameters.Add("@SOURCE_ID", postedData.sourceId.Value);
                    dynamicParameters.Add("@SOURCE_NAME", postedData.sourceName.Value);
                    //TALYGEN_SP_SERVICE_CRM_ADD_PRODUCT_MAPPING
                    var noteId =sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_ADD_PRODUCT_MAPPING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (noteId != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = noteId
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

        public string getProductTypeList(string productTypeName, long companyId, long loggedInUserId, string sortBy, string sortExp, int? pageSize, int? pageNumber, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (productTypeName == "")
                    {
                        productTypeName = null;
                    }
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCT_TYPE_NAME", productTypeName);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_PRODUCT_TYPE_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// <summary>
        /// Save Proct type into database
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveProductType(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            dynamic data = null;
            try
            {
                foreach (var attr in sys)
                {
                    var newObj = Convert.ToString(attr.Value);

                    dynamic sysss = JsonConvert.DeserializeObject(newObj);

                    foreach (var name in sysss)
                    {
                        if (index == 1)
                        {
                            data = name.Value;
                            break;
                        }
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", Convert.ToString(data));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        string returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_PRODUCT_TYPE_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                       
                        return CommonClass.ReturnMessage(returnData);                        
                    }
                }
                errorMessage.Add(new ListString()
                {
                    Status = "Failure",
                    Name = "JSON Data is Empty",
                    Code = "0"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage.Add(new ListString()
                {
                    Status = "Failure",
                    Name = ex.Message,
                    Code = "0"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
        }

        public string GetProductTypeTax(long companyId, long loggedInUserId, long id)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@ID", id);
                    var customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_PRODUCT_TYPE_TAX", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

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
        public string GetTaxByProduct(long companyId, long userId, string state, string zipcode, string district, long countryId, long projectId)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@STATE", state);
                    dynamicParameters.Add("@ZIP_CODE", zipcode);
                    dynamicParameters.Add("@DISTRICT_ID", district);
                    dynamicParameters.Add("@COUNTRY_ID", countryId);
                    dynamicParameters.Add("@PRODUCT_ID", projectId);
                    var data = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_TAX_BY_PRODUCT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return data;
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
