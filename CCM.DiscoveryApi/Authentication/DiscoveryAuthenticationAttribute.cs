using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using CCM.DiscoveryApi.Infrastructure;
using CCM.WebCommon.Authentication;
using NLog;

namespace CCM.DiscoveryApi.Authentication
{
    /// <summary>
    /// Performs preauthentication of a SR Discovery request
    /// 
    /// Checks that the request contains SR Discovery authentication parameters 
    /// and converts then to basic authentication HTTP header
    /// 
    /// Based on code / example from
    /// http://www.asp.net/web-api/overview/security/authentication-filters
    /// </summary>
    public class DiscoveryAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        // SIP account authentication for SR Discovery

        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public bool AllowMultiple => false;


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;

            try
            {
                if (request.Content.Headers.ContentType.MediaType != "application/x-www-form-urlencoded")
                {
                    context.ErrorResult = new AuthenticationFailureResult("Wrong content type", request, HttpStatusCode.BadRequest);
                    return;
                }

                // Make sure we're reading from beginning of stream
                Stream stream = await request.Content.ReadAsStreamAsync();
                stream.Seek(0, SeekOrigin.Begin);

                NameValueCollection formData = await request.Content.ReadAsFormDataAsync(cancellationToken);
                
                var userName = formData["username"];
                var pwdhash = formData["pwdhash"];

                if (userName == null || pwdhash == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing user name or password", request, HttpStatusCode.BadRequest);
                    return;
                }

                // Convert SR Discovery special authentication to Basic Authentication
                var authString = AuthenticationHelper.GetAuthString(userName, pwdhash);
                request.Headers.Authorization = new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), authString);

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationTypes.Basic));

                if (log.IsDebugEnabled)
                {
                    log.Debug(
                        "Request to {0} params. User name:'{1}' Password:{2}",
                        request.RequestUri.OriginalString,
                        userName,
                        string.IsNullOrEmpty(pwdhash) ? "<missing>" : "********"
                        );
                }
            }
            catch (Exception)
            {
                context.ErrorResult = new InternalServerErrorResult(request);
            }
        }
        
        public virtual Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

    }
}