using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    /// <summary>
    /// Default Lead Note Interface
    /// </summary>
    public interface ILeadNoteService
    {
        /// <summary>
        /// This method is use to fetch notes for lead 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="LOGGEDINUSERID"></param>
        /// <param name="crmleadid"></param>
        /// <returns></returns>
        string getNoteLead(int companyId, int LOGGEDINUSERID, int crmleadid);

    }
}
