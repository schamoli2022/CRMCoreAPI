using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public class NoteService:INoteService
    {
        private readonly string connectionString = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public NoteService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserID"></param>
        /// <param name="subModuleId"></param>
        /// <param name="typeCode"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public string getNoteLead(int companyId, int loggedInUserID, int subModuleId, string typeCode, int sourceId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserID);
                    dynamicParameters.Add("@SUB_MODULE_ID", subModuleId);
                    dynamicParameters.Add("@SOURCE_ID", typeCode);
                    dynamicParameters.Add("@TYPE_CODE", sourceId);
                    var customField = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_LEAD_NOTE_LISTING_DETAIL", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
