using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CRMCoreAPI.Common;


namespace CRMCoreAPI.Services
{
    public class TerritoryService : ITerritoryService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public TerritoryService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>



        public string GetTerritory(string TerritoryName, string ParentTerritoryName, string UserName, int? TerritoryID, long CompanyID, string CreatedAt, string ModifiedAt, string LangCode, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (TerritoryName == "")
                    {
                        TerritoryName = null;
                    }
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TERRITORY_NAME", TerritoryName );
                    dynamicParameters.Add("@PARENT_TERRITORY_NAME", ParentTerritoryName);
                    dynamicParameters.Add("@USERS_NAME", UserName);
                    dynamicParameters.Add("@TERRITORY_ID", TerritoryID);
                    dynamicParameters.Add("@COMPANY_ID", CompanyID);
                    dynamicParameters.Add("@CREATED_AT", CreatedAt);
                    dynamicParameters.Add("@MODIFIED_AT", ModifiedAt);
                    dynamicParameters.Add("@LANG_CODE", LangCode);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_TERRITORY_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }

            }
        }
        /// <param name="companyId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetcustomFieldTerritory(long companyId, long userID, string moduleName, string subModuleCode)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userID);
                    dynamicParameters.Add("@MODULE_NAME", moduleName);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    var customField = sqlConnection.Query<string>("TALYGEN_SERVICE_GET_FIELDS_BY_MODULE_SUB_MODULE_ID", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return customField;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        public string SaveTerritory(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic sys = JsonConvert.DeserializeObject(postString);
            int index = 1;
            dynamic data = null;
            try
            {
                foreach (var attr in sys)
                {
                    var newObj = Convert.ToString(attr.Value);

                    dynamic sysss = JsonConvert.DeserializeObject(newObj);

                    foreach (var name in sysss)
                    {
                        if (index == 1)
                        {
                            data = name.Value;
                            break;
                        }
                    }
                }
                if (data != null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        string jsonS = "[" + Convert.ToString(data) + "]";
                        dynamicParameters.Add("@json", jsonS);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_TERRITORY", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                        //return "true";
                        //if (leadId != null)
                        //{
                        //    errorMessage.Add(new ListString()
                        //    {
                        //        Status = "Success"
                        //    });
                        //    return JsonConvert.SerializeObject(errorMessage);
                        //}
                        //else
                        //{
                        //    errorMessage.Add(new ListString()
                        //    {
                        //        Status = "Failure"
                        //    });
                        //    return JsonConvert.SerializeObject(errorMessage);
                        //}

                        return CommonClass.ReturnMessage(leadId);
                    }
                }
                errorMessage.Add(new ListString()
                {
                    Status = "Failure"
                });
                return JsonConvert.SerializeObject(errorMessage);
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
