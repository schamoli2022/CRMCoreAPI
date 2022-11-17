using CRMCoreAPI.ServiceModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CRMCoreAPI.Common;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        public CampaignService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string AddGroupEmailTemplate(string addemailtemplate, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(addemailtemplate);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUES", postedData.selecteditems.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@MODULE_NAME", "CRM");
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.submodulecode.Value);
                    dynamicParameters.Add("@SUBJECT", postedData.emailSubject.Value);
                    dynamicParameters.Add("@PRIORITY", postedData.emailPriorityValue.Value);
                    dynamicParameters.Add("@TEMPLATE_ID", postedData.templatename.Value);
                    dynamicParameters.Add("@ISSINGLEMAIL", postedData.mailStatus.Value);
                    dynamicParameters.Add("@CC_RECIPIENT", postedData.emailCc.Value);
                    dynamicParameters.Add("@BCC_RECIPIENT", postedData.emailBcc.Value);
                    dynamicParameters.Add("@EMAIL_CONTENT", postedData.body.Value);
                    dynamicParameters.Add("@CAMPAIGN_ID", postedData.campaignId.Value);

                    var taskId = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_GROUP_EMAIL_TEMPLATE_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        /// <summary>
        /// Return Campaign list in JSON Data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="campaignName"></param>
        /// <param name="campaignOwnerIds"></param>
        /// <param name="campaignType"></param>
        /// date from column is used as proposed date from 
        /// <param name="campaignDateFrom"></param>
        /// date to column is used as propoed date to
        /// <param name="campaignDateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string GetCampaigns(long companyId, long loggedInUserId, string statusIDs, string campaignName, string campaignOwnerIds, string campaignType, string campaignDateFrom, string campaignDateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
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
                    dynamicParameters.Add("@CAMPAIGN_NAME", campaignName);
                    dynamicParameters.Add("@CAMPAIGN_OWNER", campaignOwnerIds);
                    dynamicParameters.Add("@CAMPAIGN_TYPE", campaignType);
                    dynamicParameters.Add("@DATE_FROM", campaignDateFrom);
                    dynamicParameters.Add("@DATE_TO", campaignDateTo);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var CampaignListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_CAMPAIGN_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetCustomFieldForCampaign(long companyId, long userId, string moduleName, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var CampaignListing = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_FIELDS_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveCampaign(string postString, long CompanyId, long UserId)
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
                        var CampaignId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CAMPAIGN_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(CampaignId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(CampaignId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_CAMPAIGN_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_CAMPAIGN_INSERT.ToString(), "CRM_CAMPAIGN_INSERT_FULL", title);
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

        public string SaveConvertToLeadCampaign(string campaignData, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(campaignData);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", "[" + Convert.ToString(sys.postString) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var LeadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CAMPAIGN_CONVERT_TO_LEAD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(LeadId);                    
                    return returnData;
                }

            }
            catch (Exception ex)            {

                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
           
        }

        public string SaveGroupContactCampaign(string groupData, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(groupData);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", "[" + Convert.ToString(JsonConvert.DeserializeObject(sys.postString.Value)) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var LeadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CONTACT_GROUP_NAME_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(LeadId);
                    return returnData;
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
        public string SaveEmailScheduler(string schedulerData, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(schedulerData);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", "[" + Convert.ToString(JsonConvert.DeserializeObject(sys.postString.Value)) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var Id = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_EMAIL_TEMPLATE_SCHEDULING_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(Id);
                    return returnData;
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


        /// <summary>
        /// This method is used for performing the validation on 
        /// the data passed in purchaseOrderData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="campaignData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string campaignData)
        {
            //var parseTree = JsonConvert.DeserializeObject<JObject>(purchaseOrderData);
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                //var parsed = JsonConvert.DeserializeObject<JObject>(purchaseOrderData);
                //foreach (var property in parsed.Properties())
                //{
                //    Console.WriteLine(property.Name);
                //    foreach (var innerProperty in ((JObject)property.Value).Properties())
                //    {
                //        Console.WriteLine("\t{0}: {1}", innerProperty.Name, innerProperty.Value.ToObject<object>());
                //    }
                //}                             
                dynamic data = null;
                int index = 1;
                dynamic dictValidation = null;
                List<string> validationData = new List<string>();
                dynamic sys = JsonConvert.DeserializeObject(campaignData);
                foreach (var attr in sys)
                {
                    foreach (var name in attr.Value)
                    {
                        if (index == 1)
                        {
                            data = name;

                        }
                        else
                            validationData.Add(JsonConvert.SerializeObject(name));

                    }
                    index++;
                    //foreach (var name in attr.Value)
                    //{
                    //    var dictdd = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(name));
                    //    foreach (var kv in dictdd)
                    //    {
                    //        Console.WriteLine(kv.Key + ":" + kv.Value);
                    //    }
                    //}
                }
                foreach (var item in validationData)
                {
                    dictValidation = JsonConvert.DeserializeObject<Dictionary<string, string>>(item);
                    foreach (var dval in dictValidation)
                    {
                        string fieldName = dictValidation["field_name"];
                        var val = data[fieldName];
                        switch (dval.Key)
                        {
                            case "required":
                                if (dval.Value == "true")
                                {
                                    if (val.Value.Length <= 0 && String.IsNullOrWhiteSpace(val.Value))
                                    {
                                        errorMessage.Add(new ListString()
                                        {
                                            Name = fieldName,
                                            Value = dval.Key,
                                            Status = "Failure"
                                        });
                                    }
                                }
                                break;
                            case "min":
                                if (val.Value.Length < Convert.ToInt16(dval.Value))
                                {
                                    errorMessage.Add(new ListString()
                                    {
                                        Name = fieldName,
                                        Value = dval.Key,
                                        Status = "Failure"
                                    });
                                }
                                break;
                            case "type":
                                bool isEmail = Regex.IsMatch(val.Value, RegexExp.Email, RegexOptions.IgnoreCase);
                                if (!(isEmail))
                                {
                                    errorMessage.Add(new ListString()
                                    {
                                        Name = fieldName,
                                        Value = "email",
                                        Status = "Failure"
                                    });
                                }
                                break;
                        }
                    }
                }
                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return JsonConvert.SerializeObject(errorMessage);
                }
                else
                {
                    errorMessage.Add(new ListString()
                    {
                        Status = "Success"
                    });
                    return JsonConvert.SerializeObject(errorMessage);
                }
            }
            catch (Exception ex)
            {
                errorMessage.Add(new ListString()
                {
                    Name = "Exception",
                    Value = ex.Message,
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
        }
    }
}
