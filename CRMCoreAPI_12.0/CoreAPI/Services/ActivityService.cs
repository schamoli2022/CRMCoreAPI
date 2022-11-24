using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Common;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    public class ActivityService : IActivityService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        public ActivityService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
       
        public string getActivities(long CompanyId, long UserId, string moduleCode, int? sourceId, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", UserId);
                    dynamicParameters.Add("@MODULE_NAME", moduleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId); 
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    var ActivityListing = sqlConnection.Query<string>("TALYGEN_CRM_SERVICE_ACTIVITIES_LIST", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return ActivityListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
            } 
        public string updateActivity(long? id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ACTIVITY_ID", id);
                    //dynamicParameters.Add("@COMPANY_ID", companyId);
                    //dynamicParameters.Add("@USER_ID", loggedInUserId);
                    //dynamicParameters.Add("@REF_ID", refId);
                    //dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    //dynamicParameters.Add("@MODULE_NAME", moduleName);

                    var sqlMetaData = sqlConnection.QueryFirstOrDefault<string>("TALYGEN_SP_SERVICE_UPDATE_ACTIVITY_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }


        public string Addtask(string postString, long CompanyId, long UserId)  
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@SOURCE_ID", postedData.sourceId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@MODULE_NAME", postedData.moduleName.Value);
                    dynamicParameters.Add("@SUB_MODULE_NAME", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@SUBJECT", postedData.subjecttext.Value);
                    dynamicParameters.Add("@DUE_DATE", postedData.dueDate.Value);
                    dynamicParameters.Add("@PRIORITY", postedData.priorityLavel.Value);
                    dynamicParameters.Add("@MESSAGE", postedData.messagetext.Value); //activityId
                    dynamicParameters.Add("@ACTIVITY_TYPE", postedData.activityType.Value);
                    dynamicParameters.Add("@ACTIVITY_ID", postedData.activityId.Value);
                    var taskId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_ADD_TASK_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (taskId != null )
                    {
                       var returnData = CommonClass.ReturnMessage(taskId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(taskId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                if (postedData.activityType.Value== "Task")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.TSKADDED.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.TSKADDED.ToString(), "TSKADDED", title);
                                }

                                if (postedData.activityType.Value == "FollowUp")
                                {
                                    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.FOLLOWUP.ToString());

                                    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.FOLLOWUP.ToString(), "FOLLOWUP", title);
                                }



                            }

                        }
                        return returnData;



                        //errorMessage.Add(new ListString()
                        //{
                        //    Status = "Success",
                        //    Value = taskId
                        //});

                        //return JsonConvert.SerializeObject(errorMessage);
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

        public string SaveScheduleCall(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@SOURCE_ID", postedData.sourceId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@MODULE_NAME", postedData.moduleName.Value);
                    dynamicParameters.Add("@SUB_MODULE_NAME", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@CONTACT_NAME", postedData.contactid.Value);
                    dynamicParameters.Add("@SUBJECT", postedData.subjectText.Value);
                    dynamicParameters.Add("@CALL_PURPOSE", postedData.callpurpose.Value);
                    dynamicParameters.Add("@DUE_DATE", postedData.callScheduletime.Value);
                    dynamicParameters.Add("@DESCRIPTION", postedData.description.Value);
                    dynamicParameters.Add("@ACTIVITY_ID", postedData.activityId.Value);
                    dynamicParameters.Add("@ACTIVITY_TYPE", postedData.activityType.Value);

                    var taskId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_SCEDULE_CALL_V_9_10", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (taskId != null)
                    {

                        var returnData = CommonClass.ReturnMessage(taskId);
                        if (returnData != null)
                        {
                            var returnobj = JsonConvert.DeserializeObject(Convert.ToString(taskId));
                            string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                            string chkOperation = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CHK_OPERATION"]);

                            if (code == "1" && chkOperation == "INSERT")
                            {
                                var objCommon = new CommonClass(configuration);
                                //if (postedData.activityType.Value == "Task")
                                //{
                                    string title = Resources.Resources.ResourceManager.GetString("Call");

                                    objCommon.SetInboxNotification(returnobj, "Call", "Call", title);
                               // }

                                //if (postedData.activityType.Value == "FollowUp")
                                //{
                                //    string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.FOLLOWUP.ToString());

                                //    objCommon.SetInboxNotification(returnobj, ENotificationTypeCode.FOLLOWUP.ToString(), "Call", title);
                                //}
                            }

                        }
                        return returnData;


                        //errorMessage.Add(new ListString()
                        //{
                        //    Status = "Success",
                        //    Value = taskId
                        //});

                        //return JsonConvert.SerializeObject(errorMessage);
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
