using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.Common;
using Newtonsoft.Json.Linq;
using CRMCoreAPI.ServiceModels;

namespace CRMCoreAPI.Services
{
    public class VendorService:IVendorService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public VendorService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string GetVendors(long companyId, long loggedInUserId, string statusIDs, string createdBy, string vendorName, string phoneNumber, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@STATUS_IDS", statusIDs);
                    dynamicParameters.Add("@CREATED_BY", createdBy);
                    dynamicParameters.Add("@VENDOR_NAME", vendorName);
                    dynamicParameters.Add("@PHONE_NUMBER", phoneNumber);
                    dynamicParameters.Add("@DATE_FROM", dateFrom);
                    dynamicParameters.Add("@DATE_TO", dateTo);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_VENDOR_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return leadListing;
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
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetcustomFieldVendor(long companyId, long userID, string moduleName, string subModuleCode)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userID);
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
        public string SaveVendor(string postString, long CompanyId, long UserId)
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
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_VENDOR_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        var returnData = CommonClass.ReturnMessage(leadId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(leadId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_VENDOR_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_VENDOR_INSERT.ToString(), "CRM_VENDOR_INSERT_FULL",title);
                            }
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
    }
}
