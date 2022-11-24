using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRMCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService _AttachmentService;

        public AttachmentController(IAttachmentService AttachmentService, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _AttachmentService = AttachmentService;
        }

        [HttpGet]
        [Route("AttachmentListing")]
        public async Task<IActionResult> AttachmentListing(int id, string moduleCode, string subModuleCode)
        {
            try
            {
                var expenseData = _AttachmentService.GetAttachment(id, CompanyId, UserId, moduleCode, subModuleCode);
                //var sqlMetaData = JsonConvert.SerializeObject(expenseData);
                return Content(expenseData, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}
