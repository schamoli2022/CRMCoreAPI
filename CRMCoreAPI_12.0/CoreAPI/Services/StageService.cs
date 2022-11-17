using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using CRMCoreAPI.ServiceInterface;
using System.Text.RegularExpressions;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json;
using CRMCoreAPI.Common;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    public class StageService : IStageService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public StageService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// GET STAGE LISTING
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetStages(long companyId, long loggedUserId, string stagename, string sortBy, string sortExp, int pageSize, int pageNumber, long totalRecords, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if(stagename=="")
                    {
                        stagename = null;
                    }
                    sqlConnection.Open();
                    //CRMData dData = new CRMData();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedUserId);
                    dynamicParameters.Add("@STAGE_NAME", stagename);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", "");
                    dynamicParameters.Add("@TOTAL_RECORDS", totalRecords);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICES_CA_GET_STAGE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// This method is used for fetch the custom fields to bind with ADD FORM 
        /// On the basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string</returns>
        public string GetcustomFieldStage(long companyId, long userId, string moduleName, string subModuleCode)
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
        public string SaveStage(string postString, long CompanyId, long UserId)
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
                        var StageId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_STAGES_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(StageId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(StageId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_STAGE_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_STAGE_INSERT.ToString(), "CRM_STAGE_INSERT_FULL",title);
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


        /// <summary>
        /// This method is used for performing the validation on 
        /// the data passed in stageData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="stageData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string stageData)
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
                dynamic sys = JsonConvert.DeserializeObject(stageData);
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
        /// <summary>
        /// this function used to update stage for deal and also done insertion in stage mapping table
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string updateDealStage(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@SOURCE_ID", tempJson.refId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@MODULE_NAME", tempJson.moduleName.Value);
                    dynamicParameters.Add("@SUB_MODULE_CODE", tempJson.subModuleCode.Value);
                    dynamicParameters.Add("@STAGE_ID", tempJson.stage.Value);
                    var PriceBookId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_UPDATE_DEAL_STAGE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return CommonClass.ReturnMessage(PriceBookId);
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

        public string GetMappingStages(int sourceId, long companyId, long loggedUserId, string moduleName, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {                    
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedUserId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICEL_GET_STAGE_MAPPED_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

