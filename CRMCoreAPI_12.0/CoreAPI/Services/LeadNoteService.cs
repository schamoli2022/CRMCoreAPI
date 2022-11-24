using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CRMCoreAPI.ServiceInterface;

namespace CRMCoreAPI.Services
{
    public class LeadNoteService: ILeadNoteService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// This is for note tab
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="LOGGEDINUSERID"></param>
        /// <param name="crmleadid"></param>
        /// <returns></returns>
        public string getNoteLead(int companyId, int LOGGEDINUSERID, int crmleadid)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@company_id", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", LOGGEDINUSERID);
                    dynamicParameters.Add("@crm_lead_id", crmleadid);
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
