using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using Microsoft.Extensions.Configuration;
using CRMCoreAPI.Common;

namespace CRMCoreAPI.Controllers
{
    [Route("api/Form")]
    [ApiController]
    public class CommonController : BaseController
    {
        private readonly ICommonService _services;
        private IConfiguration configuration;

        public CommonController(ICommonService services, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _services = services;
            configuration = _configuration;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="id"></param>
       /// <param name="moduleName"></param>
       /// <param name="type"></param>
       /// <param name="search"></param>
       /// <param name="refId"></param>
       /// <returns></returns>
        [HttpGet]
        [Route("GetDDLData")]
        public async Task<IActionResult> GetDDLData(long? id, string moduleName, string type, string search, long? refId)
        {
            string msg = _services.GetDDLData(id, CompanyId, UserId, moduleName, type, search, refId);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleName"></param>
        /// <param name="type"></param>
        /// <param name="search"></param>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDDLDataBySubModule")]
        public async Task<IActionResult> GetDDLDataBySubModule(long? id, string moduleName, string subModuleName, string type, string search, long? refId)
        {
            string msg = _services.GetDDLDataBySubModule(id, CompanyId, UserId, moduleName, subModuleName, type, search, refId);
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

        [HttpGet]
        [Route("GetEmailData")]
        public async Task<IActionResult> GetEmailData(long? id)
        {
            try
            {
                //UploadFile();
                var expenseData = _services.GetEmailData(id);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetEmailLog")]
        public async Task<IActionResult> GetEmailLog(long? id)
        {
            try
            {
                //UploadFile();
                var expenseData = _services.GetEmailLogById(id, CompanyId, UserId);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("GetDDLCommunicationModeData")]
        public async Task<IActionResult> GetDDLCommunicationModeData(long? id, string moduleName, string type, string search)
        {
            string msg = _services.GetDDLCommunicationModeData(id, CompanyId, UserId, moduleName, type, search);
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
        [HttpPost]
        [Route("AddEmailTemplate")]
        public async Task<IActionResult> AddEmailTemplate([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.AddEmailTemplate(postString, CompanyId, UserId);
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

        [HttpGet]
        [Route("Values")]

        public ActionResult <IEnumerable<string>> GetTestValues()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        ///  Common Delete method to delete the data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CommonDelete")]
        public async Task<IActionResult> CommonDeleteData([FromBody] string postString)
        {

            string msg = _services.CommonDeleteData(postString, CompanyId, UserId);
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
        /// 
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmailDetail")]
        public async Task<IActionResult> GetEmailDetail(string ids)
        {

            string msg = _services.GetEmailById(ids, CompanyId, UserId);
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


        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] string postString)
        {

            string msg = _services.SendEmailData(postString, CompanyId, UserId);
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

        [HttpGet]
        [Route("EmailListing")]
        public async Task<IActionResult> EmailListing(long? id, string search, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            try
            {

                var expenseData = _services.GetEmails(id, search, CompanyId, UserId, refId, moduleName, subModuleCode, sortBy, sortExp, pageSize, pageNumber);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                // UploadFile();
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("ScheduleListing")]
        public async Task<IActionResult> ScheduleListing(long? id, string search, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            try
            {

                var expenseData = _services.GetScheduleEmails(id, search, CompanyId, UserId, refId, moduleName, subModuleCode, sortBy, sortExp, pageSize, pageNumber);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                // UploadFile();
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("TemplateListing")]
        public async Task<IActionResult> TemplateListing(long? id, string search, long? refId, string moduleName, string subModuleCode, string sortBy, string sortExp, int? pageSize, int? pageNumber)
        {
            try
            {

                var expenseData = _services.GetEmailTemplates(id, search, CompanyId, UserId, refId, moduleName, subModuleCode, sortBy, sortExp, pageSize, pageNumber);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                // UploadFile();
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpPost, Route("UploadFile")]
        public async Task<IActionResult> UploadFileAsBlob([FromBody] string postString)
        {
            try
            {
                dynamic sys = JsonConvert.DeserializeObject(postString);
                dynamic data = null;
                foreach (var attr in sys)
                {
                    data = Convert.ToString(attr.Value);
                }

                sys = JsonConvert.DeserializeObject(data);

                string fileName = "";
                string mimeType = "";
                long id = 0;
                string base64String = "";
                string moduleName = "";
                string subModuleCode = "";
                string contentType = "";
                string containerName = "";
                string extension = "";

                foreach (var attr in sys)
                {
                    fileName = Convert.ToString(JObject.Parse(Convert.ToString(attr))["fileName"]);
                    mimeType = Convert.ToString(JObject.Parse(Convert.ToString(attr))["attachement_mime"]);
                    var jobjectMimieType = JsonConvert.DeserializeObject(mimeType);
                    contentType = Convert.ToString(JObject.Parse(Convert.ToString(jobjectMimieType))["ContentType"]);
                    extension = Convert.ToString(JObject.Parse(Convert.ToString(jobjectMimieType))["Extension"]);
                    id = Convert.ToInt64(JObject.Parse(Convert.ToString(attr))["id"]);
                    base64String = Convert.ToString(JObject.Parse(Convert.ToString(attr))["base64String"]);
                    moduleName = Convert.ToString(JObject.Parse(Convert.ToString(attr))["moduleName"]);
                    subModuleCode = Convert.ToString(JObject.Parse(Convert.ToString(attr))["subModuleCode"]);
                    containerName = Convert.ToString(JObject.Parse(Convert.ToString(attr))["containerName"]);
                }
                string accountName = configuration.GetSection("AzureStorage").GetSection("AccountName").Value;
                string accountKey = configuration.GetSection("AzureStorage").GetSection("AccountKey").Value;

                var storageCredentials = new StorageCredentials(accountName, accountKey);
                var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                var container = cloudBlobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                byte[] imageBytes = Convert.FromBase64String(base64String);

                //upload a file
                string blobPath = "files/" + moduleName + "/" + subModuleCode + "/" + UserId + "/" + id + "/" + Guid.NewGuid().ToString() + "." + extension;
                var newBlob = container.GetBlockBlobReference(blobPath);
                var path = newBlob.StorageUri;
                newBlob.Properties.ContentType = contentType;
                //await newBlob.SetPropertiesAsync();
                //await newBlob.UploadFromFileAsync(@"path\myfile.png");
                await newBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

                // download a file

                //var newBlobs = container.GetBlockBlobReference("cg-husi-190559");
                // await newBlobs.DownloadToFileAsync("path/myfile.png", FileMode.Create);
                fileName = Path.GetFileNameWithoutExtension(fileName);
                List<string> ImagesExt = new List<string> { "JPG", "JPEG", "BMP", "GIF", "PNG" };
                string ThumbblobPath = string.Empty;
                if (ImagesExt.IndexOf(extension.ToUpper()) > -1) {
                    Stream stream = new MemoryStream(imageBytes);

                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                    System.Drawing.Image thumb = img.GetThumbnailImage(200, 200, () => false, IntPtr.Zero);

                    var strm = new MemoryStream();
                    thumb.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                    byte[] thumbBytes = strm.ToArray();

                    ThumbblobPath = "files/" + moduleName + "/" + subModuleCode + "/" + UserId + "/" + id + "/" + Guid.NewGuid().ToString() + "_thumb." + extension;
                    var thumbNewBlob = container.GetBlockBlobReference(ThumbblobPath);
                    var thumbPath = thumbNewBlob.StorageUri;
                    thumbNewBlob.Properties.ContentType = contentType;
                    //await newBlob.SetPropertiesAsync();
                    //await newBlob.UploadFromFileAsync(@"path\myfile.png");
                    await thumbNewBlob.UploadFromByteArrayAsync(thumbBytes, 0, thumbBytes.Length);
                }

               var result= _services.SaveAttachment(blobPath, mimeType, id, moduleName, subModuleCode, fileName, UserId, CompanyId, ThumbblobPath);

                //return newBlob?.Uri.ToString();
                List<ListString> errorMessage = new List<ListString>();

                if (result== "Exist")
                {
                    errorMessage.Add(new ListString()
                    {
                        Status = "Exist",
                        Name = newBlob?.Uri.ToString()
                    });
                    return Content(JsonConvert.SerializeObject(errorMessage), "application/json");
                }
                errorMessage.Add(new ListString()
                { 
                    Status = "Success",
                    Name = newBlob?.Uri.ToString()
                });
                return Content(JsonConvert.SerializeObject(errorMessage), "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains("invalid"))
                {
                    List<ListString> errorMessage = new List<ListString>();
                    errorMessage.Add(new ListString()
                    {
                        Status = "Failure"
                    });
                    return Content(JsonConvert.SerializeObject(errorMessage), "application/json");
                }
                else
                {
                    return NotFound();
                }
            }
        }
        [HttpPost, Route("DownloadAttachment")]
        public async Task<IActionResult> DownloadAttachment(string path, string fileName, string contentType)
        {
            var net = new System.Net.WebClient();
            var data = net.DownloadData(path);
            var content = new System.IO.MemoryStream(data);

            return File(content, contentType, fileName);
        }

        [HttpGet, Route("TimelineListing")]
        public async Task<IActionResult> TimelineListing(long id, string subModuleCode)
        {
            try
            {
                var expenseData = _services.GetTimelineData(id, CompanyId, UserId, subModuleCode);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost, Route("SaveTax")]
        public async Task<IActionResult> SaveTax([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveTax(postString, CompanyId, UserId);
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
        /// Get Search criteria list for a user with resptect to module and sub module
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet, Route("SearchCriteriaListing")]
        public async Task<IActionResult> SearchCriteriaListing(string moduleName, string subModuleCode)
        {
            try
            {
                var returnData = _services.GetSearchCriteriaListing(CompanyId, UserId, moduleName, subModuleCode);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Get field name on basis of module and submodule
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="reqFrom"></param>
        /// <returns></returns>
        [HttpGet, Route("GetFieldName")]
        public async Task<IActionResult> GetFieldName(string moduleName, string subModuleCode, string reqFrom = null)
        {
            string msg = _services.GetFieldName(CompanyId, UserId, moduleName, subModuleCode, reqFrom);
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
        /// Get operator list with respet to data type
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [HttpGet, Route("GetOperator")]
        public async Task<IActionResult> GetOperatorList(string dataType, long? customField)
        {
            string msg = _services.GetOperatorList(CompanyId, UserId, dataType, customField);
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
        /// Save search filter data with resptect to user id to search the data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveFilter")]
        public async Task<IActionResult> SaveSearchFilter([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveSearchFilter(postString, CompanyId, UserId);
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
        /// get the search criteria by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        [HttpGet, Route("SearchCriteriaById")]
        public async Task<IActionResult> GetSearchCriteriaById(long id, string moduleName, string subModuleCode, int? isDelete)
        {
            try
            {
                var returnData = _services.GetSearchCriteriaById(id, CompanyId, UserId, moduleName, subModuleCode, isDelete);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate(string name, string type, long id = 0, long? refid = null)
        {
            try
            {
                var returnData = _services.GetExistOrNot(CompanyId, name, type, id, refid);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("EncryptPassword")]
        public async Task<IActionResult> EncryptPassword(string password)
        {
            try
            {
                var returnData = _services.GetEncryptedPassword(CompanyId, password);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetUnifiedProjectListing")]
        public async Task<IActionResult> GetUnifiedProjectManagementListing(string mobileNum)
        {
            try
            {
                var returnData = _services.GetUnifiedProjectManagementListing(mobileNum, CompanyId, UserId);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpGet, Route("GetUnifiedTicketingListing")]
        public async Task<IActionResult> GetUnifiedTicketingListing(string mobileNum)
        {
            try
            {
                var returnData = _services.GetUnifiedTicketingListing(mobileNum, CompanyId, UserId);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("GetCallLogsListing")]
        public async Task<IActionResult> GetCallLogsListing(long id, string subModuleCode)
        {
            try
            {
                var returnData = _services.GetCallLogsListing(id, CompanyId, UserId, subModuleCode);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveIndustryType")]
        public async Task<IActionResult> SaveIndustryType([FromBody] string postString)
        {

            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveIndustryType(postString, CompanyId, UserId);
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

        [HttpPost]
        [Route("SaveConfiguringJsonData")]
        public async Task<IActionResult> SaveConfiguringJsonData([FromBody] string postString)
        {

            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveConfiguringJsonData(postString, CompanyId, UserId);
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

        [HttpGet]
        [Route("GetConfigNumberJsonData")]
        public async Task<IActionResult> GetConfigNumberJsonData(long? id)
        {
            try
            {
                var configdata = _services.GetConfigNumberJsonData(id, CompanyId);
                return Content(configdata, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }


        /// <summary>
        /// Save rule filter resptect to module and sub-module id to search the duplicate data
        /// </summary>
        /// <param name="postString"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveRuleFilter")]
        public async Task<IActionResult> SaveRuleFilter([FromBody] string postString)
        {
            if (postString == null)
            {
                return Content("Failure", "application/json");
            }
            string msg = _services.SaveRuleFilter(postString, CompanyId, UserId);
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
        /// Get the rule criteria by module name and sub module code
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet, Route("RuleCriteria")]
        public async Task<IActionResult> GetRuleCriteria(string moduleName, string subModuleCode)
        {
            try
            {
                var returnData = _services.GetRuleCriteria(CompanyId, UserId, moduleName, subModuleCode);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Get the duplicate records by module name and sub module code
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleName"></param>
        /// <param name="subModuleCode"></param>
        /// <returns></returns>
        [HttpGet, Route("GetDuplicateLeads")]
        public async Task<IActionResult> GetDuplicateLeads(string moduleName, string subModuleCode,string ruleCondition)
        {
            try
            {
                var returnData = _services.GetDuplicateLeads(CompanyId, UserId, moduleName, subModuleCode, ruleCondition);
                return Content(returnData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

    }
}
