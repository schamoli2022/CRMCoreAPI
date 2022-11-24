using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Models;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Dynamic;
using System.Web.Http;
using Newtonsoft.Json;
using CRMCoreAPI.ServiceModels;



namespace CRMCoreAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices _services;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ExpenseController(IExpenseServices services)
        {
            _services = services;
        }

        [HttpPost]
        [Route("postjsondata")]
        public ActionResult postjsondata( FormCollection keyValuePairs) 
        {
            var expenseItems = keyValuePairs;
            if (expenseItems == null)
            {
                return NotFound();
            }
            return Ok(expenseItems);
        }

        [HttpPost]
        [Route("AddExpense")]
        public ActionResult<ExpenseItems> AddExpenseItems(ExpenseItems items)
        {
            var expenseItems = _services.AddExpenseItems(items);
            if(expenseItems == null)
            {
                return NotFound();
            }
            return Ok(expenseItems);
        }

        [HttpGet]
        [Route("GetExpenseItems")]
        public ActionResult<Dictionary<string,ExpenseItems>> GetExpenseItems()
        {
            var expenseItems = _services.GetExpenseItems();

            if (expenseItems.Count == 0)
            {
                return NotFound();
            }
            else
                return expenseItems;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetExpenseList")]
        public async Task<IActionResult> GetExpenseList(int companyId) 
        {
            var expenseData = _services.GetExpenseList(companyId);

            //var sqlMetaData = JsonConvert.SerializeObject(expenseData.sqlMetaData);
            var sqlMetaData = (expenseData.sqlMetaData).Replace("[{", "{").Replace("}]","}");
            var firstEntry = new DataEntry
            {
                Id = Guid.NewGuid(),
                Name = "GetExpenseList",
                Description = "Get all the leads in a company",
                //Metadata = "{ \"lead_id\": \"12\", \"company_id\": \"100125\", \"lead_owner_id\": 34 }"
                Metadata = sqlMetaData
            };

            var firstSchema = new Dictionary<string, Type>();
            try
            {
                var newfieldData = JsonConvert.DeserializeObject<List<FieldsData>>(expenseData.fieldMetadata);

                foreach (var item in newfieldData)
                {

                    switch (item.data_type)
                    {
                        case "string":
                            firstSchema.Add(item.column_name, typeof(string));

                            break;
                        case "Guid":
                            firstSchema.Add(item.column_name, typeof(Guid));
                            break;
                        case "Byte[]":
                            firstSchema.Add(item.column_name, typeof(byte));
                            break;
                        case "Boolean":
                            firstSchema.Add(item.column_name, typeof(bool));
                            break;
                        case "Byte":
                            firstSchema.Add(item.column_name, typeof(byte));
                            break;
                        case "Int16":
                            firstSchema.Add(item.column_name, typeof(Int16));
                            break;
                        case "Int32":
                            firstSchema.Add(item.column_name, typeof(Int32));
                            break;
                        case "Int64":
                            firstSchema.Add(item.column_name, typeof(Int64));
                            break;
                        case "Single":
                            firstSchema.Add(item.column_name, typeof(Single));
                            break;
                        case "Double":
                            firstSchema.Add(item.column_name, typeof(Double));
                            break;
                        default:
                            firstSchema.Add(item.column_name, typeof(string));
                            break;
                    }

                }
                var dynamicFirstEntry = new DynamicDataEntry(firstEntry, firstSchema);

                var result = new[] { dynamicFirstEntry };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PostLead")]
        public async Task<IActionResult> PostLead(string leadData)
        {
            leadData = "{\"data\":[{ \"lead_name\": \"Crm1\", \"company_id\": \"100125\", \"lead_owner_id\": \"34\" }],\"validation\":[{\"field_name\":\"lead_name\",\"required\" : \"true\" , \"min\": \"6\" , \"type\":\"email\"}]}";
            _services.ValidateData(leadData);
            //var sqlMetaData = (expenseData).Replace("[{", "{").Replace("}]", "}");
            //sqlMetaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(sqlMetaData);
            
            //var firstEntry = new DataEntry
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "GetExpenseList",
            //    Description = "Get all the leads in a company",
            //    //Metadata = "{ \"lead_id\": \"12\", \"company_id\": \"100125\", \"lead_owner_id\": 34 }"
            //    Metadata = sqlMetaData
            //};

            //var firstSchema = new Dictionary<string, Type>();
            try
            {          
               // var dynamicFirstEntry = new DynamicDataEntry(firstEntry, firstSchema);

                //var result = new[] { dynamicFirstEntry };

                return Ok("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}