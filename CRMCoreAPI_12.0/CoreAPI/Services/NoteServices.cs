using CRMCoreAPI.ServiceInterface;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    public class NoteServices:INoteServices
    {
        private readonly string connectionString = null;
        private object sqlConnection;

        public NoteServices(IConfiguration _configuration)
        {
            //_configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        public string GetNotes(int sourceId, long companyId, long loggedInUserID, string subModuleCode, string moduleName)

        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRIMARY_KEY_VALUE", sourceId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserID);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_NOTE_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// <summary>
        /// save the note and return created note id with the success message
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string saveNote(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CRM_NOTE_ID", postedData.crmNotesId.Value);
                    dynamicParameters.Add("@SOURCE_ID", postedData.sourceId.Value);
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", postedData.subModuleCode.Value);
                    dynamicParameters.Add("@NOTE", postedData.note.Value);
                    dynamicParameters.Add("@IS_PRIVATE", postedData.isPrivate.Value);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@IS_SUBMIT", 1);
                    var noteId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_NOTE_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (noteId != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = noteId
                        });
                        
                        return JsonConvert.SerializeObject(errorMessage);
                    }
                    else
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Failure"
                        });
                        return JsonConvert.SerializeObject(errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    errorMessage.Add(new ListString()
                    {
                        Status = "Failure"
                    });
                    return JsonConvert.SerializeObject(errorMessage);
                }
            }

        }
    }
}
