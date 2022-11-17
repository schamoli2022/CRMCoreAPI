using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMCoreAPI.Filters
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.ToLower().StartsWith("bearer"))
            {
                //Extract credentials
                // string idToken = authHeader.Substring("Bearer ".Length).Trim();
                var idToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(idToken))
                {
                    //var token = new JwtSecurityToken(idToken); var claims1 = token.Claims;
                    var handler = new JwtSecurityTokenHandler();
                    var token1 = handler.ReadJwtToken(idToken);
                    Claim[] claims = LoadClaimsForUser(token1, idToken);
                    var id = new ClaimsIdentity(claims, "ApplicationCookie");
                    context.User = new CustomPrincipal(id);
                }
                await _next.Invoke(context);

            }
            else if (context != null && context.Request != null && context.Request.Path != null && (context.Request.Path.Value.Contains("/Login/Get")
             || context.Request.Path.Value.Contains("/health")))
            {
                await _next.Invoke(context);
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
        private Claim[] LoadClaimsForUser(JwtSecurityToken user, string accesstoken)
        {
            var companyId = user.Claims.First(claim => claim.Type == "companyId").Value;
            var companyName = user.Claims.First(claim => claim.Type == "companyName").Value;
            var containerName = user.Claims.First(claim => claim.Type == "containerName").Value;
            var id = user.Claims.First(claim => claim.Type == "id").Value;
            var isEnableLogin = user.Claims.First(claim => claim.Type == "isEnableLogin").Value;
            var loginStatus = user.Claims.First(claim => claim.Type == "loginStatus").Value;
            var activeLicenseCount = user.Claims.First(claim => claim.Type == "activeLicenseCount").Value;
            var workingLanguage = user.Claims.First(claim => claim.Type == "workingLanguage").Value;
            var timeFormat = user.Claims.First(claim => claim.Type == "timeFormat").Value;
            var gender = user.Claims.First(claim => claim.Type == "gender").Value;
            var roles = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            var role = "";
            if (roles != null)
            {
                role = roles.Value;
            }
            else
            {
                roles = user.Claims.FirstOrDefault(claim => claim.Type == "role");
                if (roles != null)
                {
                    role = roles.Value;
                }
            }

            var names = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
            var name = "";
            if (names != null)
            {
                name = names.Value;
            }
            else
            {
                names = user.Claims.FirstOrDefault(claim => claim.Type == "UserName");
                if (names != null)
                {
                    name = names.Value;
                }
            }

            var usertype = user.Claims.First(claim => claim.Type == "usertype").Value;
            var initialSetup = user.Claims.First(claim => claim.Type == "initialSetup").Value;


            var emails = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            var email = "";
            if (emails != null)
            {
                email = emails.Value;
            }
            else
            {
                emails = user.Claims.FirstOrDefault(claim => claim.Type == "email");
                if (emails != null)
                {
                    email = emails.Value;
                }
            }

            var menutype = user.Claims.First(claim => claim.Type == "menutype").Value;
            var timezone = user.Claims.First(claim => claim.Type == "timezone").Value;
            var theme = user.Claims.First(claim => claim.Type == "theme").Value;
            var companylogo = user.Claims.First(claim => claim.Type == "companylogo").Value;
            var avatar = user.Claims.First(claim => claim.Type == "avatar").Value;
            var dateFormat = user.Claims.First(claim => claim.Type == "dateFormat").Value;
            var IsInTrial = user.Claims.First(claim => claim.Type == "IsInTrial").Value;
            var Currency = user.Claims.First(claim => claim.Type == "Currency").Value;
            var PageSize = user.Claims.First(claim => claim.Type == "PageSize").Value;
            var claims = new Claim[]
            {
               new Claim(ClaimTypes.Email, email),
               new Claim(ClaimTypes.Name, name),
               new Claim(ClaimTypes.Role, role),
               new Claim("UserType", usertype),
               new Claim("AcessToken",accesstoken),// string.Format("Bearer {0}", accesstoken)),
               new Claim("gender", gender),
                new Claim("companyId", companyId),
               new Claim("companyName", companyName),
                new Claim("containerName", containerName),
               new Claim("loginStatus", loginStatus),
                new Claim("isEnableLogin", isEnableLogin),
                  new Claim("activeLicenseCount", activeLicenseCount),
                    new Claim("workingLanguage", workingLanguage),
                    new Claim("TimeFormat", timeFormat),
                     new Claim("initialSetup", initialSetup),
                      new Claim("menutype", menutype),
                      new Claim("timezone", timezone),
               new Claim("theme", theme),
                new Claim("avatar", avatar),
                 new Claim("companylogo", companylogo),
                 new Claim("dateFormat", dateFormat),
                  new Claim("IsInTrial", IsInTrial),
                   new Claim("Currency", Currency),
                     new Claim("PageSize", PageSize),
               new Claim("Id", id),
        };
            return claims;
        }
    }
}
