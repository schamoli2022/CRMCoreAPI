using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IForecastService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetForecast(int? forecast_id, long loggedInUserId, string ForecastName, string statusIds, long companyId, string forecastCreatedDate, string created_by, string forecastDateTo, byte? isPaged, string sortBy, string sortExp, int? pageSize, int? pageNum);

        /// <summary>
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        string GetcustomFieldForecast(long companyId, string moduleName, string subModuleCode, long userId);

        /// <summary>
        /// Implementation of save lead
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        string SaveForecast(string ForecastData, long CompanyId, long UserId);
    }
}
