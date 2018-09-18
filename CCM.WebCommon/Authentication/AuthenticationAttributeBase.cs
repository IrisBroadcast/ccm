using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using log4net;
using Ninject;

namespace CCM.WebCommon.Authentication
{
    /// <summary>
    /// http://www.asp.net/web-api/overview/security/authentication-filters
    /// </summary>
    public abstract class AuthenticationAttributeBase : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        [Inject]
        public IRadiusProvider RadiusProvider { get; set; }

        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public abstract Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken);

        protected virtual async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

#if DEBUG
            bool authenticated = (userName == ApplicationSettings.DiscoveryUsername && password == ApplicationSettings.DiscoveryPassword); // Alltid authenticerad
#else
            bool authenticated = false;
#endif

            if (!authenticated)
            {
                authenticated = RadiusProvider.Authenticate(userName, password); // TODO: Anropa asynkront
            }

            if (!authenticated) { return null; }

            // Create a ClaimsIdentity with all the claims for this user.
            Claim nameClaim = new Claim(ClaimTypes.Name, userName);
            List<Claim> claims = new List<Claim> { nameClaim };

            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationTypes.Basic);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        public virtual Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}