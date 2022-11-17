using CRMCoreAPI.Common;
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

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TaxService: ITaxService
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public TaxService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = _configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> SaveTaxCode(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    dynamicParameters.Add("@TAX_CODE", postedData.taxCode.Value);
                    var result = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_TAX_CODE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
                    var returnData = CommonClass.ReturnMessage(result);                    
                    return returnData;
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

        public async Task<string> SaveTax(string postString, long CompanyId, long UserId)
        {
            List<ListString> errorMessage = new List<ListString>();
            dynamic postedString = JsonConvert.DeserializeObject(postString);
            dynamic postedData = JsonConvert.DeserializeObject(Convert.ToString(postedString.postString));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@JSON", Convert.ToString(postedData));
                    dynamicParameters.Add("@COMPANY_ID", CompanyId);
                    dynamicParameters.Add("@USER_ID", UserId);
                    string returnData = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_SERVICE_SAVE_TAX_V_9_3", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());

                    return CommonClass.ReturnMessage(returnData);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetTaxByCodeId(long companyId, long loggedInUserId, long? id)
        {

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@ID", id);
                    var customField = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_PRODUCT_TYPE_TAX", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
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
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetAllTaxCode(long companyId, long userId, long? id)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", userId);
                    dynamicParameters.Add("@ID", id);
                    var customField = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_ALL_TAX_CODE", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
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
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="taxCode"></param>
        /// <returns></returns>
        public async Task<string> GetTaxByCode(long companyId, long loggedInUserId, string taxCode)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_ID", loggedInUserId);
                    dynamicParameters.Add("@TAX_CODE", taxCode);
                    var customField = await Task.FromResult(sqlConnection.Query<string>("TALYGEN_SP_SERVICE_GET_PRODUCT_TYPE_TAX", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault());
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
