using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface IWidgetsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="userTypeID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        string getWidgetsList(long companyId, long loggedInUserId, long userTypeID, DateTime? fromDate, DateTime? toDate, string langCode, string reportType);
        //string GetTickets(long userId, long companyId, string moduleName);
    }
}
