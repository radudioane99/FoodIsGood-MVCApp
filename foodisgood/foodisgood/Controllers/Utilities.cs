using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace foodisgood
{
    public static class Utilities
    {
        public static string GetFirstName(this System.Security.Principal.IPrincipal usr)
        {
            var firstNameClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FirstName");
            if (firstNameClaim != null)
            {
                return firstNameClaim.Value;
            }
            return "";
        }
    }
}