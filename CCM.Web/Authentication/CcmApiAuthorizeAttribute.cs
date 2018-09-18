using System.Reflection;
using System.Web.Http.Controllers;
using NLog;

namespace CCM.Web.Authentication
{
    public class CcmApiAuthorizeAttribute : ApiAuthorizeAttribute
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            log.Info("Authorization");

            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                log.Info("Authorization failed because user is not authenticated");
                return false;
            }

            log.Info("Attribute roles: {0}", Roles);

            if (Roles.Length > 0)
            {
                var roles = Roles.Split(',');
                foreach (var role in roles)
                {
                    if (actionContext.RequestContext.Principal.IsInRole(role.Trim()))
                    {
                        log.Info("User {0} is in role {1}", actionContext.RequestContext.Principal.Identity.Name, role);
                        return true;
                    }
                    else
                    {
                        log.Info("User {0} is NOT in role {1}", actionContext.RequestContext.Principal.Identity.Name, role);
                    }
                }
                return false;
            }

            return true;
        }

        
     
    }
}