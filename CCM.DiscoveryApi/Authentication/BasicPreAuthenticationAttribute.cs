using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using CCM.WebCommon.Authentication;

namespace CCM.DiscoveryApi.Authentication
{
    public class BasicPreAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        // Used by Discovery v2
        // Performs preauthentication by checking that request contains basic authentication credentials.
        // Actual user authentication is deferred to CCM web api.
        
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(), AuthenticationTypes.Basic));
            return await Task.FromResult(claimsPrincipal);
        }
    }
}