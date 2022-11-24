
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TALYGEN;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public UserService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
            //connectionString = @"Server=192.168.0.200;Database=Talygen_i20;user=developer;password=Wn$h5j8kZn6;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<RoleClass> GetAllRoles_Privilege_Allowed(long userId,long companyId,string userType,string  controller,string  action)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                   
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_TYPE", userType);
                    dynamicParameters.Add("@CONTROLLER", controller);
                    dynamicParameters.Add("@ACTION", action);


                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<RoleClass>("dbo.TALYGEN_SP_CO_GET_PRIVILEGE_ALLOWED", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }

            }
        }

        public List<RoleClass> GetAllRoles_Privileges(long userId, long companyId, string userType)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@LOGGEDIN_USER_ID", userId);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@USER_TYPE", userType);
                    



                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<RoleClass>("dbo.TALYGEN_SP_AP_GET_PRIVILEGES", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                    //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return sqlMetaData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }

            }
        }
    }
}
