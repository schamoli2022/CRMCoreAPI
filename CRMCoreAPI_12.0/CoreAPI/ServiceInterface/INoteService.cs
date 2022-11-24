using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public interface INoteService
    {
        /// <summary>
        /// Implementation of note listing method
        /// </summary>
        /// <returns></returns>
        string getNoteLead(int companyId, int loggedInUserID, int subModuleId, string typeCode, int sourceId);
    }
}
