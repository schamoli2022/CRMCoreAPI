using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using System.Data.SqlClient;
using Dapper;
using CRMCoreAPI.ServiceModels;
using Newtonsoft.Json.Linq;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    public class ItemsGroup : IitemsGroupService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        public ItemsGroup(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        public string GetItems(long companyId, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);

                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_ITEMS_GROUP_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        //public string SaveItem(string postString)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    dynamic sys = JsonConvert.DeserializeObject(postString);
        //    int index = 1;
        //    try
        //    {
        //        var tempJson = JsonConvert.DeserializeObject(Convert.ToString(sys.postString));
        //        if (tempJson!=null)
        //        {
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@json", "["+Convert.ToString(JsonConvert.SerializeObject(tempJson))+"]");
        //                var groupId = sqlConnection.Query<string>("TALYGEN_SP_SAVE_ITEMS_GROUP", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //                var returnData = CommonClass.ReturnMessage(groupId);
                        
        //                return returnData;
        //            }
        //        }
        //        errorMessage.Add(new ListString()
        //        {
        //            Status = "Failure"
        //        });
        //        return JsonConvert.SerializeObject(errorMessage);
        //    }
        //    // return "true";            
        //    catch (Exception ex)
        //    {
        //        errorMessage.Add(new ListString()
        //        {
        //            Status = "Failure"
        //        });
        //        return JsonConvert.SerializeObject(errorMessage);
        //    }
        //}
    }
}
