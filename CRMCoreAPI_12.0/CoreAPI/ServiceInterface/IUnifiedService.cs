using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public interface IUnifiedService
    {
        string SaveCallIncommingOrOutgoing(string postString, long CompanyId, long UserId);

        string checkForRunningCall(long userId, long companyId, long extension);

        string SaveCallMapping(string postString, long UserId, long CompanyId);

        string DropDownForAddComment(long userId, long companyId);
        string DropDownForSubmodule(long userId, long companyId, string submoduleCode);
        string SaveCallOriginateCall(string actionId, string UniqueId, string extention, string CallTo, string callerName, long? userId, long? companyId, string callingMedium);
        string GetDefaultCA(long companyId);
        string SaveAsteriskConfiguration(string postString, long CompanyId, long UserId);
        string GetAsteriskList(long companyId);
        string GetAsteriskConnectionData(long id,long companyId);
        string GetSetupTabData(long companyId, string tabName);
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
        string getExtensionBasedOnStatus(string extensionStatus, long? userId, long? companyId);

        string getUserExtensionAndAuthKey(long companyId, long? userId);

        string getRecentcalls(long userId, long companyId, long? searchedCallLogId, long? pageSize, long? pageNumber, bool isLocalSearch, string searchKeyWord, string fromDate, string toDate);
        string GetNexmoList(long userId, long companyId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord);

        string SaveNexmoConfiguration(string postString, long CompanyId, long UserId);
        string NexmoStatusChange(long NexmoId, string type);

        string GetNexmoRecord( long userId, long companyId, long? NexmoId, bool isForVoice);

        string SaveNexmoPhoneNumber(string postString, long CompanyId, long UserId);

        string GetNexmoPhoneNumberList(long? NexmoId,long userId, long companyId, long? phoneNexmoId);
        List<DDLUser> GetNexmoUserList(long companyId, long userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="logId"></param>
        /// <returns></returns>
        Task<string> GetCallDetails(long companyId, long userId, long logId);
        Task<string> NexmoCallHangup(string postString, long CompanyId, long UserId);
        string UpdateCancelNumberNexmo(string postString, long CompanyId, long UserId);
        Task<string> SaveVonageConfiguration(string postString, long CompanyId, long UserId);
        Task<string> VonageList(long? VonageId, long companyId, long userId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord, bool isBasedOnUser = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VonageId"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> WebHookData(long? VonageId, long companyId, long userId);
        Task<string> GetVonageToken(long? CompanyId, long? UserId, long? VonageId);
        Task<string> SaveVonageWebHook(string postString, long CompanyId, long UserId); 
        Task<string> VonageStatusUpdate(long? VonageId, long CompanyId, long UserId, string StatusForUpdate);
        Task<string> SaveVonageUsers(string postString, long CompanyId, long UserId);
        Task<string> UpdateVonageCallHoldStatus(long LogId, long UserId, long CompanyId, string Status); 
        Task<string> UpdateVonageUserId(long UserId, long CompanyId, long VonageFirstUserId, long VonageUserSecondId);
        //Task<string> GetSingleCallDetail(long? CompanyId, long? UserId, long? CallLogId);

        Task<string> TokBoxList(long? TokboxId, long companyId, long userId, string sortBy, string sortExp, long? pageSize, long? pageNumber, string searchKeyWord);

        Task<string> TokBoxStatusUpdate(long? VonageId, long CompanyId, long UserId, string StatusForUpdate);

        Task<string> SaveTokBoxConfiguration(string postString, long CompanyId, long UserId);
    }
}
