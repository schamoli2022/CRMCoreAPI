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
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public BrandService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="userid"></param>
        /// <param name="brandname"></param>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public string GetBrands(long CompanyID, long userid, string sortBy, string sortExp, int pageSize, int pageNum,long brandid=0, string brandname = "")


        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    if(brandname==null)
                    {
                        brandname = "";
                    }
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyID);
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", userid);
                    dynamicParameters.Add("@brand_name", brandname);
                    dynamicParameters.Add("@brand_id", brandid);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNum);


                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var brandListing = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_GET_BRAND_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return brandListing;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return ex.Message + " Connectionstring " + connectionString;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public string SaveBrand(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
                
                try
            {
                  
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@BRAND_id", postedData.data.brand_id.Value);
                    dynamicParameters.Add("@BRAND_NAME", postedData.data.brandname.Value);
                    dynamicParameters.Add("@dESCRIPTION", postedData.data.branddescription.Value);
                    dynamicParameters.Add("@createdby",UserId);
                    dynamicParameters.Add("@modified_by", UserId);
                    dynamicParameters.Add("@STATUS_ID", 1001);
                    dynamicParameters.Add("@company_id", CompanyId);
               
                    var taskId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_BRAND", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (taskId != null)
                    {
                        errorMessage.Add(new ListString()
                        {
                            Status = "Success",
                            Value = taskId
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

                //    if (data != null)
                //{
                //    using (var sqlConnection = new SqlConnection(connectionString))
                //    {



                //        sqlConnection.Open();
                //        var dynamicParameters = new DynamicParameters();
                //        //string jsonS = "[" + Convert.ToString(data) + "]";
                //        dynamicParameters.Add("@json", Convert.ToString(data));
                //        var brandId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_BRAND", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                //        //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                //        //return "true";
                //        if (brandId != null)
                //        {
                //            errorMessage.Add(new ListString()
                //            {
                //                Status = "Success"
                //            });
                //            return JsonConvert.SerializeObject(errorMessage);
                //        }
                //        else
                //        {
                //            errorMessage.Add(new ListString()
                //            {
                //                Status = "Failure"
                //            });
                //            return JsonConvert.SerializeObject(errorMessage);
                //        }
                //    }
                //}
                //errorMessage.Add(new ListString()
                //{
                //    Status = "Failure"
                //});
                //return JsonConvert.SerializeObject(errorMessage);
            }
            // return "true";            
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
