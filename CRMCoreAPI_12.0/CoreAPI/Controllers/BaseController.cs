using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMCoreAPI.Filters;
using CRMCoreAPI.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMCoreAPI.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        public long CompanyId { get; set; }
        public long UserId { get; set; }

        public string ContainerName { get; set; }

        protected BaseController(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext.User.GetUserId();
            CompanyId = httpContextAccessor.HttpContext.User.GetCompanyId();
            ContainerName = httpContextAccessor.HttpContext.User.GetContainerName();
        }
    }
}