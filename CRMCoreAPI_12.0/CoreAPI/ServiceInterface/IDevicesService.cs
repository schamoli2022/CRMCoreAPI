using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.Devices;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// Default Device Interface
    /// </summary>
    public interface IDevicesService
    {
        string GetDevices(long companyId, long loggedInUserId, long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid);

        string SearchDevices(long CompanyId, long loggedInUserId, long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid, string sublocation, long? itemtype, string itemdescription, string uniquecode, string serialnumber, string ipaddress, long? connectionstatus, long? item, long? hardwaretype);

        string SaveDevices(string DeviceData, long CompanyId, long UserId);

        string GetDeviceByID(long id, long companyId, long userId);

        string GetDevicesAccessibility(long CompanyId, long loggedInUserId, long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid);

        string SaveDevicesAccessibility(string DeviceAccessibilityData, long CompanyId, long UserId);

        string GetDeviceAccessibilityByID(long id, long companyId, long userId);

        string GetEnrolmentListing(long companyId, long loggedInUserId, long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid);

        string GetEnrolmentByID(long id, long companyId, long userId);

        string SaveEnrolment(string enrolmentData, long CompanyId, long UserId);

        string SaveAttendenceRule(string attendenceRule, long CompanyId, long UserId);

        string ManageDevicebyUSerGetListing(long companyId, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid);

        string DeviceAccessibilityUpdateViewManageEnrole(long deviceId, string canView, string canManage, string canEnrole);

        //List<AttendanceReport> GetTimeAttendanceReportForWeb(long CompanyId, long LoggedUserId, string EmpName, string EmpCode, string FromDate, string ToDate, string ReportName, int pageNumber, int pageSize);

        string GetTimeAttendanceReportForWeb(long CompanyId, long LoggedUserId, string EmpName, string EmpCode, string FromDate, string ToDate, string ReportName, long DeptId, long ShiftId, int PageNumber, int PageSize);

        string GetEmployeeNames(long CompanyId, long LoggedUserId, int ISiNCLUDELOGINUSERID, string APPROVALGROUPIDS, string APPROVALCHAINIDS, string MODULENAME);

        string GetDepartmentNames(long CompanyId);

        string GetShiftNames(long CompanyId);



        #region Time Attendance

        string GetAttendanceDDLData(long? id, long companyId, long userId, string moduleName, string type, string search, long? refId);

        #endregion
    }
}
