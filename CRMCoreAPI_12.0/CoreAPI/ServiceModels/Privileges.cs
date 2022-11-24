using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace TALYGEN
{
    public  class Privileges
    {
        /// <summary>
        /// Created By Rakesh kumar \n Created On:06/02/2019 11:15:00 AM
        /// Purpose: GetUser role privileges
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        //private readonly string connectionString = null;
        private IConfiguration _configuration;
       // private UserService services;
        private  readonly IUserService _services;
        //UserService obj = new UserService(connectionString);

        /// <summary>
        /// 
        /// </summary>
        /// 

        public Privileges(IConfiguration configuration, IUserService service)
        {
          
            _configuration = configuration;
            _services = service;
            

        }



        public   List<RoleClass> GetAllRoles(long userId)
        {
            var roleList = new List<RoleClass>();
            if (userId == 0)
                return roleList;

            // roleList = obj.GetAllRoles_Privileges(userId, 0, "").ToList();
            roleList = _services.GetAllRoles_Privileges(userId, 0, "").ToList();

            return roleList;
        }

        /// <summary>
        /// Created By:Rakesh kumar
        /// Created On:06/02/2019
        /// Desc: GetUser role=client
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="company_id"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        public  List<RoleClass> GetAllRoles(long userId, long? company_id, string usertype, bool isCompanyChange = false, string controller="", string action="")
        {
            var roleList = new List<RoleClass>();
            if (userId == 0)
                return roleList;
            var rolelisting = _services.GetAllRoles_Privilege_Allowed(userId, company_id.Value, usertype, controller, action).ToList();
           
            return rolelisting;
        }
        public   bool IsHasPermission(long userId,long companyId,string userType, string controller, string action, ref bool hasLicense, bool checkAuthentication = true)
        {
            var loggedUserInfo = userId;
            var rolelist = new List<RoleClass>();
            if (userId != null)
            {
               
                rolelist = GetAllRoles(userId, companyId, userType, controller: controller, action: action);
                if (rolelist.Count>0 && rolelist[0].USER_ID == -1)
                {
                    hasLicense = false;
                }
                
                // else
                //   rolelist = GetAllRoles(userId, loggedUserInfo.CompanyId, loggedUserInfo.UserType);
            }
          

            if (rolelist.Count == 0)
                return false;

            var res = rolelist.FirstOrDefault(x => (x.CONTROLLER.Equals(controller, StringComparison.OrdinalIgnoreCase) || x.CONTROLLER.Equals(controller, StringComparison.OrdinalIgnoreCase) ) && x.ACTION.Equals(action, StringComparison.OrdinalIgnoreCase));
            if (res == null)
                return false;
            hasLicense = true;
            
            return res.IsHasPermission;

        }

        public  bool IsHasPermission(long userId, long companyId, string userType, string controller, string action,  bool checkAuthentication = true)
        {
            var hasLicense=true;
            return IsHasPermission(userId, companyId, userType, controller, action, ref hasLicense, checkAuthentication);
        }

      
    }

    /// <summary>
    /// Custom Class to contains all roles of a user
    /// </summary>
    [Serializable]
    public class RoleClass
    {
        public long USER_ID { get; set; }

        public string CONTROLLER { get; set; }

        public string ACTION{ get; set; }

        public bool IsHasPermission { get { return DEFAULT_VALUE; }  }

        public bool DEFAULT_VALUE { get; set; }
        public string ORIGINAL_CONTROLLER_NAME { get; set; }
    }
}