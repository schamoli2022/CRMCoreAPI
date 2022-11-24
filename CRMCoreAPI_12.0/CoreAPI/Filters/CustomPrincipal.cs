using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMCoreAPI.Filters
{
    public class CustomPrincipal : ClaimsPrincipal
    {
        public CustomPrincipal(ClaimsPrincipal principal) : base(principal)
        { }
        public CustomPrincipal(ClaimsIdentity identity) : base(identity)
        { }
        public static new CustomPrincipal Current
        {
            get
            {
                return new CustomPrincipal(ClaimsPrincipal.Current);
            }
        }
        public override bool IsInRole(string role)
        {

            return base.IsInRole(role); //ClaimsPrincipal.Current.IsInRole(role);

        }
    }
}
