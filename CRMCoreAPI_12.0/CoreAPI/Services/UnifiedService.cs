using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using CRMCoreAPI.Common;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    public class UnifiedService : IUnifiedService
    {
        private string connectionString;
        private IConfiguration configuration;
        public UnifiedService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string checkForRunningCall(long userId, long companyId, long extension)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@EXTENSION", extension);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_CURRENT_CALL_LOG_BY_USERID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string DropDownForAddComment(long userId, long companyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_MODULE_SUBMODULE_FOR_DDL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string DropDownForSubmodule(long userId, long companyId, string submoduleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@SUBMODULECODE", submoduleCode);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_TABLE_DATA_BASIS_ON_SUBMODULE_FOR_DDL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetAsteriskConnectionData(long id, long companyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_ASTERISK_CONNECTION_LIST_BY_ID_AND_COMPANYID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetAsteriskList(long companyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_ASTERISK_LIST_COMPANYID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetDefaultCA(long companyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_DEFAULT_CA_BY_COMPANYID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetSetupTabData(long companyId, string tabName)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@TYPE", tabName);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_SETUP_TABS_LIST_BY_COMPANYID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveAsteriskConfiguration(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedNewData.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", postedData.existID.Value);
                    dynamicParameters.Add("@EMAIL_ID", postedData.CaEmail.Value);
                    dynamicParameters.Add("@DEVICE_NAME", postedData.DeviceName.Value);
                    dynamicParameters.Add("@LOCATION", postedData.Location.Value);
                    dynamicParameters.Add("@ZIPCODE", postedData.ZipCode.Value);
                    dynamicParameters.Add("@KEY_ID", postedData.KeyModel.Value);
                    dynamicParameters.Add("@LOCAL_SERVICE_URL", postedData.txtURL.Value);
                    dynamicParameters.Add("@LOCAL_SERVICE_PORT", postedData.txtPort.Value);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@MOBILEDATA", postedData.mobilenodata.Value);

                    //extension data with user ids.
                    dynamicParameters.Add("@EXTENSIONJSON", (postedData.extensionJson != null) ? postedData.extensionJson.Value : null);
                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_SAVE_ASTERISK_CONFIGURATION", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public string SaveCallIncommingOrOutgoing(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedData = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", postedData.userId.Value);
                    dynamicParameters.Add("@COMPANY_ID", postedData.companyId.Value);
                    dynamicParameters.Add("@EXTENSION", postedData.extension.Value);
                    dynamicParameters.Add("@CALL_FROM", postedData.callFrom.Value);
                    dynamicParameters.Add("@CALL_TO", postedData.callto.Value);
                    dynamicParameters.Add("@CALL_TYPE", postedData.callType.Value);
                    dynamicParameters.Add("@LOG_ID", postedData.logId.Value);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_SAVE_UPDATE_CALL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (sqlMetaData != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = sqlMetaData
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
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveCallMapping(string postString, long UserId, long CompanyId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedData = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@LOG_ID", postedData.logId.Value);
                    dynamicParameters.Add("@CALLER_NAME", postedData.callerName.Value);
                    dynamicParameters.Add("@COMMENT", (postedData.comment != null) ? postedData.comment.Value : null);
                    dynamicParameters.Add("@MAPPING_TYPE", (postedData.mappingType != null) ? postedData.mappingType.Value : null);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@REF_SOURCE_ID", (postedData.refSourceId != null) ? postedData.refSourceId.Value : null);
                    dynamicParameters.Add("@IS_CANCEL", (postedData.isCancel != null) ? postedData.isCancel.Value : 0);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_SAVE_CALL_MAPPING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public string SaveCallOriginateCall(string actionId, string UniqueId, string extension, string CallTo, string callerName, long? userId, long? companyId, string callingMedium)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@EXTENSION", (extension != "") ? extension : null);
                    dynamicParameters.Add("@CALLER_NAME", (callerName != "") ? callerName : null);
                    dynamicParameters.Add("@CALL_FROM", (extension != "") ? extension : null);
                    dynamicParameters.Add("@CALL_TO", (CallTo != "") ? CallTo : null);
                    dynamicParameters.Add("@CALL_TYPE", "Outgoing");
                    dynamicParameters.Add("@QUERY_TYPE", "INSERT");
                    dynamicParameters.Add("@RECENT_STATUS", "RINGING");
                    dynamicParameters.Add("@ACTION_ID", (actionId != "") ? actionId : null);
                    dynamicParameters.Add("@UNIQUE_ID", (UniqueId != "") ? UniqueId : null);
                    dynamicParameters.Add("@CALLING_MEDIUM", (callingMedium != "") ? callingMedium : null);
                    dynamicParameters.Add("@IS_FROM_APP", true);
                    dynamic mapID = sqlConnection.Query("TALYGEN_SP_UNIFIED_SAVE_UPDATE_CALL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    Dictionary<string, string> rtnData = new Dictionary<string, string>();
                    rtnData["logId"] = Convert.ToString(mapID.LOG_ID);
                    rtnData["userId"] = Convert.ToString(mapID.USER_ID);
                    rtnData["callerName"] = Convert.ToString(mapID.CALLER_NAME);
                    return JsonConvert.SerializeObject(rtnData);
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
        /// <summary>
        /// 
        /// </summary>
        /// exntension status like Idle, OnHold, Unavailable etc
        /// <param name="extensionStatus"></param>
        /// its logged in user id
        /// <param name="userId"></param>
        /// its logged in company id
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string getExtensionBasedOnStatus(string extensionStatus, long? userId, long? companyId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@EXTENSION_STATUS", extensionStatus);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);

                    string rsult = sqlConnection.Query<string>("TALYGEN_UNIFIED_GET_EXTENSION_BASED_ON_STATUS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    errorMessage.Add(new ListString()
                    {
                        Value = Convert.ToString(rsult),
                        Status = "success"
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

        public string getUserExtensionAndAuthKey(long companyId, long? userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);

                    string rsult = sqlConnection.Query<string>("TALYGEN_UNIFIED_GET_USER_EXTENSIONS_AND_AUTH_KEY", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    errorMessage.Add(new ListString()
                    {
                        Value = Convert.ToString(rsult),
                        Status = "success"
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

        public string getRecentcalls(long userId, long companyId, long? searchedCallLogId, long? pageSize, long? pageNumber, bool isLocalSearch, string searchKeyWord,string fromDate, string toDate)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@SEARCHED_CALL_LOG_ID", searchedCallLogId);
                    dynamicParameters.Add("@PAGE_NUMBER", pageNumber);
                    dynamicParameters.Add("@PAGE_SIZE", pageSize);
                    dynamicParameters.Add("@IS_LOCAL_SEARCH", isLocalSearch);
                    dynamicParameters.Add("@SEACH_KEYWORD", searchKeyWord);
                    dynamicParameters.Add("@FROM_DATE", fromDate);
                    dynamicParameters.Add("@TO_DATE", toDate);

                    string rsult = sqlConnection.Query<string>("TALYGEN_UNIFIED_GET_MOST_RECENT_CALLLS_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    errorMessage.Add(new ListString()
                    {
                        Value = Convert.ToString(rsult),
                        Status = "success"
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


        public string GetNexmoList(long userId, long companyId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);

                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@SEARCH", searchKeyWord);

                    string rsult = sqlConnection.Query<string>("TALYGEN_SP_COMPANY_NEXMO_LIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return rsult;
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
        public string SaveNexmoConfiguration(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedNewData.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", postedData.existID.Value);
                    dynamicParameters.Add("@TITLE", postedData.Title.Value);
                    dynamicParameters.Add("@API_KEY", postedData.apiKey.Value);
                    dynamicParameters.Add("@API_SECRET", postedData.apiSecret.Value);
                    dynamicParameters.Add("@APPLICATION_ID", postedData.applicationId.Value);
                    dynamicParameters.Add("@APPLICATION_KEY", postedData.applicationKey.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@DESCRIPTION", postedData.description.Value);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_SAVE_NEXMO_CONFIGURATION", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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
        public string NexmoStatusChange(long NexmoId, string type)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", NexmoId);
                    dynamicParameters.Add("@TYPE", type);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_UPDATE_NEXMO_STATUS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = mapID;
                    return returnData;
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
        public string GetNexmoRecord(long userId, long companyId, long? NexmoId, bool isForVoice)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", NexmoId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@IS_FOR_VOICE", isForVoice);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_COMPANY_NEXMO_LIST_SINGLE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = mapID;
                    return returnData;
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


        public string SaveNexmoPhoneNumber(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedNewData.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PHONE_NEXMO_ID", postedData.existID.Value);
                    dynamicParameters.Add("@USER_PHONE", postedData.PhoneNumber.Value);
                    dynamicParameters.Add("@ISSMS", postedData.Sms.Value);
                    dynamicParameters.Add("@SMSDEFAULT", postedData.SmsDefault.Value);
                    dynamicParameters.Add("@ISVOICE", postedData.Voice.Value);
                    dynamicParameters.Add("@VOICEDEFAULT", postedData.VoiceDefault.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@NEXMO_ID", postedData.NexmoId.Value);
                    dynamicParameters.Add("@USER_PHONE_ID", postedData.UserPhoneId.Value);
                    dynamicParameters.Add("@FORWRD_CALL_NUMBER", postedData.forwardCallNumber.Value);
                    dynamicParameters.Add("@COUNTRY", postedData.country.Value);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_SAVE_NEXMO_PHONE_NUMBER", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public string GetNexmoPhoneNumberList(long? NexmoId, long userId, long companyId, long? phoneNexmoId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    dynamicParameters.Add("@NEXMO_ID", NexmoId);
                    dynamicParameters.Add("@PHONE_NEXMO_ID", phoneNexmoId);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_GET_NEXMO_NUMBER_LIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = mapID;
                    return returnData;
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

        public List<DDLUser> GetNexmoUserList(long companyId, long userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            var contactList = new List<DDLUser>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANYID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", "");
                    dynamicParameters.Add("@SORT_EXP", "");
                    dynamicParameters.Add("@PAGESIZE", 100);
                    dynamicParameters.Add("@PAGENUMBER", 1);
                    dynamicParameters.Add("@SEARCH", "");
                    var mapID = sqlConnection.Query<DDLUser>("TALYGEN_SP_COMM_USER_CONTACT_ALL", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                    var data = mapID.GroupBy(d => d.USER_ID).ToList();
                    contactList = data.Select(x => new DDLUser
                    {
                        CONTACTNAME = x.FirstOrDefault().CONTACTNAME,
                        USER_ID = x.Key
                    }).ToList();
                    var returnData = contactList;
                    return returnData;
                }
                catch (Exception ex)
                {
                    errorMessage.Add(new ListString()
                    {
                        Status = "Failure"
                    });
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<string> GetCallDetails(long companyId, long userId, long logId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@LOG_ID", logId);
                    var customField = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_CURRENT_CALL_LOG_BY_USERID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        public async Task<string> NexmoCallHangup(string postString, long CompanyId, long UserId)
        {
            try
            {
                dynamic jsonObject = JObject.Parse(postString);

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@EXTENSION", (jsonObject.extension != null) ? jsonObject.extension.Value : null);
                    dynamicParameters.Add("@CALLER_NAME", (jsonObject.callerName != null) ? jsonObject.callerName.Value : null);
                    dynamicParameters.Add("@CALL_FROM", (jsonObject.extension != null) ? jsonObject.extension.Value : null);
                    dynamicParameters.Add("@CALL_TO", (jsonObject.CallTo != null) ? jsonObject.CallTo.Value : null);
                    dynamicParameters.Add("@CALL_TYPE", "Outgoing");
                    dynamicParameters.Add("@QUERY_TYPE", "UPDATE");
                    dynamicParameters.Add("@RECENT_STATUS", "HANGUP");
                    dynamicParameters.Add("@ACTION_ID", (jsonObject.actionId != null) ? jsonObject.actionId.Value : null);
                    dynamicParameters.Add("@UNIQUE_ID", (jsonObject.uniqueId != null) ? jsonObject.uniqueId.Value : null);
                    dynamicParameters.Add("@LOG_ID_FOR_LOCAL_HANGUP", (jsonObject.logId != null) ? jsonObject.logId.Value : null);
                    dynamicParameters.Add("@CALLING_MEDIUM", "NEX");
                    dynamicParameters.Add("@IS_FROM_APP", true);
                    dynamic mapID = sqlConnection.Query("TALYGEN_SP_UNIFIED_SAVE_UPDATE_CALL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    Dictionary<string, string> rtnData = new Dictionary<string, string>();
                    rtnData["logId"] = Convert.ToString(mapID.LOG_ID);
                    rtnData["userId"] = Convert.ToString(mapID.USER_ID);
                    rtnData["callerName"] = Convert.ToString(mapID.CALLER_NAME);
                    return JsonConvert.SerializeObject(rtnData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        public string UpdateCancelNumberNexmo(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedNewData.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PHONE_NEXMO_ID", postedData.phoneNexmoId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@NEXMO_ID", postedData.NexmoId.Value);

                    var mapID = sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_UPDATE_NEXMO_PHONE_STATUS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> SaveVonageConfiguration(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedNewData.ToString());
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_VONAGE_SAVE_CONFIGURATION_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public async Task<string> VonageList(long? VonageId, long companyId, long userId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord, bool isBasedOnUser = false)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VONAGE_ID", VonageId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGE_SIZE", pageSize);
                    dynamicParameters.Add("@PAGE_NUMBER", pageNumber);
                    dynamicParameters.Add("@IS_USER_BASED", isBasedOnUser);
                    dynamicParameters.Add("@SEARCH_KEYWORD", searchKeyWord);

                    var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_VONAGE_CONFIGURATION_LIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = rslt;
                    return returnData;
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

        public async Task<string> WebHookData(long? VonageId, long companyId, long userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VONAGE_ID", VonageId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);

                    var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_VONAGE_WEBHOOK", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = rslt;
                    return returnData;
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

        public async Task<string> GetVonageToken(long? CompanyId, long? UserId, long? VonageId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@VONAGE_ID", VonageId);

                    var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_TOKEN", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = rslt;
                    return returnData;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> SaveVonageWebHook(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedNewData.ToString());
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_VONAGE_SAVE_WEBHOOK_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(rslt);
                    return returnData;
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

        public async Task<string> VonageStatusUpdate(long? VonageId, long CompanyId, long UserId, string StatusForUpdate)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    Dictionary<string, dynamic> updateStatusData = new Dictionary<string, dynamic>();
                    updateStatusData["VonageId"] = VonageId;
                    updateStatusData["UserId"] = UserId;
                    updateStatusData["CompanyId"] = CompanyId;
                    updateStatusData["QueryType"] = "UPDSTS";
                    updateStatusData["StatusForUpdate"] = StatusForUpdate;

                    string postedNewData = JsonConvert.SerializeObject(updateStatusData);
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedNewData);

                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_VONAGE_SAVE_CONFIGURATION", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        //public async Task<string> GetSingleCallDetail(long? CompanyId, long? UserId, long? CallLogId)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@COMPANY_ID", CompanyId);
        //            dynamicParameters.Add("@USER_ID", UserId);
        //            dynamicParameters.Add("@LOG_ID", UserId);

        //            var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_CURRENT_CALL_LOG_BY_USERID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
        //            var returnData = rslt;
        //            return returnData;
        //        }
        //        catch (Exception ex)
        //        {
        //            errorMessage.Add(new ListString()
        //            {
        //                Status = "Failure"
        //            });
        //            return JsonConvert.SerializeObject(errorMessage);
        //        }
        //    }
        //}
        public async Task<string> SaveVonageUsers(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedNewData.json.ToString());
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@VONAGE_ID", postedNewData.VonageId.ToString());

                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_VONAGE_SAVE_USERS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public async Task<string> UpdateVonageCallHoldStatus(long LogId, long UserId, long CompanyId, string Status)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@QUERY_TYPE", "UPDATE_HOLD_STATUS");
                    dynamicParameters.Add("@RECENT_STATUS", Status);
                    dynamicParameters.Add("@LOG_ID_FOR_LOCAL_HANGUP", LogId);
                    dynamicParameters.Add("@CALLING_MEDIUM", "VON");
                    dynamicParameters.Add("@IS_FROM_APP", true);
                    dynamic mapID = sqlConnection.Query("TALYGEN_SP_UNIFIED_SAVE_UPDATE_CALL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    Dictionary<string, string> rtnData = new Dictionary<string, string>();
                    rtnData["logId"] = Convert.ToString(mapID.LOG_ID);
                    rtnData["userId"] = Convert.ToString(mapID.USER_ID);
                    rtnData["callerName"] = Convert.ToString(mapID.CALLER_NAME);
                    rtnData["status"] = Convert.ToString(mapID.STATUS);
                    return JsonConvert.SerializeObject(rtnData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public async Task<string> UpdateVonageUserId(long UserId, long CompanyId, long VonageFirstUserId, long VonageUserSecondId)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VONAGE_FIRST_ID", VonageFirstUserId);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@VONAGE_SECOND_ID", VonageUserSecondId);

                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_VONAGE_UPDATE_SECOND_VONAGE_USER_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        public async Task<string> TokBoxList(long? TokboxId, long companyId, long userId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TOKBOX_ID", TokboxId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGE_SIZE", pageSize);
                    dynamicParameters.Add("@PAGE_NUMBER", pageNumber);
                    dynamicParameters.Add("@SEARCH_KEYWORD", searchKeyWord);

                    var rslt = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_GET_TOKBOX_CONFIGURATION_LIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = rslt;
                    return returnData;
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

        public async Task<string> TokBoxStatusUpdate(long? TokBoxId, long CompanyId, long UserId, string StatusForUpdate)
        {
            List<ListString> errorMessage = new List<ListString>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    Dictionary<string, dynamic> updateStatusData = new Dictionary<string, dynamic>();
                    updateStatusData["TokBoxId"] = TokBoxId;
                    updateStatusData["UserId"] = UserId;
                    updateStatusData["CompanyId"] = CompanyId;
                    updateStatusData["QueryType"] = "UPDSTS";
                    updateStatusData["StatusForUpdate"] = StatusForUpdate;

                    string postedNewData = JsonConvert.SerializeObject(updateStatusData);
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedNewData);

                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_TOKBOX_SAVE_CONFIGURATION", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> SaveTokBoxConfiguration(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedNewData = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedNewData.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", postedData.ToString());
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var mapID = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_UNIFIED_TOKBOX_SAVE_CONFIGURATION_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(mapID);
                    return returnData;
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
    public class DDLUser
    {
        public string CONTACTNAME { get; set; }
        public long USER_ID { get; set; }
    }
}
