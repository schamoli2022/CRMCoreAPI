using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceModels
{
    public class IdentityClaimsPro : ClaimsPrincipal
    {
        public string client_id { get; set; }
        public string usertype { get; set; }
        public long CompanyId { get; set; }
        public string subId { get; set; }

        public long UserId { get; set; }
        public IdentityClaimsPro(ClaimsPrincipal principal)
            : base(principal)
        {


            var hasClaimUser = principal.Claims.Select(c => new { c.Type, c.Value }).ToArray();
            foreach (var lst in hasClaimUser)
            {
                if (lst.Type == "client_id")
                {
                    client_id = lst.Value;
                }
                if (lst.Type == "companyId")
                {
                    CompanyId = Convert.ToInt64(lst.Value);
                }
                if (lst.Type == "usertype")
                {
                    usertype = lst.Value;
                }
                if (lst.Type == "sub")
                {
                    UserId = Convert.ToInt64(lst.Value);
                }
            }
        }



    }
}
