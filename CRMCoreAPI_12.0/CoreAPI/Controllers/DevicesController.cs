using System;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class DevicesController : BaseController
    {
        private readonly IDevicesService _devices;
        private IConfiguration configuration;
        public DevicesController(IDevicesService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _devices = services;
            configuration = config;
        }

        [HttpGet]
        [Route("DevicesListing")]
        public async Task<IActionResult> DevicesListing(long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            try
            {
                var DeviceData = _devices.GetDevices(CompanyId, UserId, userId, sortBy, sortExp, pageSize, pageNumber, locationid, statusid);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }

        [HttpGet]
        [Route("SearchDevicesListing")]
        public async Task<IActionResult> SearchDevicesListing(long userId, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid, string sublocation, long? itemtype, string itemdescription, string uniquecode, string serialnumber, string ipaddress, long? connectionstatus, long? item, long? hardwaretype)
        {
            try
            {
                var DeviceData = _devices.SearchDevices(CompanyId, UserId, userId, sortBy, sortExp, pageSize, pageNumber, locationid, statusid, sublocation, itemtype, itemdescription, uniquecode, serialnumber, ipaddress, connectionstatus, item, hardwaretype);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }

        [HttpGet]
        [Route("DevicesAccesibilityListing")]
        public async Task<IActionResult> DevicesAccesibilityListing(long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            try
            {
                var DeviceAccessibilityData = _devices.GetDevicesAccessibility(CompanyId, UserId, userId, id, sortBy, sortExp, pageSize, pageNumber, locationid, statusid);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceAccessibilityData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }

        [HttpPost]
        [Route("SaveDevice")]
        public async Task<IActionResult> SaveDevice([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _devices.SaveDevices(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("getDeviceByID")]
        public async Task<IActionResult> GetDeviceByID(long id)
        {
            try
            {
                var DeviceData = _devices.GetDeviceByID(id,CompanyId,UserId);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        [HttpGet]
        [Route("GetDeviceAccessibilityByID")]
        public async Task<IActionResult> GetDeviceAccessibilityByID(long id)
        {
            try
            {
                var DeviceData = _devices.GetDeviceAccessibilityByID(id, CompanyId, UserId);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        [HttpGet]
        [Route("DeviceAccessibilityUpdate_ViewManageEnrole")]
        public async Task<IActionResult> DeviceAccessibilityUpdate_ViewManageEnrole(long Id, string canView, string canManage, string canEnrole)
        {
            try
            {
                var DeviceData = _devices.DeviceAccessibilityUpdateViewManageEnrole(Id, canView, canManage, canEnrole);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveDevicesAccessibility")]
        public async Task<IActionResult> SaveDevicesAccessibility([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _devices.SaveDevicesAccessibility(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }



        [HttpGet]
        [Route("EnrolmentListing")]
        public async Task<IActionResult> EnrolmentListing(long userId, long id, string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            try
            {
                var DeviceData = _devices.GetEnrolmentListing(CompanyId, UserId, userId, id, sortBy, sortExp, pageSize, pageNumber, locationid, statusid);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }

        [HttpPost]
        [Route("SaveAttendence")]
        public async Task<IActionResult> SaveAttendence([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _devices.SaveAttendenceRule(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveEnrolment")]
        public async Task<IActionResult> SaveEnrolment([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _devices.SaveEnrolment(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetEnrolmentByID")]
        public async Task<IActionResult> GetEnrolmentByID(long id)
        {
            try
            {
                var DeviceData = _devices.GetEnrolmentByID(id, CompanyId, UserId);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        [HttpGet]
        [Route("ManageDeviceByUserListing")]
        public async Task<IActionResult> ManageDeviceByUserListing( string sortBy, string sortExp, int pageSize, int pageNumber, long? locationid, long? statusid)
        {
            try
            {
                var DeviceData = _devices.ManageDevicebyUSerGetListing(CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber, locationid, statusid);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(DeviceData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }


        [HttpGet]
        [Route("GetTimeAttendanceReportForWeb")]
        public async Task<IActionResult> GetTimeAttendanceReportForWeb(string EmpName, string EmpCode, string FromDate, string ToDate, string ReportName, long DeptId, long ShiftId, int PageNumber, int PageSize)
        {
            //AttendanceReportList AtnReportList = new AttendanceReportList();
            try
            {
                var AttndcReport = _devices.GetTimeAttendanceReportForWeb(CompanyId, UserId, EmpName, EmpCode, FromDate, ToDate, ReportName, DeptId, ShiftId, PageNumber, PageSize);
                //AtnReportList.GetAtndReport = AttndcReport;
                //return AtnReportList;
                return Content(AttndcReport, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("GetEmployeeNames")]
        public async Task<IActionResult> GetEmployeeNames(int ISiNCLUDELOGINUSERID, string APPROVALGROUPIDS, string APPROVALCHAINIDS, string MODULENAME)
        {          
            try
            {
                var EmpNames = _devices.GetEmployeeNames(CompanyId, UserId, ISiNCLUDELOGINUSERID, APPROVALGROUPIDS, APPROVALCHAINIDS, MODULENAME);
              
                return Content(EmpNames, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("GetDepartmentNames")]
        public async Task<IActionResult> GetDepartmentNames()
        {
            try
            {
                var DeptNames = _devices.GetDepartmentNames(CompanyId);
                
                return Content(DeptNames, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("GetShiftNames")]
        public async Task<IActionResult> GetShiftNames()
        {
            try
            {
                var ShiftNames = _devices.GetShiftNames(CompanyId);

                return Content(ShiftNames, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        #region TimeAttendance

        [HttpGet]
        [Route("GetAttendanceDDLData")]
        public async Task<IActionResult> GetAttendanceDDLData(long? id, string moduleName, string type, string search, long? refId)
        {
            string msg = _devices.GetAttendanceDDLData(id, CompanyId, UserId, moduleName, type, search, refId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

    }
}