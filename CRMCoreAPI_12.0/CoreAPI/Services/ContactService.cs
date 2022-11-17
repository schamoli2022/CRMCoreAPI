using CRMCoreAPI.ServiceInterface;
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
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.IO;

namespace CRMCoreAPI.Services
{
  
    public class ContactService: IContactService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;


        public ContactService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        public string GetContacts( string First_Name, string Last_Name, string job_title, string email_id, string business_phone, string mobile_phone, string fax, string preferred_contact,  long company_id, long loggedInUserId, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, int? statusId, string requestType,string searchCondition, string AccountType,int? clientId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
               try
                { 
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@FIRST_NAME", First_Name);
                    dynamicParameters.Add("@LAST_NAME", Last_Name);
                    dynamicParameters.Add("@JOB_TITLE", job_title);
                    dynamicParameters.Add("@EMAIL_ID", email_id);
                    dynamicParameters.Add("@BUSINESS_PHONE", business_phone);
                    dynamicParameters.Add("@MOBILE_PHONE", mobile_phone);
                    dynamicParameters.Add("@FAX", fax);
                    dynamicParameters.Add("@PREFERRED_CONTACT", preferred_contact);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@STATUS_ID", statusId);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@COMPANY_ID", company_id);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@REQUESTTYPE", requestType);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    dynamicParameters.Add("@AccountType", AccountType);
                    dynamicParameters.Add("@ClientID", clientId);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_CONTACTS_LISTING_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        public string GetcustomFieldContact(long company_id, string moduleName, string subModuleCode, long userId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", company_id);
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

        public string GetGroupContacts(int? sourceId, long company_id, string subModuleCode, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@COMPANY_ID", company_id);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    //dynamicParameters.Add("@USERID", null);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_CONTACTS_GROUP_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string getSingleContact(long id, long companyId, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CONTACT_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_CONTACT_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveContact(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                Dictionary<string, dynamic> jsonData = new Dictionary<string, dynamic>();
                jsonData["data"] = tempJson.data;
                
                if (jsonData.Count() > 0)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        //string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@json", Convert.ToString(JsonConvert.SerializeObject(jsonData)));
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        var contactId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CONTACT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        var returnData = CommonClass.ReturnMessage(contactId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(contactId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_CONTACT_INSERT.ToString());

                                objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_CONTACT_INSERT.ToString(), "CRM_CONTACT_INSERT_FULL", title);
                            }
                            try

                            {
                                if (tempJson.fileObj != null && tempJson.fileObj.Value != "") {
                                    var tempAttachment = JsonConvert.DeserializeObject(Convert.ToString(tempJson.fileObj));
                                    string fileName = "";
                                    string mimeType = "";
                                    long id = 0; // Id Of Contact
                                    string base64String = "";
                                    string moduleName = "CRM";
                                    string subModuleCode = "CRM_CONTACTS";
                                    string contentType = "";
                                    string containerName = "";

                                    fileName = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["fileName"]);
                                    mimeType = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["attachement_mime"]);
                                    var jobjectMimieType = JsonConvert.DeserializeObject(mimeType);
                                    contentType = Convert.ToString(JObject.Parse(Convert.ToString(jobjectMimieType))["ContentType"]);
                                    id = Convert.ToInt64(JObject.Parse(Convert.ToString(tempAttachment))["id"]);
                                    base64String = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["base64String"]);
                                    moduleName = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["moduleName"]);
                                    subModuleCode = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["subModuleCode"]);
                                    containerName = Convert.ToString(JObject.Parse(Convert.ToString(tempAttachment))["containerName"]);
                                    string accountName = configuration.GetSection("AzureStorage").GetSection("AccountName").Value;
                                    string accountKey = configuration.GetSection("AzureStorage").GetSection("AccountKey").Value;

                                    var storageCredentials = new StorageCredentials(accountName, accountKey);
                                    var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                                    var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                                    var container = cloudBlobClient.GetContainerReference(containerName);
                                    container.CreateIfNotExistsAsync();

                                    byte[] imageBytes = Convert.FromBase64String(base64String);

                                    //upload a file
                                    string blobPath = "files/" + moduleName + "/" + subModuleCode + "/" + UserId + "/" + Convert.ToString(JObject.Parse(Convert.ToString(contactId))["MAIN_ID"]) + "/" + Guid.NewGuid().ToString() + "" + Path.GetExtension(fileName);
                                    var newBlob = container.GetBlockBlobReference(blobPath);
                                    var path = newBlob.StorageUri;
                                    newBlob.Properties.ContentType = contentType;
                                    //await newBlob.SetPropertiesAsync();
                                    //await newBlob.UploadFromFileAsync(@"path\myfile.png");
                                    newBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

                                    // download a file

                                    //var newBlobs = container.GetBlockBlobReference("cg-husi-190559");
                                    // await newBlobs.DownloadToFileAsync("path/myfile.png", FileMode.Create);
                                    fileName = Path.GetFileNameWithoutExtension(fileName);
                                    var dynamicParametersFile = new DynamicParameters();
                                    dynamicParametersFile.Add("@CONTACT_ID", Convert.ToString(JObject.Parse(Convert.ToString(contactId))["MAIN_ID"]));
                                    dynamicParametersFile.Add("@BCARD_URL", blobPath);
                                    sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_CRM_CONTACT_BCARD", dynamicParametersFile, commandType: CommandType.StoredProcedure).FirstOrDefault();


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

        public string SaveContactWithMapping(string postString, long CompanyId, long UserId)
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
                    var noteId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CONTACT_MAPPING_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    //return "true";
                    //if (noteId != null)
                    //{
                    //    errorMessage.Add(new ListString()
                    //    {
                    //        Status = "Success"
                    //    });
                    //    return JsonConvert.SerializeObject(errorMessage);
                    //}
                    //else
                    //{
                    //    errorMessage.Add(new ListString()
                    //    {
                    //        Status = "Failure"
                    //    });
                    //    return JsonConvert.SerializeObject(errorMessage);
                    //}
                    //////////////////////START NOTIFICTATION //////////////////////////////
                    var returnData = CommonClass.ReturnMessage(noteId);
                    if (returnData != null)
                    {
                        var returnobj = JsonConvert.DeserializeObject(Convert.ToString(noteId));
                        string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                        string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                        if (code == "1" && chkOperation == "INSERT")
                        {
                            var objCommon = new CommonClass(configuration);
                            string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_CONTACT_INSERT.ToString());

                            objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.CRM_CONTACT_INSERT.ToString(), "CRM_CONTACT_INSERT_FULL", title);
                        }
                    }

                    //return CommonClass.ReturnMessage(leadId);
             //       return returnData;

                    ////////////////////////END NOTIFICATION//////////////////////////////////


                    return CommonClass.ReturnMessage(noteId);
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

        public string SaveExistingContactWithMapping(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedString.postString));
                    dynamicParameters.Add("@ONLYMAPING", 1);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var noteId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_CONTACT_MAPPING_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return CommonClass.ReturnMessage(noteId);
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


        public string SaveContactImport(string postString, long CompanyId, long UserId,string SubModule)
        {
            string BatchId = null;
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
                        if (name.Name == "data")
                        {
                            data = Convert.ToString(name.Value);
                        }
                        if (name.Name == "BatchId")
                        {
                            BatchId = name.Value;
                        }
                    }
                }

                if (data != null && CompanyId > 0 && BatchId != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        string jsonStr = Convert.ToString(data);
                        dynamicParameters.Add("@JSONSTR", jsonStr);
                        dynamicParameters.Add("@COMPANYID", CompanyId);
                        dynamicParameters.Add("@BATCHID", BatchId);
                        string ProcedureName = "TALYGEN_SP_AP_CRM_SAVE_CONTACT_IMPORT";
                        if (SubModule == "CRM_ACCOUNTS")
                            ProcedureName = "TALYGEN_SP_AP_CRM_SAVE_ACCOUNT_IMPORT";
                        var ContactId = sqlConnection.Query<string>(ProcedureName, dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        return CommonClass.ReturnMessage(ContactId);
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

        public string getUnifiedContact(string mobileNum, long? companyId, long? userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@MOBILE_NUMBER", mobileNum);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", userId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_CONTACT_DETAIL", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveUnifiedContact(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@json", "[" + Convert.ToString(sys.postString) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    var LeadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_UNIFIED_SAVE_CONTACT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SaveClientInfo(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            try
            {
                if (sys.postString != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", "[" + Convert.ToString(sys.postString) + "]");
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        var Userid = sqlConnection.Query<string>("TALYGEN_SP_AP_SAVE_CLIENT", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        return Userid;
                    }
                }
                else
                    return "";
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

        public string DeleteClientContact(string postString, long CompanyId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", "[" + Convert.ToString(sys.postString) + "]");
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    var sqldata = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_AP_DELETE_CLIENT_USER", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqldata;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        public string UserRole(long CompanyId,long userId,int usertype)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@usertype", usertype);
                    var sqldata = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_ROLE_LIST", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqldata;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SaveContactRoleTitle(string postString, long CompanyId, long UserId)
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
                    var resultdata = sqlConnection.Query<string>("TALYGEN_SP_SAVE_CONTACT_ROLE_TITLE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SaveContactAsPrimary(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            try
            {
                if (sys.postString != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@JSON", "[" + Convert.ToString(sys.postString) + "]");
                        dynamicParameters.Add("@CREATED_BY", UserId);
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        var Userid = sqlConnection.Query<string>("TALYGEN_SP_AP_SAVE_CONTACT_AS_PRIMARY", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        return Userid;
                    }
                }
                else
                    return "";
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
