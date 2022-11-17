using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class LeadService : ILeadService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public LeadService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        public async Task AddLead(Lead lead)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add("@UserId", lead.lead_id);
                //dynamicParameters.Add("@UserName", lead.client_first_name+" "+lead.client_last_name);
                //dynamicParameters.Add("@Email",lead.client_email);
                //dynamicParameters.Add("@Channel", lead.channel_id);

                await sqlConnection.ExecuteAsync(
                    "spAddUser",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LeadId"></param>
        /// <returns></returns>
        public async Task DeleteLead(int LeadId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserId", LeadId);
                await sqlConnection.ExecuteAsync(
                    "spDeleteUser",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LeadId"></param>
        /// <returns></returns>
        public async Task<Lead> GetLead(int LeadId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserId", LeadId);
                return await sqlConnection.QuerySingleOrDefaultAsync<Lead>("spGetUser", dynamicParameters, commandType: CommandType.StoredProcedure);
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
        public string GetcustomFieldLead(long companyId, long userId, string moduleName, string subModuleCode)
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


        public string GetExportDetail(long companyId, string moduleName, string subModuleCode)
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
                    var customField = sqlConnection.Query<string>("TALYGEN_CRM_GET_CUSTOM_VIEWPORT_FOR_EXPORT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        ///  This method is used for fetching the available and selected custom fields for list layout
        ///  basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetListLayoutFields(long companyId, long userId, string moduleName, string subModuleCode)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@VIEWER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_AVAILABLE_SELECTED_FIELDS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// This method is used for fetching the list of leads 
        /// on the basis of parameters passed and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="companyId">Company Id parameter is passed for 
        /// fetching the records for a particular company.</param>
        /// <param name="viewerId">Viewer Id parameter is passed for 
        /// fetching the records for a particular user of the company.</param>
        /// <param name="clientName">Client Name parameter is passed for 
        /// fetching the records for a particular Client and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="statusIds">Status Ids parameter is passed for 
        /// fetching the records for the status selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="clientIds">Client Ids parameter is passed for 
        /// fetching the records for the clients selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="companyName">Company Name parameter is passed for 
        /// fetching the records for the company name passed and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="channelIds">channel Ids parameter is passed for 
        /// fetching the records for the status selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadOwnerIds">Lead Owner Ids parameter is passed for 
        /// fetching the records for the lead owners selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadDateFrom">Lead Date From parameter is passed for 
        /// fetching the records for the date from selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="leadDateTo">Lead Date to parameter is passed for 
        /// fetching the records for the date to selected and is for search
        /// related purpose. This is an optional parameter.</param>
        /// <param name="isPaged">is Paged parameter is passed for 
        /// fetching the records for a particular company</param>
        /// <param name="sortBy">Sort By parameter is passed for 
        /// sorting the records fetched by the value passed in the parameter.</param>
        /// <param name="sortExp">Sort By parameter is passed for 
        /// sorting the records fetched in the ascending or descending order 
        /// depending ton the parameter passed.</param>
        /// <param name="pageSize">Page Size parameter is passed for 
        /// fetching the number of records on a particular page. By default the
        /// records on a page are displayed on the basis of configuration setting</param>
        /// <param name="pageNum">Page Number parameter is passed for 
        /// mentioning the page number if it is 1,2 or so on./param>
        /// <param name="crmtype">CRM Type parameter is passed for 
        /// fetching the records of Lead(l), opportunity(o), propect(p)</param>
        /// <returns>returns Json string data</returns>
        public string GetLeads(long companyId, long viewerId, string clientName, string statusIds, string clientIds, string companyName, string channelIds, string leadOwnerIds, string leadDateFrom, string leadDateTo, byte isPaged, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNum, char crmtype, string searchCondition, bool isExport)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VIEWER_ID", viewerId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@CLIENT_NAME", clientName);
                    dynamicParameters.Add("@COMPANY_NAME", companyName);
                    dynamicParameters.Add("@CLIENT_ID", clientIds);
                    dynamicParameters.Add("@CHANNEL_IDS", channelIds);
                    dynamicParameters.Add("@LEAD_OWNER_ID", leadOwnerIds);
                    dynamicParameters.Add("@LEAD_DATE_FROM", leadDateFrom);
                    dynamicParameters.Add("@LEAD_DATE_TO", leadDateTo);
                    dynamicParameters.Add("@VALUE_FROM", null);
                    dynamicParameters.Add("@VALUE_TO", null);
                    dynamicParameters.Add("@STATUS_ID", statusIds);
                    dynamicParameters.Add("@IS_PAGED", isPaged);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);

                    dynamicParameters.Add("@CRM_TYPE", crmtype);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNum);
                    dynamicParameters.Add("@IS_EXPORT", isExport);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_LEAD_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return leadListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return ex.Message + " Connectionstring " + connectionString;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        public async Task UpdateLead(Lead lead)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add("@UserId",lead.lead_id);
                //dynamicParameters.Add("@UserName", lead.client_first_name+" "+ lead.client_last_name);
                //dynamicParameters.Add("@Email", lead.client_email);
                //dynamicParameters.Add("@Channel", lead.channel_id);
                await sqlConnection.ExecuteAsync(
                    "spUpdateUser",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// This method is used for performing the validation on 
        /// the data passed in leadData parameter and return appropriate 
        /// response in json format.
        /// </summary>
        /// <param name="leadData">Json string is passed in this parameter 
        ///  and further manipulation is performed for validating the data.
        ///  </param>
        public string ValidateData(string leadData)
        {
            //var parseTree = JsonConvert.DeserializeObject<JObject>(leadData);
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                //var parsed = JsonConvert.DeserializeObject<JObject>(leadData);
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
                dynamic sys = JsonConvert.DeserializeObject(leadData);
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


        public string SaveVdeskLead(string leadData)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", leadData);

                    var result = sqlConnection.Query<string>("TALYGEN_SP_AP_SAVE_LEAD_VDESK", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
             
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveLead(string postString, long CompanyId, long UserId)
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
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_LEAD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();


                        var returnData = CommonClass.ReturnMessage(leadId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(leadId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_LEAD_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_LEAD_INSERT.ToString(), "CRM_LEAD_INSERT_FULL", title);
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


        /// <summary>
        ///  Save list layout into TALYGEN_CUSTOM_FIELD_COMPANY_VIEWPORT table
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        //public string SaveListLayout(string postString)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    try
        //    {
        //        dynamic sys = JsonConvert.DeserializeObject(postString);
        //        int index = 1;
        //        dynamic data = null;

        //        foreach (var attr in sys)
        //        {
        //            if (index == 1)
        //            {
        //                data = attr.Value;
        //                break;
        //            }
        //        }
        //        if (data != null)
        //        {
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                string jsonS = "[" + Convert.ToString(data) + "]";
        //                dynamicParameters.Add("@JSON", jsonS);
        //                var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_LIST_LAYOUT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

        //                if (returnId != null)
        //                {
        //                    errorMessage.Add(new ListString()
        //                    {
        //                        Status = "Success"
        //                    });
        //                    return JsonConvert.SerializeObject(errorMessage);
        //                }
        //                else
        //                {
        //                    errorMessage.Add(new ListString()
        //                    {
        //                        Status = "Failure"
        //                    });
        //                    return JsonConvert.SerializeObject(errorMessage);
        //                }
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
        ///  Save Page layout into TALYGEN_CUSTOM_FIELD_mapping table
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        //public string SavePageLayout(string postString)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    try
        //    {
        //        dynamic sys = JsonConvert.DeserializeObject(postString);
        //        int index = 1;
        //        dynamic data = null;

        //        foreach (var attr in sys)
        //        {
        //            if (index == 1)
        //            {
        //                data = attr.Value;
        //                break;
        //            }
        //        }
        //        if (data != null)
        //        {
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                string jsonS = "[" + Convert.ToString(data) + "]";
        //                dynamicParameters.Add("@JSON", jsonS);
        //                var returnId = sqlConnection.Query<string>("TALYGEN_SERVICE_SAVE_FIELDS_AND_GROUP_PAGE_LAYOUT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

        //                if (returnId != null)
        //                {
        //                    errorMessage.Add(new ListString()
        //                    {
        //                        Status = "Success"
        //                    });
        //                    return JsonConvert.SerializeObject(errorMessage);
        //                }
        //                else
        //                {
        //                    errorMessage.Add(new ListString()
        //                    {
        //                        Status = "Failure"
        //                    });
        //                    return JsonConvert.SerializeObject(errorMessage);
        //                }
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

        public string convertLeadToAccount(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            List<long> contactIds = new List<long>();
            foreach (var item in postedData.contactIds)
            {
                contactIds.Add(Convert.ToInt64(item));
            }
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@LEAD_ID", postedData.sourceId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@ACCOUNT_NAME", postedData.accountName.Value);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@CONTACT_IDS", String.Join(",", contactIds));
                    dynamicParameters.Add("@ACCOUNT_ID", postedData.accountId.Value);
                    dynamicParameters.Add("@COMPANY", postedData.company.Value);
                    dynamicParameters.Add("@ADDRESS1", postedData.address1.Value);
                    dynamicParameters.Add("@ADDRESS2", postedData.address2.Value);
                    dynamicParameters.Add("@CITY", postedData.city.Value);
                    dynamicParameters.Add("@ZIP", postedData.zip.Value);
                    dynamicParameters.Add("@STATE", postedData.state.Value);

                    var rslt = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_CONVERT_LEAD_TO_ACCOUNT_V_10_4", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (rslt != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = rslt
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
        /// Close Lead
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string CloseLead(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();

            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                dynamic data = null;
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

                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CLOSE_LEAD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                        //return "true";
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success"
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
        public string ChangeOwner(string postString, long CompanyId, long UserId)
        {

            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            List<long> CheckedIds = new List<long>();
            foreach (var item in postedData.checkedIds)
            {
                CheckedIds.Add(Convert.ToInt64(item));
            }

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@OWNER_ID", postedData.newleadID.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@MODULE_ID", postedData.moduleName.Value);
                    dynamicParameters.Add("@SUB_MODULE_ID", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", UserId);
                    dynamicParameters.Add("@LEAD_IDS", String.Join(",", CheckedIds));


                    var rslt = sqlConnection.Query<string>("TALYGEN_SP_AP_CRM_OPR_CHANGE_LEAD_OWNER", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (rslt != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = rslt
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
        public string GetLeadImport(string batchid)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@batch_id", batchid);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_AP_CRM_LEAD_IMPORT_DETAILS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        public string SaveLeadImport(string postString, long CompanyId, long UserId)
        {
            string BatchId=null;
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
                        //if (index == 1)
                        //{
                        //    data = name.Value;
                        //    break;
                        //}
                        if (name.Name == "data")
                        {
                            data = Convert.ToString(name.Value);
                        }
                        if (name.Name == "CompanyId")
                        {
                            CompanyId = name.Value;
                        }
                        if (name.Name == "BatchId")
                        {
                            BatchId = name.Value;
                        }
                    }
                }
                if (data != null && CompanyId > 0 && BatchId != null ) 
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        string jsonS = Convert.ToString(data);
                        dynamicParameters.Add("@JSON", jsonS);
                        dynamicParameters.Add("@COMPANYID", CompanyId);
                        dynamicParameters.Add("@BATCHID", BatchId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_AP_CRM_SAVE_LEAD_IMPORT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();


                        return CommonClass.ReturnMessage(leadId);
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

        public string GetUnifiedLead(long contactId, long companyId, long userId)
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
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_LEAD_DETAIL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveUnifiedLead(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", Convert.ToString(tempJson.data));
                    dynamicParameters.Add("@CONTACT_ID_FOR_UNIF", tempJson.contactId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@CREATED_BY", UserId);
                    var LeadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_LEAD_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var returnData = CommonClass.ReturnMessage(LeadId);
                    if (returnData != null)
                    {
                        var returnobj = JsonConvert.DeserializeObject(Convert.ToString(LeadId));
                        string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                        string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                        if (code == "1" && chkOperation == "INSERT")
                        {
                            var objCommon = new CommonClass(configuration);
                            string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_LEAD_INSERT.ToString());

                            objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_LEAD_INSERT.ToString(), "CRM_LEAD_INSERT_FULL", title);
                        }
                    }
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

        public string ReopenLead(string postString, long companyId, long userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@LEAD_IDS", tempJson.ids.Value);
                        dynamicParameters.Add("@COMPANY_ID", companyId);
                        dynamicParameters.Add("@USER_ID", userId);
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_RE_OPEN_LEAD", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success",
                                Value = returnId
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

        public string GetAllLeadOwnerPermission(long companyId, long userId, string subModuleCode, long? leadOwner, long? leadId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", userId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@LEAD_OWNER", leadOwner);
                    dynamicParameters.Add("@LEAD_ID", leadId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_AP_CRM_GET_LIST_PERMISSIONS", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string saveAdditionalOwnerPermission(string postString, long companyId, long userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", Convert.ToString(tempJson));
                        dynamicParameters.Add("@COMPANY_ID", companyId);
                        dynamicParameters.Add("@USER_ID", userId);
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_OWNER_PERMISSIONS_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success"
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

        public string additionalLeadOwnerListing(long id, long companyId, long userId, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@LEAD_ID", id);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_AP_CRM_GET_ADDITIONAL_LEAD_OWNER_LIST", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string DeleteAttachment(long id, long sourceid)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@attachment_id", id);
                    dynamicParameters.Add("@source_id", sourceid);
                    var metadata = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_CRM_DELETE_ATTACHMENT", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return metadata;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string DeleteAccountBeforeDeal(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
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
                        dynamicParameters.Add("@ACCOUNT_ID", postedData.accntid.Value);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_CRM_DELETE_ACCOUNT_BEFORE_DEAL", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success"
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

        public string saveAdditionalDealOwnerPermission(string postString, long companyId, long userId)
        {
            List<ListString> errorMessage = new List<ListString>();
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", Convert.ToString(tempJson));
                        dynamicParameters.Add("@COMPANY_ID", companyId);
                        dynamicParameters.Add("@USER_ID", userId);
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_DEAL_OWNER_PERMISSIONS", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = "Success"
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
    //ChangeOwner
    public class CRMData
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> sqlMetaData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fieldMetadata { get; set; }


    }

    public class Lead
    {
    }


}
