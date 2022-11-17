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
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using CRMCoreAPI.Common;
using System.Web;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public CommonService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetDDLData(long? id, long companyId, long userId, string moduleName, string type, string search, long? refId)
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
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@TYPE", type);
                    dynamicParameters.Add("@SEARCH", search);
                    dynamicParameters.Add("@REF_ID", refId);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_DDL_BY_MODULE_AND_COMPANY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetDDLDataBySubModule(long? id, long companyId, long userId, string moduleName, string subModuleName, string type, string search, long? refId)
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
                    dynamicParameters.Add("@SUB_MODULE_NAME", subModuleName);
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@TYPE", type);
                    dynamicParameters.Add("@SEARCH", search);
                    dynamicParameters.Add("@REF_ID", refId);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_DDL_BY_MODULE_AND_SUBMODULE_COMPANY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }


        public string GetDDLCommunicationModeData(long? id, long companyId, long userId, string moduleName, string type, string search)
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
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@TYPE", type);
                    dynamicParameters.Add("@SEARCH", search);
                    string customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_DDL_BY_COMMUNICATION_MODE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }


        public string GetEmailById(string ids, long companyId, long userId)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@IDS", ids);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    string customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_EMAIL_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string AddEmailTemplate(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUES", postedData.slecteditems.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@MODULE_NAME", "CRM");
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.submodulecode.Value);
                    dynamicParameters.Add("@SUBJECT", postedData.emailSubject.Value);
                    dynamicParameters.Add("@PRIORITY", postedData.emailPriorityValue.Value);
                    dynamicParameters.Add("@TEMPLATE_ID", postedData.templatename.Value);
                    dynamicParameters.Add("@ISSINGLEMAIL", postedData.mailStatus.Value);
                    dynamicParameters.Add("@EMAILTO", postedData.to.Value);
                    dynamicParameters.Add("@CC_RECIPIENT", postedData.emailCc.Value);
                    dynamicParameters.Add("@BCC_RECIPIENT", postedData.emailBcc.Value);
                    dynamicParameters.Add("@EMAIL_CONTENT", postedData.body.Value);

                    var taskId = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_EMAIL_TEMPLATE_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: 100).FirstOrDefault();
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
        ///  Common Delete method to delete the data 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string CommonDeleteData(string postString, long CompanyId, long UserId)
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
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_COMMON_DELETE_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                        //return "true";
                        if (returnId != null)
                        {
                            errorMessage.Add(new ListString()
                            {
                                Status = returnId
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


        public string GetEmailData(long? id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@EMAIL_TEMPLATE_ID", id);
                    //dynamicParameters.Add("@COMPANY_ID", companyId);
                    //dynamicParameters.Add("@USER_ID", loggedInUserId);
                    //dynamicParameters.Add("@REF_ID", refId);
                    //dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    //dynamicParameters.Add("@MODULE_NAME", moduleName);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_EMAILTEMPLATE_DATA", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetEmailLogById(long? id, long companyId, long loggedInUserId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@LOG_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_EMAIL_LOG_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string SendEmailData(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();

            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
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
                        string customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_COMPANY_SMTP_CONFIGURATION_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        dynamic jsonData = JsonConvert.DeserializeObject(customField);
                        dynamic configData = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(jsonData.DATA));



                        dynamic modelData = JsonConvert.DeserializeObject(Convert.ToString(data));
                        if (configData != null)
                        {
                            var status = SendEmail(Convert.ToInt32(configData[0].SmtpPort), Convert.ToString(configData[0].SmtpServer), Convert.ToString(configData[0].FromEmail), Convert.ToString(modelData.from), Convert.ToString(modelData.to), Convert.ToString(configData[0].SmtpUserName), Convert.ToString(configData[0].SmtpPassword), Convert.ToString(modelData.Subject), Convert.ToString(modelData.body), Convert.ToBoolean(configData[0].SecureSmtp), Convert.ToString(modelData.importance), Convert.ToString(modelData.quoteCompanyLogo), Convert.ToString(modelData.emailCc), Convert.ToString(modelData.emailBcc), Convert.ToString(modelData.companyLogo), Convert.ToString(modelData.emailContent), Convert.ToString(modelData.forInvoice), CompanyId, UserId, Convert.ToInt64(modelData.refId));
                            if (status)
                            {
                                if (postedData.isSaveLog.Value == "")
                                {
                                    SaveEmailLog(postString);
                                }

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
        public bool SaveEmailLog(string postString)
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
                        var returnId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_EMAIL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                        //return "true";
                        if (returnId != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SendEmail(int port, string host, string fromEmail, string from, string to, string smtpUserName, string smtpPassword, string subject, string body, bool enalbleSSL, string importance, string quoteCompanyLogo = "", string cc = "", string bcc = "", string companyLogo = "", string emailContentdesc = "", string forInvoice = "", long? companyId = 0, long? userId = 0, long? invoiceId = 0)
        {
            try
            {
                if (bcc == "0")
                {
                    bcc = from;
                }
                if (quoteCompanyLogo == "quotationLogo")
                {
                    var Mc = Regex.Matches(body, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);

                    for (int icnt = 0; icnt < Mc.Count; icnt++)
                    {
                        body = body.Replace(Mc[icnt].ToString(), "<img onload='this.fitproportional=true;this.pv=0;this.ph=0;' height='101px' src='" + companyLogo + "'>");//test has the new string now
                    }
                }
                if (forInvoice == "yes")
                {
                    string enccompanyId = "";
                    string encuserId = "";
                    string encinvoiceId = "";
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(companyId)))
                    {
                        enccompanyId = HttpUtility.UrlEncode(CommonClass.Encrypt(Convert.ToString(companyId)));
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(userId)))
                    {
                        encuserId = HttpUtility.UrlEncode(CommonClass.Encrypt(Convert.ToString(userId)));
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(invoiceId)))
                    {
                        encinvoiceId = HttpUtility.UrlEncode(CommonClass.Encrypt(Convert.ToString(invoiceId)));
                    }
                    string externallnk = configuration.GetSection("AzureStorage").GetSection("ExternalLink").Value;
                    externallnk = externallnk + "WebSupport/ProvideFeedback?companyId=" + enccompanyId +
                        "&userId=" + encuserId + "&invoiceId=" + encinvoiceId;
                    string link = "\n\n\n\n <br/><br/> Please <a href='" + externallnk + "'> click here </a> to provide your valuable feedback.";
                    body = string.Concat(body, link);
                }
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.Host = host;
                client.EnableSsl = enalbleSSL;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);

                body = emailContentdesc + "\r\n" + body;
                MailMessage mm = new MailMessage(fromEmail, to, subject, body);
                mm.To.Add(to);
                if (!string.IsNullOrEmpty(cc))
                    mm.CC.Add(cc);

                if (!string.IsNullOrEmpty(bcc))
                    mm.Bcc.Add(bcc);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                if (importance == "high")
                {
                    mm.Priority = MailPriority.High;
                }
                else if (importance == "low")
                {
                    mm.Priority = MailPriority.Low;

                }
                else
                {
                    mm.Priority = MailPriority.Normal;
                }
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.IsBodyHtml = true;
                client.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetEmails(long? id, string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TABLE_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@REF_ID", refId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SEARCH", search);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    //dynamicParameters.Add("@USERID", null);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_EMAIL_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
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
        /// Get TimeLine data
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attachmentType"></param>
        /// <param name="sourceId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="fileName"></param>
        /// <param name="userId"></param>
        public string GetTimelineData(long id, long companyId, long userId, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCE_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_TIMELINE_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }

        }
        /// <returns></returns>
        public string SaveAttachment(string path, string attachmentType, long sourceId, string moduleName, string subModuleCode, string fileName, long userId, long companyId, string thumbNailPath = null)
        {
            List<ListString> errorMessage = new List<ListString>();
            string customField = "";
            try
            {


                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PATH", path);
                    dynamicParameters.Add("@ATTACHEMENT_MIME", attachmentType);
                    dynamicParameters.Add("@SOURCEID", sourceId);
                    dynamicParameters.Add("@MODULENAME", moduleName);
                    dynamicParameters.Add("@SUBMODULECODE", subModuleCode);
                    dynamicParameters.Add("@FILENAME", fileName);
                    dynamicParameters.Add("@USERID", userId);
                    dynamicParameters.Add("@COMPANYID", companyId);
                    dynamicParameters.Add("@THUMBNAIL_PATH", (string.IsNullOrEmpty(thumbNailPath) == false) ? thumbNailPath : null);
                    customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_CRM_ATTACHMENT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

                if (customField == "1")
                {

                    return "Exist";
                }

                errorMessage.Add(new ListString()
                {
                    Status = "Success"
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

        public string SaveTax(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedData));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    string returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_TAX_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return CommonClass.ReturnMessage(returnData);
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
        /// Get Search criteria list for a user with resptect to module and sub module
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetSearchCriteriaListing(long companyId, long userId, string moduleName, string subModuleCode)
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
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_SEARCH_CRITERIA_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }

        }
        /// <summary>
        /// Get field name on basis of module and submodule
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>

        public string GetFieldName(long companyId, long userId, string moduleName, string subModuleCode, string reqFrom = null)
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
                    dynamicParameters.Add("@REQ_FROM", reqFrom);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_DDL_FIELD_NAME_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }

        }
        /// <summary>
        /// Get operator list with respet to data type
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetOperatorList(long companyId, long userId, string dataType, long? customField)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@DATA_TYPE", dataType);
                    dynamicParameters.Add("@CUSTOM_FIELD", customField);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_DDL_OPERATOR_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }

        }
        /// <summary>
        /// Save search filter data with resptect to user id to search the data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveSearchFilter(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedData));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    string returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_SEARCH_FILTER_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return CommonClass.ReturnMessage(returnData);
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
        /// get the search criteria by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetSearchCriteriaById(long id, long companyId, long userId, string moduleName, string subModuleCode, int? isDelete)
        {
            //SaveError(isDelete > 0 ? "Value in isDelete Param" : "No value in IsDelete", "GetSearchCriteriaById", Convert.ToString(userId));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@IS_DELETE", isDelete);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_SEARCH_CRITERIA_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

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

        public string GetExistOrNot(long companyId, string name, string type, long id = 0, long? refid = null)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANYID", companyId);
                    dynamicParameters.Add("@VALUE", name);
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@TYPE", type);
                    dynamicParameters.Add("@REF_ID", refid);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_CHECK_DUPLICATE", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetEncryptedPassword(long companyId, string password)
        {
            try
            {
                string randSalt = configuration.GetSection("PasswordSalt").GetSection("Salt").Value;
                var returnData = CommonLibClass.FetchMD5(CommonLibClass.FetchMD5(password) + randSalt);
                return returnData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string GetUnifiedProjectManagementListing(string mobileNum, long? companyId, long? userId)
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
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_PROJECT_MANAGEMENT_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetUnifiedTicketingListing(string mobileNum, long? companyId, long? userId)
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
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_TICKETING_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetCallLogsListing(long id, long? companyId, long? loggedInUserId, string subModuleCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@REFID", id);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@SUBMODULECODE", subModuleCode);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_UNIFIED_GET_CALL_LOG_BY_REFID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetScheduleEmails(long? id, string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TABLE_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@REF_ID", refId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SEARCH", search);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    //dynamicParameters.Add("@USERID", null);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_EMAIL_SCHEDULE_LISTING_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }
            }
        }

        public string GetEmailTemplates(long? id, string search, long companyId, long loggedInUserId, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TABLE_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@REF_ID", refId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SEARCH", search);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    //dynamicParameters.Add("@USERID", null);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_GET_EMAIL_TEMPLATE_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }
            }
        }

        public string SaveIndustryType(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            dynamic data = null;
            dynamic data1 = null;
            //List<ListString> errorMessage = new List<ListString>();
            //dynamic postedString = JsonConvert.DeserializeObject(postString);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    foreach (var attr in sys)
                    {
                        var DataName = attr.Name;
                        if (DataName == "postString")
                        {
                            data = attr.Value;
                        }
                        else if (DataName == "type")
                            data1 = attr.First.Value;
                    }

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(data));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@TYPE", data1);
                    var resultdata = sqlConnection.Query<string>("TALYGEN_SP_SAVE_CRM_INDUSTRY_TYPE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SaveConfiguringJsonData(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            dynamic data = JsonConvert.DeserializeObject(Convert.ToString(postedData.mainarray));
            dynamic configtype = JsonConvert.DeserializeObject(Convert.ToString(postedData.configtype));
            dynamic previewnum = JsonConvert.DeserializeObject(Convert.ToString(postedData.previewnum));
            dynamic previewnumformat = JsonConvert.DeserializeObject(Convert.ToString(postedData.previewnumformat));
            dynamic serialnumber = JsonConvert.DeserializeObject(Convert.ToString(postedData.serialnumber));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    string jsonS = Convert.ToString(data);
                    dynamicParameters.Add("@JSON", jsonS);
                    dynamicParameters.Add("@CONFIG_TYPE", Convert.ToString(configtype.Configtype.Value));
                    dynamicParameters.Add("@PREVIEW_NUM", Convert.ToString(previewnum.PreviewNum.Value));
                    dynamicParameters.Add("@PREVIEW_FORMAT", Convert.ToString(previewnumformat.PreviewNumFormat.Value));
                    dynamicParameters.Add("@SERIAL_NUMBER", Convert.ToString(serialnumber.SerialNum.Value));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    string result = sqlConnection.Query<string>("TALYGEN_SP_SAVE_CONFIGURING_SERIAL_NUMBER_JSON_DATA", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;
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

        public string GetConfigNumberJsonData(long? id, long CompanyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CONFIG_ID", id);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_GET_CONFIG_SERIAL_NUMBER_DATA", dynamicParameters, commandType: CommandType.StoredProcedure);
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
        /// Save rule filter resptect to module and sub-module id to search the duplicate data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveRuleFilter(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedData));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    string returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_RULE_SEARCH_FILTER", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return CommonClass.ReturnMessage(returnData);
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
        /// Get the rule criteria by module name and sub module code
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetRuleCriteria(long companyId, long userId, string moduleName, string subModuleCode)
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
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_RULE_SEARCH_CRITERIA_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }

        }


        /// <summary>
        /// GGet the duplicate records by module name and sub module code
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        public string GetDuplicateLeads(long companyId, long userId, string moduleName, string subModuleCode, string ruleCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@VIEWER_ID", userId);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@RULE_CONDITION", ruleCondition);
                    var returnData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_CRM_GET_DUPLICATE_LEAD_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return returnData;
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