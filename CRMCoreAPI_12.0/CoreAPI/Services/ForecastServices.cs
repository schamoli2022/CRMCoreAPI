using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Common;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
 
    public class ForecastServices: IForecastService

    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public ForecastServices(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetForecast(int? forecast_id, long loggedInUserId, string ForecastName, string statusIds, long companyId, string forecastCreatedDate, string created_by, string forecastDateTo, byte? isPaged, string sortBy, string sortExp, int? pageSize, int? pageNum)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (ForecastName == "")
                    {
                        ForecastName = null;
                    }
                    if (forecastCreatedDate == "")
                    {
                        forecastCreatedDate = null;
                    }
                    if (forecastDateTo == "")
                    {
                        forecastDateTo = null;
                    }
                    if (statusIds == "")
                    {
                        statusIds = null;
                    }
                  
                   if(forecastDateTo=="")
                    {
                        forecastDateTo = null;
                    }

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@forecast_id", forecast_id);
                    dynamicParameters.Add("@company_id", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@created_at", forecastCreatedDate);
                    dynamicParameters.Add("@created_by", created_by);
                    dynamicParameters.Add("@forecast_category_name", ForecastName);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNum);
                    //dynamicParameters.Add("@CRM_TYPE", crmtype);
                    dynamicParameters.Add("@status_id", statusIds);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_FORECAST_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        public string GetcustomFieldForecast(long companyId, string moduleName, string subModuleCode, long userId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@USER_ID", userId);
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
        public string SaveForecast(string postString, long CompanyId, long UserId)
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
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVEE_FORECAST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //var returnData = CommonClass.ReturnMessage(leadId);
                        //if (returnData != null)
                        //{
                        //    var returnobj = JsonConvert.DeserializeObject(Convert.ToString(leadId));
                        //    string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                        //    string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                        //    if (code == "1" && chkOperation == "INSERT")
                        //    {
                        //        var objCommon = new CommonClass(configuration);
                        //        objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_FORECAST_INSERT.ToString(), "CRM_FORECAST_INSERT_FULL");
                        //    }
                        //}

                       
                        //return returnData;
                        return CommonClass.ReturnMessage(leadId);
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
    }
}
