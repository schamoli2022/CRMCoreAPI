using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CRMCoreAPI.Filters
{
    public static class PrincipalExtension
    {
        public static long GetUserId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, CustomClaimTypes.Id)?.Value;
            if (string.IsNullOrEmpty(value)) return default(long);

            long result;
            return long.TryParse(value, out result) ? result : default(long);
        }
        public static long GetCompanyId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, CustomClaimTypes.CompanyId)?.Value;
            if (string.IsNullOrEmpty(value)) return 0;

            long result;
            return long.TryParse(value, out result) ? result : 0;
        }
        public static long GetOrgCompanyId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "OrgCompanyId")?.Value;
            if (string.IsNullOrEmpty(value)) return 0;

            long result;
            return long.TryParse(value, out result) ? result : 0;
        }

        public static int GetLoginStatus(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "loginStatus")?.Value;
            if (string.IsNullOrEmpty(value)) return 0;

            int result;
            return int.TryParse(value, out result) ? result : 0;
        }
        public static int PageSize(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "PageSize")?.Value;
            if (string.IsNullOrEmpty(value)) return 0;

            int result;
            return int.TryParse(value, out result) ? result : 0;
        }
        public static bool GetIsEnableLogin(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "isEnableLogin")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool IsInTrial(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "IsInTrial")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool GetInitialSetup(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "initialSetup")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static void SetInitialSetup(this IPrincipal user, bool value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("initialSetup", value.ToString()));
        }
        public static void SetTrialMessage(this IPrincipal user, int value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("trialMessage", value.ToString()));
        }
        public static bool GetIsDefault(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "isDefault")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool GetIsSearchBarOff(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "isSearchBarOff")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool GetIsMenuSliderOff(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "isMenuSliderOff")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool Reset_Password(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "reset_Password")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool TimeZoneStatus(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "TimeZoneStatus")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool IsWithCC(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "IsWithCC")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool DoNotShowDirtyCheck(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "DoNotShowDirtyCheck")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }

        public static bool EnableMyDashboard(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "EnableMyDashboard")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }
        public static bool FlagNotificationCheck(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, "FlagNotificationCheck")?.Value;
            if (string.IsNullOrEmpty(value)) return false;

            bool result;
            return bool.TryParse(value, out result) ? result : false;
        }

        public static string CompanyStatusCode(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "companyStatusCode")?.Value;
        }
        public static string IsVendor(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "IsVendor")?.Value;
        }

        public static string Currency(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "Currency")?.Value;
        }

        public static string IdToken(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "AcessToken")?.Value;
        }
        public static string TimeZone(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "timezone")?.Value;
        }
        public static string Theme(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "theme")?.Value;
        }
        public static string DateFormat(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "dateFormat")?.Value;
        }

        public static string MenuType(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "menutype")?.Value;
        }
        public static string Gender(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "Gender")?.Value;
        }
        public static string IsClientCA(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "IsClientCA")?.Value;
        }

        public static string Avatar(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "avatar")?.Value;
        }
        public static void SetMenuType(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("menutype", value.ToString()));
        }
        public static void SetCompanyStatusCode(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("companyStatusCode", value.ToString()));
        }


        public static void SetCompanyId(this IPrincipal user, long value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("companyId", value.ToString()));
        }

        public static void SetOrgCompanyId(this IPrincipal user, long value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("OrgCompanyId", value.ToString()));
        }

        public static void SetCurrency(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("Currency", value.ToString()));

        }
        public static void SetisDefault(this IPrincipal user, bool value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("isDefault", value.ToString()));

        }
        public static void SetTimeZoneStatus(this IPrincipal user, bool value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("TimeZoneStatus", value.ToString()));

        }


        public static string CompanyLogo(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "companylogo")?.Value;
        }
        public static void SetCompanyLogo(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("companylogo", value.ToString()));
        }
        public static string GetEmail(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Email)?.Value;
        }
        public static string GetTrialMessage(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "trialMessage")?.Value;
        }
        public static string GetWorkingLanguage(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "workingLanguage")?.Value;
        }
        public static void SetWorkingLanguage(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("workingLanguage", value.ToString()));
        }
        public static void SetUserType(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("usertype", value.ToString()));
        }

        public static string GetCompanyName(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "companyName")?.Value;
        }
        public static void SetCompanyName(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("companyName", value.ToString()));
        }
        public static string GetContainerName(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "containerName")?.Value;
        }
        public static void SetContainerName(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim("containerName", value.ToString()));
        }
        public static void SetRole(this IPrincipal user, string value)
        {
            var CP = ClaimsPrincipal.Current.Identities.First();
            CP.AddClaim(new Claim(ClaimTypes.Role, value.ToString()));
        }

        public static string GetUserName(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Name)?.Value;
        }

        public static string GetTimeFormat(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "timeFormat")?.Value;
        }

        public static string GetRoleId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Role)?.Value;
        }

        public static string GetUserType(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, "usertype")?.Value;
        }
        public static Claim FindClaim(this IPrincipal user, string claimType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));


            var claimsPrincipal = ClaimsPrincipal.Current;
            // user as ClaimsPrincipal;
            // var customPrincipal = new CustomPrincipal(claimsPrincipal);
            //customPrincipal.curr
            if (claimsPrincipal == null)
            {
                claimsPrincipal = user as ClaimsPrincipal;
            }
            return claimsPrincipal?.FindFirst(claimType);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class CustomClaimTypes
    {
        public const string Id = "Id";
        public const string CompanyId = "CompanyId";
        public const string UserType = "UserType";
        public const string TimeFormat = "TimeFormat";
    }
}
