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
    public class DealService : IDealService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public DealService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string GetDeal(string Dealname, int? statusID, int? createdBy, string createdAt, string closingDate, long companyID, long loggedInUserId, int? sourceId, string subModuleCode, string sortBy, string sortExp, int pagesize, int pagenumber, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(createdAt) || createdAt.Length == 0)
                        createdAt = null;
                    if (string.IsNullOrWhiteSpace(closingDate) || closingDate.Length == 0)
                        closingDate = null;
                    sqlConnection.Open();
                    //CRMData dData = new CRMData();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyID);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@DEAL_NAME", Dealname);
                    dynamicParameters.Add("@STATUS_ID", statusID);
                    dynamicParameters.Add("@CREATED_BY", createdBy);
                    dynamicParameters.Add("@CREATED_AT", createdAt);
                    dynamicParameters.Add("@CLOSING_DATE", closingDate);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                   
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pagesize);
                    dynamicParameters.Add("@PAGENUMBER", pagenumber);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);



                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("[TALYGEN_SP_SERVICE_DEAL_GET_LISTING]", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetcustomFieldDeal(long companyId, long userID, string moduleName, string subModuleCode)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
       
        public string SaveDeal(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                Dictionary<string, dynamic> jsonData = new Dictionary<string, dynamic>();
                jsonData["data"] = tempJson.data;
                jsonData["clientData"] = tempJson.clientData;
               
                //if (tempJson.clientData != null)
                //{
                //    //var randSalt = CommonLibClass.FetchRandStr(3);
                //    dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(JsonConvert.DeserializeObject(Convert.ToString(tempJson.clientData))));
                //    if (postedData[0].user_id.Value <= 0)
                //    {

                //        var password = "";
                //        if (postedData[0].password != null && postedData[0].password != "")
                //        {
                //            password = postedData[0].password.Value;
                //            postedData[0].salt.Value = "abc";
                //            //postedData[0].password.Value = CommonLibClass.FetchMD5(CommonLibClass.FetchMD5(password) + randSalt);
                //            jsonData["clientData"] = postedData;
                //        }
                //    }
                //}              
                if (jsonData.Count() > 0)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", Convert.ToString(JsonConvert.SerializeObject(jsonData)));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_DEAL_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(leadId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(leadId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);
                         
                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_DEAL_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_DEAL_INSERT.ToString(), "CRM_DEAL_INSERT_FULL",title);
                            }
                        }

                        //return CommonClass.ReturnMessage(leadId);
                        return returnData;
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

        public string GetUnifiedDeal(long contactId, long companyId, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CONTACT_ID", contactId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_DEAL_DETAIL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string RevertAction(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            dynamic data = null;
            try
            {
                foreach (var attr in sys)
                {
                    data = Convert.ToString(attr.Value);
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
                        var ReturnedData = sqlConnection.Query<string>("TALYGEN_SP_RevertDealToLead", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(ReturnedData);

                        return returnData;
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

        public string PrimaryContactForDeal(long companyId, long? sourceid, string submodulecode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@SOURCE_ID", sourceid);
                    dynamicParameters.Add("@SUB_MODULE_CODE", submodulecode);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_PRIMARY_CONTACT_FOR_DEAL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
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