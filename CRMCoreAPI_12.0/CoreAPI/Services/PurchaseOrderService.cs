using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.Common;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    public class PurchaseOrderService:IPurchaseOrderService
    {
        private readonly string connectionString = null;
        /// <summary>
        private IConfiguration configuration;
        /// </summary>
        /// <param name="_configuration"></param>
        public PurchaseOrderService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="statusIDs"></param>
        /// <param name="accountName"></param>
        /// <param name="companyName"></param>
        /// <param name="leadOwners"></param>
        /// <param name="dealName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string GetPurchaseOrders(long companyId, long loggedInUserId, string statusIDs, string accountName, string companyName, string leadOwners, string dealName, string dateFrom, string dateTo, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
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
                    dynamicParameters.Add("@ACCOUNT_NAME", accountName);
                    dynamicParameters.Add("@COMPANY_NAME", companyName);
                    dynamicParameters.Add("@LEAD_OWNER", leadOwners);
                    dynamicParameters.Add("@DEAL_NAME", dealName);
                    dynamicParameters.Add("@DATE_FROM", dateFrom);
                    dynamicParameters.Add("@DATE_TO", dateTo);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var purchaseOrderListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_PURCHASE_ORDER_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return purchaseOrderListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        public string GetcustomFieldPurchaseOrders(long companyId, long userID, string moduleName, string subModuleCode)
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="postString"></param>
        ///// <returns></returns>
        //public string SavePurchaseOrder(string postString)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    dynamic sys = JsonConvert.DeserializeObject(postString);
        //    int index = 1;
        //    dynamic data = null;
        //    try
        //    {
        //        foreach (var attr in sys)
        //        {
        //            var newObj = Convert.ToString(attr.Value);

        //            dynamic sysss = JsonConvert.DeserializeObject(newObj);

        //            foreach (var name in sysss)
        //            {
        //                if (index == 1)
        //                {
        //                    data = name.Value;
        //                    break;
        //                }
        //            }
        //        }
        //        if (data != null)
        //        {
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                //string jsonS = "[" + Convert.ToString(data) + "]";
        //                dynamicParameters.Add("@json", Convert.ToString(data));
        //                var PurchaseOrderId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_PURCHASE_ORDERS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //                var returnData = CommonClass.ReturnMessage(PurchaseOrderId);
        //                if (returnData != null)
        //                {
        //                    var returnobj = JsonConvert.DeserializeObject(Convert.ToString(PurchaseOrderId));
        //                    string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
        //                    string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

        //                    if (code == "1" && chkOperation == "INSERT")
        //                    {
        //                        var objCommon = new CommonClass(configuration);
        //                        string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_PURCHASEORDER_INSERT.ToString());
        //                        objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_PURCHASEORDER_INSERT.ToString(), "CRM_PURCHASEORDER_INSERT_FULL", title);
        //                    }
        //                }


        //                return returnData;
        //            }
        //        }
        //        errorMessage.Add(new ListString()
        //        {
        //            Status = "Failure"
        //        });
        //        return JsonConvert.SerializeObject(errorMessage);
        //    }         
        //    catch (Exception ex)
        //    {
        //        errorMessage.Add(new ListString()
        //        {
        //            Status = "Failure"
        //        });
        //        return JsonConvert.SerializeObject(errorMessage);
        //    }
        //}


        /// <summary>
        /// This method is used for performing the validation on 
        /// the data passed in purchaseOrderData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="purchaseOrderData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string purchaseOrderData)
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
                dynamic sys = JsonConvert.DeserializeObject(purchaseOrderData);
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

        public string approveRejectPurchaseOrder(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUES", postedData.ids.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@MODULE_NAME", "CRM");
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@APPROVALTYPE", postedData.approvaltype.Value);
                   
                    var taskId = sqlConnection.Query<string>("TALYGEN_SERVICE_UPDATE_PURCHASEORDER_STATUS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
    }
}
