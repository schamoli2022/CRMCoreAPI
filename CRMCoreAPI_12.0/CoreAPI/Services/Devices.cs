using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using Dapper;
using System.Data.SqlClient;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Devices : IDevicesService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        public Devices(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetDevices(long companyId, long loggedInUserId, long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LOCATION_ID", locationid);
                    dynamicParameters.Add("@STATUS_ID", statusid);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_DEVICE_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string GetDeviceByID(long id, long companyId, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_DEVICE_GET_DATA_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SaveDevices(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson!=null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", "["+Convert.ToString(JsonConvert.SerializeObject(tempJson))+"]");
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var DeviceId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_DEVICE_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(DeviceId);                      
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


        public string SaveDevicesAccessibility(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", "[" + Convert.ToString(JsonConvert.SerializeObject(tempJson)) + "]");
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_AP_IOT_SAVE_DEVICE_ACCESSBILITY_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(leadId);
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

        public string DeviceAccessibilityUpdateViewManageEnrole(long deviceId,string canView, string canManage, string canEnrole)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Id", deviceId);
                    dynamicParameters.Add("@can_view", canView);
                    dynamicParameters.Add("@can_manage", canManage);
                    dynamicParameters.Add("@can_enrole", canEnrole);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_DEVICE_ACCESSIBILIY_UPDATE_VIEW_MANAGE_ENROLE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetDevicesAccessibility(long companyId, long loggedInUserId, long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LOCATION_ID", locationid);
                    dynamicParameters.Add("@STATUS_ID", statusid);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_DEVICE_ACCESSIBILIY_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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


        public string GetEnrolmentListing(long companyId, long loggedInUserId, long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                   
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LOCATION_ID", locationid);
                    dynamicParameters.Add("@STATUS_ID", statusid);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_ENROLMENT_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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



        public string SaveAttendenceRule(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", "[" + Convert.ToString(JsonConvert.SerializeObject(tempJson)) + "]");
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_AP_SAVE_ATTENDENCE_RULE_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(leadId);
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

        public string GetDeviceAccessibilityByID(long id, long companyId, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_DEVICE_ACCESSIBILITY_GET_DATA_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string GetEnrolmentByID(long id, long companyId, long userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ID", id);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_EMPLOYEE_ENROLMENT_GET_DATA_BY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SaveEnrolment(string enrolmentData, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(enrolmentData);
            int index = 1;
            try
            {
                var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
                if (tempJson != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@json", "[" + Convert.ToString(JsonConvert.SerializeObject(tempJson)) + "]");
                        dynamicParameters.Add("@COMPANY_ID", CompanyId);
                        dynamicParameters.Add("@USER_ID", UserId);
                        var DeviceId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_ENROLMENT_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var returnData = CommonClass.ReturnMessage(DeviceId);
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

        public string ManageDevicebyUSerGetListing(long companyId, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LOCATION_ID", locationid);
                    dynamicParameters.Add("@STATUS_ID", statusid);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_MANAGE_DEVICE_BY_USER_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public string SearchDevices(long companyId, long loggedInUserId, long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid, string sublocation, long? itemtype, string itemdescription, string uniquecode, string serialnumber, string ipaddress, long? connectionstatus, long? item, long? hardwaretype)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@LOCATION_ID", locationid);
                    dynamicParameters.Add("@STATUS_ID", statusid);
                    dynamicParameters.Add("@SUB_LOCATION", sublocation);
                    dynamicParameters.Add("@ITEM_TYPE", itemtype);
                    dynamicParameters.Add("@ITEM_DESCRIPTION", itemdescription);
                    dynamicParameters.Add("@UNIQUE_CODE", uniquecode);
                    dynamicParameters.Add("@SERIAL_NUMBER", serialnumber);
                    dynamicParameters.Add("@IP_ADDRESS", ipaddress);
                    dynamicParameters.Add("@CONNECTION_STATUS", connectionstatus);
                    dynamicParameters.Add("@HARDWARE_TYPE", hardwaretype);
                    dynamicParameters.Add("@ITEM", item);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SEARCH_DEVICE_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        // Attendance Report
        public string GetTimeAttendanceReportForWeb(long CompanyId, long LoggedUserId, string EmpName, string EmpCode, string FromDate, string ToDate, string ReportName, long DeptId, long ShiftId, int PageNumber, int PageSize)
        {
            List<AttendanceReport> sqlMetaData = null;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    //DateTime dtFrom = new DateTime();
                    //dtFrom = Convert.ToDateTime(FromDate);
                    //string FDate = dtFrom.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

                    //DateTime dtTo = new DateTime();
                    //dtTo = Convert.ToDateTime(ToDate);
                    //string TDate = dtTo.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CompanyId", CompanyId);
                    dynamicParameters.Add("@LoggedUserId", LoggedUserId);
                    dynamicParameters.Add("@EmpName", EmpName);
                    dynamicParameters.Add("@EmpCode", EmpCode);                 
                    dynamicParameters.Add("@FromDate", FromDate);
                    dynamicParameters.Add("@ToDate", ToDate);
                    dynamicParameters.Add("@ReportName", ReportName);
                    dynamicParameters.Add("@DepartmentId", DeptId);
                    dynamicParameters.Add("@ShiftId", ShiftId);
                    dynamicParameters.Add("@PageNumber", PageNumber);
                    dynamicParameters.Add("@PageSize", PageSize);
                   
                    sqlMetaData = sqlConnection.Query<AttendanceReport>("TALYGEN_SP_AP_GET_TA_DAILY_MONTHLY_REPORT", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                    
                    string json = JsonConvert.SerializeObject(sqlMetaData);
                    return json;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }


        public string GetEmployeeNames(long CompanyId, long LoggedUserId, int ISiNCLUDELOGINUSERID, string APPROVALGROUPIDS,string APPROVALCHAINIDS, string MODULENAME)
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", LoggedUserId);
                    dynamicParameters.Add("@IS_iNCLUDE_LOGIN_USER_ID", ISiNCLUDELOGINUSERID);
                    dynamicParameters.Add("@APPROVAL_GROUP_IDS", APPROVALGROUPIDS);
                    dynamicParameters.Add("@APPROVAL_CHAIN_IDS", APPROVALCHAINIDS);
                    dynamicParameters.Add("@MODULE_NAME", MODULENAME);
                 
                    var sqlMetaData = sqlConnection.Query<GetEmployeeNames>("TALYGEN_SP_AP_DDL_GET_USERS_BY_COMPANY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();

                    string json = JsonConvert.SerializeObject(sqlMetaData);
                    return json;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }


        public string GetDepartmentNames(long CompanyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANYID", CompanyId);                  
                    var sqlMetaData = sqlConnection.Query<GetDeptNames>("TALYGEN_SP_BI_DEPARTMENT_LIST_BY_COMPANY_ID", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();

                    string json = JsonConvert.SerializeObject(sqlMetaData);
                    return json;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        public string GetShiftNames(long CompanyId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    var sqlMetaData = sqlConnection.Query<GetShiftNames>("TALYGEN_SP_AP_HR_GET_ALL_SHIFT", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();

                    string json = JsonConvert.SerializeObject(sqlMetaData);
                    return json;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }



        #region TimeAttendance
        public string GetAttendanceDDLData(long? id, long companyId, long userId, string moduleName, string type, string search, long? refId)
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
        #endregion

    }
}
