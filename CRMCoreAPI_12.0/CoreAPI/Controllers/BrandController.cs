using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IBrandService _Brands;

        public BrandController(IBrandService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _Brands = services;
        }


        /// <summary>
        /// 
        /// </summary>

        /// <param name="userid"></param>
        /// <param name="companyID"></param>
       
        /// <returns></returns>
        [HttpGet]
        [Route("BrandListing")]
        public async Task<IActionResult> BrandListing(string brandname, string sortBy, string sortExp, int pageSize, int pageNum, long brandid = 0)
        {
            try
            {
                var expenseData = _Brands.GetBrands(CompanyId, UserId, sortBy,sortExp, pageSize, pageNum, brandid, brandname);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }


        }
        /// <summary>
        /// This method is used for saving the Deal data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBrand")]
        public async Task<IActionResult> PostBrand([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _Brands.SaveBrand(postString, CompanyId, UserId);
            try
            {
                return Content(msg, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        /// <summary>
        /// This method is used for saving the Deal data
        /// also validation method is called.
        /// </summary>
        /// <param name="postString">Json string is passed in this parameter</param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("PostDeal")]
        //public async Task<IActionResult> PostBrand([FromBody] string postString)
        //{
        //    ////postString = "{\"data\":[{ \"lead_name\": \"Crm1\", \"company_id\": \"100125\", \"lead_owner_id\": \"34\",\"client_email\": \"\" }],\"validation\":[{\"field_name\":\"lead_name\",\"required\" : \"true\" , \"min\": \"6\" , \"type\":\"email\"},{\"field_name\":\"client_email\",\"required\" : \"true\" ,\"type\":\"email\"}]}";
        //    ////string msg=_services.ValidateData(postString);
        //    ////postString = "{\"postString\":\"{\\\"data\\\":[{\\\"field_name\\\":\\\"percentage_close\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":8},{\\\"field_name\\\":\\\"company_id\\\",\\\"field_value\\\":190559,\\\"custom_field_id\\\":2},{\\\"field_name\\\":\\\"project_name\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":19},{\\\"field_name\\\":\\\"name\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":31},{\\\"field_name\\\":\\\"ref_source_id\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":27},{\\\"field_name\\\":\\\"ref_source_name\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":28},{\\\"field_name\\\":\\\"crm_ref_id\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":29},{\\\"field_name\\\":\\\"lead_owner\\\",\\\"field_value\\\":\\\"399160\\\",\\\"custom_field_id\\\":1},{\\\"field_name\\\":\\\"channel_id\\\",\\\"field_value\\\":\\\"186730\\\",\\\"custom_field_id\\\":3},{\\\"field_name\\\":\\\"industry_type_id\\\",\\\"field_value\\\":\\\"9492\\\",\\\"custom_field_id\\\":4},{\\\"field_name\\\":\\\"expected_close_date\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":5},{\\\"field_name\\\":\\\"money_value\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":6},{\\\"field_name\\\":\\\"is_existing_client\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":9},{\\\"field_name\\\":\\\"existing_client_id\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":10},{\\\"field_name\\\":\\\"project_id\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":11},{\\\"field_name\\\":\\\"created_at\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":12},{\\\"field_name\\\":\\\"created_by\\\",\\\"field_value\\\":151535,\\\"custom_field_id\\\":13},{\\\"field_name\\\":\\\"modified_at\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":14},{\\\"field_name\\\":\\\"modified_by\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":15},{\\\"field_name\\\":\\\"status_id\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":18},{\\\"field_name\\\":\\\"lead_date\\\",\\\"field_value\\\":\\\"12/6/2018\\\",\\\"custom_field_id\\\":20},{\\\"field_name\\\":\\\"is_completed\\\",\\\"field_value\\\":null,\\\"custom_field_id\\\":21},{\\\"field_name\\\":\\\"client_first_name\\\",\\\"field_value\\\":\\\"ghfdg\\\",\\\"custom_field_id\\\":22},{\\\"field_name\\\":\\\"client_last_name\\\",\\\"field_value\\\":\\\"hdgfhdfgh\\\",\\\"custom_field_id\\\":23},{\\\"field_name\\\":\\\"client_email\\\",\\\"field_value\\\":\\\"gfhdfg@dfsg.ghfj\\\",\\\"custom_field_id\\\":24},{\\\"field_name\\\":\\\"crm_type\\\",\\\"field_value\\\":\\\"ghfgfhgf\\\",\\\"custom_field_id\\\":25},{\\\"field_name\\\":\\\"contact_number\\\",\\\"field_value\\\":\\\"54645654\\\",\\\"custom_field_id\\\":30}],\\\"validation\\\":[{\\\"field_name\\\":\\\"percentage_close\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"company_id\\\",\\\"required\\\":true,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"project_name\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"name\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"ref_source_id\\\",\\\"required\\\":false,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"ref_source_name\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"crm_ref_id\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"lead_owner\\\",\\\"required\\\":true,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"channel_id\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"industry_type_id\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"expected_close_date\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"money_value\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"is_existing_client\\\",\\\"required\\\":false,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"existing_client_id\\\",\\\"required\\\":false,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"project_id\\\",\\\"required\\\":false,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"created_at\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"created_by\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"modified_at\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"modified_by\\\",\\\"required\\\":false,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"status_id\\\",\\\"required\\\":true,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"lead_date\\\",\\\"required\\\":true,\\\"length\\\":20,\\\"type\\\":null},{\\\"field_name\\\":\\\"is_completed\\\",\\\"required\\\":true,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"client_first_name\\\",\\\"required\\\":true,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"client_last_name\\\",\\\"required\\\":true,\\\"length\\\":100,\\\"type\\\":null},{\\\"field_name\\\":\\\"client_email\\\",\\\"required\\\":true,\\\"length\\\":100,\\\"type\\\":\\\"email\\\"},{\\\"field_name\\\":\\\"crm_type\\\",\\\"required\\\":true,\\\"length\\\":10,\\\"type\\\":null},{\\\"field_name\\\":\\\"contact_number\\\",\\\"required\\\":false,\\\"length\\\":100,\\\"type\\\":null}]}\"}";
        //    //if (postString == null)
        //    //{
        //    //    return Content("Failure", "application/json");
        //    //}
        //    //string msg = _Deals.SaveDeal(postString);
        //    //try
        //    //{
        //    //    return Content(msg, "application/json");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //    return NotFound();
        //    //}
        //}

    }
}