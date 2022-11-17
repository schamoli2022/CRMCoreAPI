
using CRMCoreAPI.Common;
using CRMCoreAPI.ServiceModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMCoreAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public AccountService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetAccounts(long UserID, long CompanyId, string ClientName, string EmailId, string UserPhone, int TotalProject, DateTime CreatedAt, string StatusName, int StatusId, string Avatar, string StatusCode, int LeadId, string crmtype, int parentId, int TotalChild)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    //CRMData dData = new CRMData();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", UserID);
                    dynamicParameters.Add("@CLIENT_NAME", ClientName);
                    dynamicParameters.Add("@EMAIL_ID", EmailId);
                    dynamicParameters.Add("@USER_PHONE", UserPhone);
                    dynamicParameters.Add("@TOTAL_PROJECT", TotalProject);
                    dynamicParameters.Add("@CREATED_AT", CreatedAt);
                    dynamicParameters.Add("@STATUS_NAME", StatusName);
                    dynamicParameters.Add("@STATUS_ID", StatusId);
                    dynamicParameters.Add("@AVATAR", Avatar);
                    dynamicParameters.Add("@STATUS_CODE", StatusCode);
                    dynamicParameters.Add("@LEAD_ID", LeadId);
                    dynamicParameters.Add("@CRM_TYPE", crmtype);
                    dynamicParameters.Add("@PARENT_ID", parentId);
                    dynamicParameters.Add("@TOTALCHILD", TotalChild);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CA_GET_CLIENTS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string GetAccountsType()
        {
            using (var sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    var result = sqlCon.Query<string>("exec TALYGEN_SP_GET_ACCOUNT_TYPE").FirstOrDefault();
                    return result;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string SaveAccountInfo(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            dynamic data = null;
            dynamic data1 = null;
            try
            {
                foreach (var attr in sys)
                {
                    var newObj = Convert.ToString(attr.Value);

                    dynamic sysss = JsonConvert.DeserializeObject(newObj);

                    foreach (var name in sysss)
                    {
                        
                            var DataName = name.Name;

                        //if (index == 1)
                        if (DataName == "data")
                        {
                            data = name.Value;
                            //break;
                        }
                        else if (DataName == "accountcontact")
                            data1 = name.Value;
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", Convert.ToString(data));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        dynamicParameters.Add("@ContactData", Convert.ToString(data1));
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_ACCOUNT_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_ACCOUNT_INSERT.ToString(), "CRM_ACCOUNT_INSERT_FULL", title);
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

        public string SaveAccountAddress(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedString.postString));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var resultdata = sqlConnection.Query<string>("TALYGEN_SP_SAVE_ACCOUNT_ADDRESS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return resultdata;
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

        public string AccountAddressListing(long CompanyId, long UserId, string moduleCode, int? sourceId, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@MODULE_NAME", moduleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var result = sqlConnection.Query<string>("TALYGEN_SP_GET_ACCOUNT_ADDRESS_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
