using Dapper;
using CRMCoreAPI.ServiceInterface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public class WidgetsService : IWidgetsService
    {
        private readonly string connectionString = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public WidgetsService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="userTypeID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="langCode"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string getWidgetsList(long companyId, long loggedInUserId, long userTypeID, DateTime? fromDate, DateTime? toDate, string langCode,string reportType)
        {
           // return "1";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@USER_TYPE_ID", userTypeID);
                    dynamicParameters.Add("@FROM_DATE", fromDate);
                    dynamicParameters.Add("@TO_DATE", toDate);
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@REPORT_TYPE", reportType);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_CRM_DASHBOARD_WIDGETS", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                    return leadListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }

        //public string GetTickets(long userId, long companyId, string moduleName)
        //{
        //    // return "1";
        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@USERID", userId);
        //            dynamicParameters.Add("@COMPANY_ID", companyId);
        //            dynamicParameters.Add("@MODULE_NAME", moduleName);
        //            var leadListing = sqlConnection.Query<string>("TALYGEN_SP_AP_DASHBOARD_REPORTS_GET", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        //            return leadListing;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return "";
        //        }

        //    }
        //}
    }
}
