/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
