using CRMCoreAPI.Service;
using CRMCoreAPI.ServiceModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using CRMCoreAPI.Common;


namespace CRMCoreAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_configuration"></param>
        public InvoiceService(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="OrderSubject"></param>
        /// <param name="DealName"></param>
        /// <param name="SalesOwner"></param>
        /// <param name="LeadOwner"></param>
        /// <param name="ContactName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="AccountName"></param>
        /// <param name="leadName"></param>
        /// <param name="companyId"></param>
        /// <param name="loggedInUserId"></param>
        /// sourceId used to get sales order listing according to the mapping 
        /// with respect to sub module
        /// <param name="sourceId"></param>
        /// <param name="subModuleCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortExp"></param>
        /// <param name="isPaged"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public string GetInvoice(int? Status, string OrderSubject,string DealName,string SalesOwner, string LeadOwner, string ContactName, string dateFrom, string dateTo, string AccountName, string leadName, int? statusID, long companyId,  long loggedInUserId, int? sourceId, string subModuleCode, string sortBy, string sortExp, byte? isPaged, int? pageSize, int? pageNumber, string searchCondition)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (ContactName == "")
                    {
                    ContactName = null;
                    }
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STATUS", Status);
                    dynamicParameters.Add("@SALES_ORDER_SUBJECT", OrderSubject);
                    dynamicParameters.Add("@DEAL_NAME", DealName);
                    dynamicParameters.Add("@InvoiceOWNER", SalesOwner);
                    dynamicParameters.Add("@LEADOWNER", LeadOwner); 
                    dynamicParameters.Add("@DATEFROM", dateFrom);
                    dynamicParameters.Add("@DATETO", dateTo);
                    dynamicParameters.Add("@ACCOUNTNAME", AccountName);
                    dynamicParameters.Add("@LEADNAME", leadName);
                    dynamicParameters.Add("@STATUS_ID", statusID);
                    dynamicParameters.Add("@COMPANY_ID", companyId);
                    dynamicParameters.Add("@LOGGEDINUSERID", loggedInUserId);
                    dynamicParameters.Add("@SUB_MODULE_CODE", subModuleCode);
                    dynamicParameters.Add("@SOURCE_ID", sourceId);
                    dynamicParameters.Add("@SORT_BY", sortBy);
                    dynamicParameters.Add("@SORT_EXP", sortExp);
                    dynamicParameters.Add("@PAGESIZE", pageSize);
                    dynamicParameters.Add("@PAGENUMBER", pageNumber);
                    dynamicParameters.Add("@SEARCH_CONDITION", searchCondition);
                    //dynamicParameters.Add("@headerstring", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                    var sqlMetaData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_INVOICE_GET_LISTING", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        public string GetcustomFieldInvoice(long companyId, string moduleName, string subModuleCode, long userId)
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

        //public string SaveInvoice(string postString)
        //{
        //    List<ListString> errorMessage = new List<ListString>();
        //    dynamic sys = JsonConvert.DeserializeObject(postString);
        //    int index = 1;
        //    dynamic data = null;
        //    try
        //    {
        //        foreach (var attr in sys)
        //        {
        //            var newObj = Convert.ToString(attr.Value);

        //            dynamic sysss = JsonConvert.DeserializeObject(newObj);

        //            foreach (var name in sysss)
        //            {
        //                if (index == 1)
        //                {
        //                    data = name.Value;
        //                    break;
        //                }
        //            }
        //        }
        //        if (data != null)
        //        {
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                //string jsonS = "[" + Convert.ToString(data) + "]";
        //                dynamicParameters.Add("@json", Convert.ToString(data));
        //                var InvoiceId = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_CRM_SAVE_SALES_ORDER", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //                //dData.fieldMetadata = dynamicParameters.Get<string>("headerstring");
        //                //return "true";
        //                //if (InvoiceId != null)
        //                //{
        //                //    errorMessage.Add(new ListString()
        //                //    {
        //                //        Status = "Success"
        //                //    });
        //                //    return JsonConvert.SerializeObject(errorMessage);
        //                //}
        //                //else
        //                //{
        //                //    errorMessage.Add(new ListString()
        //                //    {
        //                //        Status = "Failure"
        //                //    });
        //                //    return JsonConvert.SerializeObject(errorMessage);
        //                //}

        //                return CommonClass.ReturnMessage(InvoiceId);
        //            }
        //        }
        //        errorMessage.Add(new ListString()
        //        {
        //            Status = "Failure"
        //        });
        //        return JsonConvert.SerializeObject(errorMessage);
        //    }
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
