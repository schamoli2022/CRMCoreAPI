using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface INoteServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="moduleId"></param>

        /// <returns></returns>
        string GetNotes(int sourceId, long companyId, long loggedInUserID, string subModuleCode, string moduleName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        string saveNote(string postString, long CompanyId, long UserId);
    }
}
