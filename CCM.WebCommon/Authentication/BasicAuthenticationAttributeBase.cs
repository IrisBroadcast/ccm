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
