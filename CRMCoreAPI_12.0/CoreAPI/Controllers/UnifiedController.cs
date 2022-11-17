using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRMCoreAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class UnifiedController : BaseController
    {
        private readonly IUnifiedService _united;

        public UnifiedController(IUnifiedService services, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _united = services;
        }

        [HttpPost, Route("SaveCallIncommingOrOutgoing")]
        public async Task<IActionResult> SaveCallIncommingOrOutgoing([FromBody] string postString)
        {
            try
            {
                var reslt = _united.SaveCallIncommingOrOutgoing(postString, CompanyId, UserId);
                return Content(reslt, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("CheckForRunningCall")]
        public async Task<IActionResult> CheckForRunningCall(long extension)
        {
            try
            {
                var reslt = _united.checkForRunningCall(UserId, CompanyId, extension);
                return Content(reslt, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveCallMapping")]
        public async Task<IActionResult> SaveCallMapping([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _united.SaveCallMapping(postString, UserId, CompanyId);
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
        [HttpGet, Route("DropDownForAddComment")]
        public async Task<IActionResult> DropDownForAddComment()
        {
            try
            {
                var reslt = _united.DropDownForAddComment(UserId, CompanyId);
                return Content(reslt, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("DropDownForSubmodule")]
        public async Task<IActionResult> DropDownForSubmodule(string submoduleCode)
        {
            try
            {
                var reslt = _united.DropDownForSubmodule(UserId, CompanyId, submoduleCode);
                return Content(reslt, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveCallOriginateCall")]
        public async Task<IActionResult> SaveCallOriginateCall([FromBody] string data)
        {
            try
            {
                 dynamic jsonObject = JObject.Parse(data);
                string callingMedium = String.Empty;
                try
                {
                    callingMedium = jsonObject.callingMedium.Value;
                }
                catch (Exception ex)
                {
                    callingMedium = null;
                }
                var reslt = _united.SaveCallOriginateCall(jsonObject.actionId.Value, jsonObject.UniqueId.Value, jsonObject.extension.Value, jsonObject.CallTo.Value, jsonObject.callerName.Value, UserId, CompanyId, callingMedium);
                return Content(reslt, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetDefaultCA")]
        public async Task<IActionResult> GetDefaultCA()
        {
            try
            {
                var result = _united.GetDefaultCA(CompanyId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveAsteriskConfiguration")]
        public async Task<IActionResult> SaveAsteriskConfiguration([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _united.SaveAsteriskConfiguration(postString, CompanyId, UserId);
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

        [HttpGet, Route("GetAsteriskList")]
        public async Task<IActionResult> GetAsteriskList()
        {
            try
            {
                var result = _united.GetAsteriskList(CompanyId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("GetAsteriskConnectionData")]
        public async Task<IActionResult> GetAsteriskConnectionData(long id)
        {
            try
            {
                var result = _united.GetAsteriskConnectionData(id,CompanyId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// exntension status like Idle, OnHold, Unavailable etc
        /// <param name="extensionStatus"></param>
        /// its logged in user id
        /// <param name="userId"></param>
        /// its logged in company id
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet, Route("getExtensionBasedOnStatus")]
        public async Task<IActionResult> getExtensionBasedOnStatus(string extensionStatus)
        {
            try
            {
                var result = _united.getExtensionBasedOnStatus(extensionStatus, UserId, CompanyId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("GetSetupTabData")]
        public async Task<IActionResult> GetSetupTabData(string tabName)
        {
            try
            {
                var result = _united.GetSetupTabData(CompanyId,tabName);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("getUserExtensionAndAuthKey")]
        public async Task<IActionResult> getUserExtensionAndAuthKey()
        {
            try
            {
                var result = _united.getUserExtensionAndAuthKey(CompanyId, UserId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns>return most recent calls data in JSON based on passed userid and company Id </returns>
        [HttpGet, Route("getRecentCalls")]
        public async Task<IActionResult> getRecentcalls(long? searchedCallLogId, long? pageSize, long? pageNumber, bool isLocalSearch, string searchKeyWord, string fromDate, string toDate)
        {
            try
            {

                var result = _united.getRecentcalls(UserId, CompanyId, searchedCallLogId, pageSize, pageNumber, isLocalSearch, searchKeyWord, fromDate, toDate);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns>return most recent calls data in JSON based on passed userid and company Id </returns>
        [HttpGet, Route("getNexmoList")]
        public async Task<IActionResult> GetNexmoList(string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord)
        {
            try
            {
                var result = _united.GetNexmoList(UserId, CompanyId, sortBy, sortExp, pageSize, pageNumber,  searchKeyWord);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveNexmoConfiguration")]
        public async Task<IActionResult> SaveNexmoConfiguration([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _united.SaveNexmoConfiguration(postString, CompanyId, UserId);
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
        [HttpGet]
        [Route("NexmoStatusChange")]
        public async Task<IActionResult> NexmoStatusChange(long NexmoId, string type)
        {
            try
            {
                string msg = _united.NexmoStatusChange(NexmoId, type);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetNexmoRecord")]
        public async Task<IActionResult> GetNexmoRecord(long? NexmoId, bool isForVoice = false)
        {
            try
            {
                string msg = _united.GetNexmoRecord( UserId, CompanyId, NexmoId, isForVoice);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetNexmoPhoneNumberList")]
        public async Task<IActionResult> GetNexmoPhoneNumberList(long? NexmoId,long userId, long? phoneNexmoId)
        {
            try
            {
                string msg = _united.GetNexmoPhoneNumberList(NexmoId,userId, CompanyId, phoneNexmoId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetNexmoUserList")]
        public async Task<IActionResult> GetNexmoUserList()
        {
            try
            {
                var msg = _united.GetNexmoUserList(CompanyId, UserId);
                var json = JsonConvert.SerializeObject(msg);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveNexmoPhoneNumber")]
        public async Task<IActionResult> SaveNexmoPhoneNumber([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _united.SaveNexmoPhoneNumber(postString, CompanyId, UserId);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="logId"></param>
        /// <returns></returns>
        [Route("GetCallDetails")]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> GetCallDetails(long logId)
        {
            try
            {
                var result = await _united.GetCallDetails(CompanyId, UserId, logId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost]
        [HttpGet]
        [Route("NexmoCallHangup")]
        public async Task<IActionResult> NexmoCallHangup([FromBody] string postString)
        {
            try
            {
                var result = await _united.NexmoCallHangup(postString, CompanyId, UserId);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("UpdateCancelNumberNexmo")]
        public async Task<IActionResult> UpdateCancelNumberNexmo([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _united.UpdateCancelNumberNexmo(postString, CompanyId, UserId);
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

        //[Route("GetSingleCallDetail")]
        //public async Task<IActionResult> GetCallDetail(long? CompanyId, long? UserId, long CallLogId)
        //{
        //    try
        //    {
        //        string msg = await _united.GetSingleCallDetail(CompanyId, UserId, CallLogId);
        //        return Content(msg, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return NotFound();
        //    }
        //}

        [HttpPost, Route("SaveVonageConfiguration")]
        public async Task<IActionResult> SaveVonageConfiguration([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _united.SaveVonageConfiguration(postString, CompanyId, UserId);
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
        [HttpGet, Route("VonageList")]
        public async Task<IActionResult> VonageList(long? VonageId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord, bool isBasedOnUser = false)
        {
            try
            {
                string msg = await _united.VonageList(VonageId, CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber, searchKeyWord, isBasedOnUser);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("WebHookData")]
        public async Task<IActionResult> WebHookData(long? VonageId)
        {
            try
            {
                string msg = await _united.WebHookData(VonageId, CompanyId, UserId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("GetVonageToken")]
        public async Task<IActionResult> GetVonageToken(long? VonageId)
        {
            try
            {
                string msg = await _united.GetVonageToken(CompanyId, UserId, VonageId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost, Route("SaveVonageWebHook")]
        public async Task<IActionResult> SaveVonageWebHook([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _united.SaveVonageWebHook(postString, CompanyId, UserId);
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

        [HttpPost, Route("VonageStatusUpdate")]
        public async Task<IActionResult> VonageStatusUpdate([FromBody] string postString)
        {
            dynamic jsonObject = JObject.Parse(postString);
            long VonageId = 0;
            string active = string.Empty;
            try
            {
                VonageId = Convert.ToInt64(jsonObject.VonageId.Value);
                active = jsonObject.StatusForUpdate.Value;
            }
            catch (Exception ex) { }
            string msg = await _united.VonageStatusUpdate(VonageId, CompanyId, UserId, active);
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

        [HttpPost, Route("SaveVonageUsers")]
        public async Task<IActionResult> SaveVonageUsers([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _united.SaveVonageUsers(postString, CompanyId, UserId);
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

        [HttpPost, Route("UpdateVonageCallHoldStatus")]
        public async Task<IActionResult> UpdateVonageCallHoldStatus([FromBody] string postString)
        {
            try
            {
                dynamic jsonObject = JObject.Parse(postString);
                long LogId = 0;
                string Status = string.Empty;
                LogId = Convert.ToInt64(jsonObject.LogId.Value);
                Status = jsonObject.StatusForUpdate.Value;
                string msg = await _united.UpdateVonageCallHoldStatus(LogId, UserId, CompanyId, Status);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("UpdateVonageUserId")]
        public async Task<IActionResult> UpdateVonageUserId([FromBody] string postString)
        {
            try
            {
                dynamic jsonObject = JObject.Parse(postString);
                long VonageId = 0;
                long VonageFirstUserId = 0;
                long VonageSecondUserId = 0;
                VonageFirstUserId = Convert.ToInt64(jsonObject.VUserFirstId.Value);
                VonageSecondUserId = Convert.ToInt64(jsonObject.VUserSecondId.Value);
                string msg = await _united.UpdateVonageUserId(UserId, CompanyId, VonageFirstUserId, VonageSecondUserId);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("TokBoxList")]
        public async Task<IActionResult> TokBoxList(long? TokboxId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord)
        {
            try
            {
                string msg = await _united.TokBoxList(TokboxId, CompanyId, UserId, sortBy, sortExp, pageSize, pageNumber, searchKeyWord);
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("TokBoxStatusUpdate")]
        public async Task<IActionResult> TokBoxStatusUpdate([FromBody] string postString)
        {
            dynamic jsonObject = JObject.Parse(postString);
            long TokboxId = 0;
            string active = string.Empty;
            try
            {
                TokboxId = Convert.ToInt64(jsonObject.TokboxId.Value);
                active = jsonObject.StatusForUpdate.Value;
            }
            catch (Exception ex) { }
            string msg = await _united.TokBoxStatusUpdate(TokboxId, CompanyId, UserId, active);
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

        [HttpPost, Route("SaveTokBoxConfiguration")]
        public async Task<IActionResult> SaveTokBoxConfiguration([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = await _united.SaveTokBoxConfiguration(postString, CompanyId, UserId);
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
    }
}