using CRMCoreAPI.ServiceModels;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TALYGEN;

namespace CRMCoreAPI.Filters
{
    //Rakesh kumar Created by CustomAuthorize Filter Dated:6-Fer-2019
    public class CustomAuthorizeFilter : AuthorizeAttribute, IAuthorizationFilter
    {

        private IConfiguration _configuration;

        private readonly IUserService _services;
        public CustomAuthorizeFilter(string controller, string action, IConfiguration configuration, IUserService service)
        {
            _configuration = configuration;
            _services = service;
            Controller = controller;
            Action = action;


        }
        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
        Privileges obj;
        IdentityClaimsPro objIdentityPro;
        public string Controller { get; set; }
        public string Action { get; set; }
        /// <summary>
        /// Overload OnAuthorization
        /// </summary>

        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var hasLicense = false;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                objIdentityPro = new IdentityClaimsPro(filterContext.HttpContext.User);
                obj = new Privileges(_configuration, _services);

                if (!obj.IsHasPermission(objIdentityPro.UserId, objIdentityPro.CompanyId, objIdentityPro.usertype, Controller, Action, ref hasLicense))
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                    return;


                }
            }

        }
    }
}
