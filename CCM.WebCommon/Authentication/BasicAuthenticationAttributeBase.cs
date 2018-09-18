using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using NLog;

namespace CCM.WebCommon.Authentication
{
    /// <remarks>
    /// Based on code from 
    /// http://www.asp.net/web-api/overview/security/authentication-filters
    /// </remarks>>
    public abstract class BasicAuthenticationAttributeBase : Attribute, IAuthenticationFilter
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        public bool AllowMultiple => false;


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                log.Debug("No authentication header in request for {0}", request.RequestUri);
                return;
            }

            if (authorization.Scheme != AuthenticationSchemes.Basic.ToString())
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                log.Debug("Not a Basic authentication header in request for {0}", request.RequestUri);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                log.Debug("Missing authentication credentials in request for {0}", request.RequestUri);
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request, HttpStatusCode.BadRequest);
                return;
            }

            Credentials credentials = BasicAuthenticationHelper.ParseCredentials(authorization.Parameter);

            if (credentials == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                log.Debug("No username and password in request for {0}", request.RequestUri);
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request, HttpStatusCode.BadRequest);
                return;
            }

            try
            {
                IPrincipal principal = await AuthenticateAsync(credentials.Username, credentials.Password, cancellationToken);

                if (principal == null)
                {
                    // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                    log.Debug("Invalid username or password in request for {0}", request.RequestUri);
                    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                    return;
                }

                // Authentication succeeded.
                context.Principal = principal;
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error in BasicAuthenticationAttribute on request to {request.RequestUri}");
                context.ErrorResult = new InternalServerErrorResult(request);
            }
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken);


        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}