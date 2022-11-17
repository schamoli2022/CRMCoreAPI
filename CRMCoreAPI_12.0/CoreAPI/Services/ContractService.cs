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
    public class ContractService : IContractService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public ContractService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contract_Name"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="userId"></param>
        /// <param name="clientIds"></param>
        /// <param name="ProductIds"></param>
        /// <param name="contractIds"></param>
        /// <param name="companyName"></param>
        /// <param name="contractName"></param>
        /// <param name="billToClientName"></param>
        /// <returns></returns>
        public string GetContracts(long companyId, string contract_Name, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long userId, string clientIds, string resellerIds, string ProductIds, string contractIds, string companyName, string contractName, string billToClientName, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (resellerIds == "")
                    {
                        resellerIds = null;
                    }
                    if (contractIds == "")
                    {
                        contractIds = null;
                    }
                    if (contract_Name == "")
                    {
                        contract_Name = null;
                    }
                    if (billToClientName == "")
                    {
                        billToClientName = null;
                    }
                    if (ProductIds == "")
                    {
                        ProductIds = null;
                    }
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CONTRACT_NAME", contract_Name);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@USERID", null);
                    dynamicParameters.Add("@CLIENTIDS", clientIds);
                    dynamicParameters.Add("@RESELLERIDS", resellerIds);
                    dynamicParameters.Add("@PRODUCTIDS", ProductIds);
                    dynamicParameters.Add("@CONTRACTID", contractIds);
                    dynamicParameters.Add("@COMPANYNAME", companyName);
                    dynamicParameters.Add("@CONTRACTNAME", contractName);
                    dynamicParameters.Add("@BILLTOCLIENTNAME", billToClientName);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CONTRACT_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetCustomFieldForContract(long companyId, long userId, string moduleName, string subModuleCode)
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
        public string SaveContract(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                Dictionary<string, dynamic> jsonData = new Dictionary<string, dynamic>();
                jsonData["data"] = tempJson.data;
                jsonData["clientData"] = tempJson.clientData;
                jsonData["ProjectDetail"] = tempJson.ProjectDetail;

                //if (tempJson.clientData != null)
                //{
                //    dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(JsonConvert.DeserializeObject(Convert.ToString(tempJson.clientData))));
                //    //var randSalt = CommonLibClass.FetchRandStr(3);

                //    if (postedData[0].user_id.Value <= 0)
                //    {
                //        var password = "";
                //        if (postedData[0].password != null && postedData[0].password != "")
                //        {
                //            password = postedData[0].password.Value;
                //            //string randSalt = configuration.GetSection("PasswordSalt").GetSection("Salt").Value;
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
                        var ContractId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CONTRACT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(ContractId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(ContractId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_CONTRACT_INSERT.ToString());
                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_CONTRACT_INSERT.ToString(), "CRM_CONTRACT_INSERT_FULL", title);
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
            catch (Exception ex)
            {
                SaveError(Convert.ToString(ex.InnerException),"Contract Save","API");
                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
            }
        }

        private void SaveError(string exception, string Url, string queryString)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@EXCEPTION", exception);
                    dynamicParameters.Add("@ERRORDATE", DateTime.UtcNow);
                    dynamicParameters.Add("@URL", Url);
                    dynamicParameters.Add("@QUERYSTRING", queryString);
                    dynamicParameters.Add("@IPADDRESS", queryString);
                    dynamicParameters.Add("@REFER", queryString);
                    dynamicParameters.Add("@EMAIL", queryString);
                    dynamicParameters.Add("@BROWSER", queryString);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_ERROR_INSERT", dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
        }


        /// <summary>
        /// This method is used for performing the validation on 
        /// the data passed in contractData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="contractData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string contractData)
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
                dynamic sys = JsonConvert.DeserializeObject(contractData);
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
