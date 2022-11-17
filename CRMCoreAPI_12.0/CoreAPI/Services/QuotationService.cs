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
    public class QuotationService : IQuotationService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public QuotationService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetQuotes(long companyId, long loggedInUserId, string startDate, string endDate, string title, string quotationNumber, int? sourceId, string subModuleCode, string sortBy, string sortExp, int pageSize, int pageNumber, string statuses, string quotationTypeCode, long? dealId, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@START_DATE", startDate);
                    dynamicParameters.Add("@END_DATE", endDate);
                    dynamicParameters.Add("@TITLE", title);
                    dynamicParameters.Add("@NUMBER", quotationNumber);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@STATUSES", statuses);
                    dynamicParameters.Add("@QUOTATION_TYPE_CODE", quotationTypeCode);
                    dynamicParameters.Add("@DEAL_ID", dealId);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_QUOTATION_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        public string GetcustomFieldQuotation(long companyId, string moduleName, string subModuleCode, long UserId)
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
                    dynamicParameters.Add("@USER_ID", UserId);
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
        public string SaveQuotation(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);

            try
            {
                dynamic postedString = JsonConvert.DeserializeObject(postString);
                dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
                dynamic data = JsonConvert.DeserializeObject(Convert.ToString(postedData.data));
                dynamic address = JsonConvert.DeserializeObject(Convert.ToString(postedData.address));
                dynamic productSummary = JsonConvert.DeserializeObject(Convert.ToString(postedData.productSummary));
                dynamic status = JsonConvert.DeserializeObject(Convert.ToString(postedData.status));
                dynamic subModuleCode = JsonConvert.DeserializeObject(Convert.ToString(postedData.subModuleCode));
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", Convert.ToString(data));
                        dynamicParameters.Add("@ADDRESS", Convert.ToString(address));
                        dynamicParameters.Add("@PRODUCTSUMMARY", Convert.ToString(productSummary));
                        dynamicParameters.Add("@STATUS", Convert.ToString(status.Status.Value));
                        dynamicParameters.Add("@SUB_MODULE_CODE", Convert.ToString(subModuleCode.SubModuleCode.Value));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var quotationId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_ORDERS_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        var returnData = CommonClass.ReturnMessage(quotationId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(quotationId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                if ((subModuleCode.SubModuleCode.Value) == "CRM_SALES_ORDERS")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_SALESORDER_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_SALESORDER_INSERT.ToString(), "CRM_SALESORDER_INSERT_FULL", title);
                                }
                                else if ((subModuleCode.SubModuleCode.Value) == "CRM_PURCHASE_ORDERS")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_PURCHASEORDER_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_PURCHASEORDER_INSERT.ToString(), "CRM_PURCHASEORDER_INSERT_FULL", title);
                                }
                                else if ((subModuleCode.SubModuleCode.Value) == "CRM_QUOTES")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_QUOTATION_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_QUOTATION_INSERT.ToString(), "CRM_QUOTATION_INSERT_FULL", title);
                                }
                                
                                else if ((subModuleCode.SubModuleCode.Value) == "CRM_INVOICE")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_INVOICE_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_INVOICE_INSERT.ToString(), "CRM_INVOICE_INSERT_FULL", title);
                                }

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
        /// Convert order item to other
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string ConvertOrder(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);

            try
            {
                dynamic postedString = JsonConvert.DeserializeObject(postString);
                dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));

                if (postedData != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", Convert.ToString(postedData));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var returnStatus = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_CONVERT_ORDERS_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(returnStatus);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(returnStatus));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                if ((postedData.ToSubModuleCode.Value) == "CRM_SALES_ORDERS")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_SALESORDER_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_SALESORDER_INSERT.ToString(), "CRM_SALESORDER_INSERT_FULL", title);
                                }
                                //else if ((postedData.SubModuleCode.Value) == "CRM_QUOTES")
                                //{
                                //    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_QUOTATION_INSERT.ToString());

                                //    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_QUOTATION_INSERT.ToString(), "CRM_QUOTATION_INSERT_FULL", title);
                                //}
                                else if ((postedData.ToSubModuleCode.Value) == "CRM_INVOICE")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_INVOICE_INSERT.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_INVOICE_INSERT.ToString(), "CRM_INVOICE_INSERT_FULL", title);
                                }
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
        /// the data passed in quoatationData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="quotationData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string quotationData)
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
                dynamic sys = JsonConvert.DeserializeObject(quotationData);
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

        public string QuotationDetails(long? id, long? companyId, long? loggedInUserId, string moduleCode, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUE", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@MODULE_NAME", moduleCode);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SERVICE_GET_QUOTATION_FIELDS_AND_VALUE_BY_MODULE_SUB_MODULE_ID_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string FeedBackDetails(long? companyId, long? loggedInUserId, long? invoiceId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@INVOICEID", invoiceId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_AP_CRM_FEEDBACK_GET_QUESTIONANSWERS", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetUnifiedOrderListing(long contactId, string type_code, long? companyId, long? userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CONTACT_ID", contactId);
                    dynamicParameters.Add("@TYPE_CODE", type_code);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_ORDER_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string QuoteToProject(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();

            try
            {
                //SaveError("convert quote type", "quotoproject", "TALYGEN_SP_SERVICE_CRM_CONVERT_QUOTE_TO_PROJECT");
                dynamic postedString = JsonConvert.DeserializeObject(postString);
                dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));

                if (postedData != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@OWNER_ID", UserId);
                        dynamicParameters.Add("@ORDER_ID", postedData.orderId.Value);
                        dynamicParameters.Add("@PROJECTID", postedData.projectTmplateId.Value);
                        dynamicParameters.Add("@PROJECT_NAME", postedData.projectName.Value);
                        var returnStatus = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_CONVERT_QUOTE_TO_PROJECT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //SaveError("convert quote type", "done", returnStatus);
                        return CommonClass.ReturnMessage(returnStatus);
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

        public string GetQuoteProductbyDeal(long companyId, long userId, long? sourceid)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SOURCE_ID", sourceid);
                    var result = sqlConnection.Query<string>("TALYGEN_SP_AP_GET_QUOTE_PRODUCTLIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string GetAddressdata(long companyId, long userId, long? addressid, long? sourceid)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@ACCOUNT_ID", sourceid);
                    dynamicParameters.Add("@ADDRESS_ID", addressid);
                    var result = sqlConnection.Query<string>("TALYGEN_SP_AP_GET_QUOTE_ADDRESS_BY_ACCOUNT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
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
