using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Repositories;
using CCM.WebCommon.Authentication;
using LazyCache;

namespace CCM.Web.Infrastructure
{
    public class SipAccountBasicAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        // Basic Authentication with SIP account

        // Due to earlier intermittent errors during many simultanious API calls with authentication we're not using Ninject dependency injection here
        // The error occured because the same Ninject created instance were reused between calls. Possibly caused by ASP.NET reusing attributs/filters.
        public ISipAccountRepository SipAccountRepository => new SipAccountRepository(new CachingService());
        //[Inject]
        //public ISipAccountRepository SipAccountRepository { get; set; }

        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await SipAccountRepository.AuthenticateAsync(userName, password))
            {
                return null;
            }

            List<Claim> claims = new List<Claim> {new Claim(ClaimTypes.Name, userName)};
            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationTypes.Basic);

            return new ClaimsPrincipal(identity);
        }
    }
}