using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAttachmentService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserID"></param>
        /// <param name="moduleCOde"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        string GetAttachment(int sourceId, long companyId, long loggedInUserID, string moduleCode, string subModuleCode);
    }

}
