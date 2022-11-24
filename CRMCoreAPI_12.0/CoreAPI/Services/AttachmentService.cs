using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string connectionString = null;

        public AttachmentService(IConfiguration _configuration)
        {
            //_configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        public string GetAttachment(int sourceId, long companyId, long loggedInUserID, string moduleCode, string subModuleCode)

        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@MODULE_NAME", moduleCode);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserID);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_ATTACHEMENT_GET_LISTING_DETAIL", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return leadListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
       

    }
}
