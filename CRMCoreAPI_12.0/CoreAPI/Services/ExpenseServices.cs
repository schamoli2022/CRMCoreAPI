using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRMCoreAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpenseServices : IExpenseServices
    {
        private readonly string connectionString = null;

        private IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public ExpenseServices(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public  CRMDynamicData GetExpenseList(int companyId)
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    CRMDynamicData dData = new CRMDynamicData();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@companyId", companyId);
                    dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    dData.sqlMetaData = sqlConnection.Query<string>("spGetLeads", dynamicParameters, commandType: CommandType.StoredProcedure).First();
                    dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
                    return dData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new CRMDynamicData();
                }
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadData"></param>
        public void ValidateData(string leadData )
        {
            //var parseTree = JsonConvert.DeserializeObject<JObject>(leadData);
            var data = "";
            var validation = "";

            try
            {
                var parsed = JsonConvert.DeserializeObject<JObject>(leadData);
                foreach (var property in parsed.Properties())
                {
                    Console.WriteLine(property.Name);
                    foreach (var innerProperty in ((JObject)property.Value).Properties())
                    {
                        Console.WriteLine("\t{0}: {1}", innerProperty.Name, innerProperty.Value.ToObject<object>());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);             
            }
            //foreach (var prop in parseTree.Properties())
            //{
            //    //data.Add(prop.Value.p)
            //    //Console.WriteLine(prop.Name + ": " + prop.Value.ToObject<object>());
            //}
        }

        private readonly Dictionary<string, ExpenseItems> _expenseItems;

        /// <summary>
        /// 
        /// </summary>
        public ExpenseServices()
        {
            _expenseItems = new Dictionary<string, ExpenseItems>();
        }

        public ExpenseItems AddExpenseItems(ExpenseItems items)
        {
            _expenseItems.Add(items.ItemName, items);
            return items;
        }

        public Dictionary<string, ExpenseItems> GetExpenseItems()
        {
            return _expenseItems;
        }      
    }
    /// <summary>
    /// 
    /// </summary>
    public class CRMDynamicData
    {
        /// <summary>
        /// 
        /// </summary>
        public string sqlMetaData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fieldMetadata { get; set; }
       

    }
   
    public class FieldsData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("column_name")]
        public string column_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data_type")]
        public string data_type { get; set; }
    }
}
