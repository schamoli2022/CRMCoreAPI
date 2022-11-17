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
    public class TrophiesAndBadgesService : ITrophiesAndBadgesService
    {
        private readonly string connectionString = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public TrophiesAndBadgesService(IConfiguration  _configuration)
        {
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="trophyOrBadgeName"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        /// <param name="status_Ids"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string getTrohpiesOrBadges(long companyId, long loggedInUserId, string trophyOrBadgeName, string action, string type, string status_Ids, string sortBy, string sortExp, int? pageSize, int? pageNumber, string langCode, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", loggedInUserId);
                    dynamicParameters.Add("@TROPHY_BADGE_NAME", trophyOrBadgeName);
                    dynamicParameters.Add("@ACTION", action);
                    dynamicParameters.Add("@TYPE", type);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber); 
                    dynamicParameters.Add("@LANG_CODE", langCode);
                    dynamicParameters.Add("@STATUS_IDS", status_Ids);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    var leadListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_TROPHIES_BADGES_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        /// This method is used for fetching the custom fields for add form on the 
        /// basis of companyId, moduleName and subModuleCode. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns>Returns the fields in json string format</returns>
        public string GetcustomFieldgetTrohpiesOrBadges(long companyId, long userId, string moduleName, string subModuleCode)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveTrophiesAndBadges(string postString, long CompanyId, long UserId)
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
                        string jsonS = Convert.ToString(data);
                        dynamicParameters.Add("@json", jsonS);
                        var leadId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_TROPHYANDBADGE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
