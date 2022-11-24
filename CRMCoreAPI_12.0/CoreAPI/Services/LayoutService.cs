using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.ServiceModels;
using System.Text.RegularExpressions;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class LayoutService : ILayoutService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public LayoutService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetLayout(long? id, long companyId, long userId, string moduleName, string subModuleCode, string otherData,string langCode="")
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
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    string customField = "";
                    if (id.HasValue)
                    {                        
                        dynamicParameters.Add("@PRIMARY_KEY_VALUE", id);
                        customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_FIELDS_AND_VALUE_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
                    else
                    {
                        dynamicParameters.Add("@OTHER_DATA", System.Web.HttpUtility.UrlDecode(otherData));
                        customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_FIELDS_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
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
        ///  This method is used for fetching the available and selected custom fields for list layout
        ///  basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetListLayoutFields(long companyId, long userId, string moduleName, string subModuleCode)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@VIEWER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_AVAILABLE_SELECTED_FIELDS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        public string SaveFieldData(string postString, long CompanyId, long UserId)
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
                        string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@json", jsonS);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_LEAD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
            // return "true";            
            catch (Exception ex)
            {
                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
        }
        /// <summary>
        ///  Save list layout into TALYGEN_CUSTOM_FIELD_COMPANY_VIEWPORT table
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveListLayout(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();      
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                int index = 1;
                dynamic data = null;

                foreach (var attr in sys)
                {
                    if (index == 1)
                    {
                        data = attr.Value;
                        break;
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@JSON", jsonS);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);

                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_LIST_LAYOUT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        
                        if (returnId != null)
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
        /// <summary>
        ///  Save Page layout into TALYGEN_CUSTOM_FIELD_mapping table
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SavePageLayout(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                int index = 1;
                dynamic data = null;

                foreach (var attr in sys)
                {
                    if (index == 1)
                    {
                        data = attr.Value;
                        break;
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@JSON", jsonS);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var returnId = sqlConnection.Query<string>("TALYGEN_SERVICE_SAVE_FIELDS_AND_GROUP_PAGE_LAYOUT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        if (returnId != null)
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
        /// <summary>
        /// This method is used to view the custom fields form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string View(long? id, long companyId, long userId, string moduleName, string subModuleCode)
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


                    dynamicParameters.Add("@PRIMARY_KEY_VALUE", id);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_VIEW_DETAILS_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
        ///  Get Sub module list on basis of module id for manage the layout
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public string GetSubModuleList(long? id, long companyId, long userId, string sortBy, string sortExp, int? pageSize, int? pageNum)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNum);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_SUB_MODULE_BY_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
        ///  Get module list  for manage the layout
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetModuleList(long companyId, long userId)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    string customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_MODULES", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return customField;
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
