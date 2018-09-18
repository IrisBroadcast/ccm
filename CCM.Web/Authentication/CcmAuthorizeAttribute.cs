using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCM.Web.Authentication
{
    public class CcmAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (Roles.Length > 0)
            {
                return Roles.Split(',').Any(role => httpContext.User.IsInRole(role.Trim()));
            }

            return true;
        }
    }
}