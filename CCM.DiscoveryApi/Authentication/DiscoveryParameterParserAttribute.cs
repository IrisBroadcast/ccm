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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using CCM.DiscoveryApi.Models;
using CCM.WebCommon.Authentication;
using NLog;

namespace CCM.DiscoveryApi.Authentication
{
    /// <summary>
    /// Checks that the request contains valid SR Discovery parameters 
    /// </summary>
    public class DiscoveryParameterParserAttribute : Attribute, IAuthenticationFilter
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        // These parameters are not filter parameters
        private readonly IEnumerable<string> _nonFilterKeys = new List<string> {"username", "pwdhash", "caller", "callee", "includeCodecsInCall"}.Select(i => i.ToLower());

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
                
                // Get SR Discovery parameters
                var parameters = GetSrDiscoveryParameters(formData);
                request.Properties.Add("SRDiscoveryParameters", parameters);

                if (log.IsDebugEnabled)
                {
                    log.Debug(
                        "Request to {0} params. Caller:'{1}' Callee:'{2}' Filters: {3}",
                        request.RequestUri.OriginalString,
                        string.IsNullOrEmpty(parameters.Caller) ? "<missing>" : parameters.Caller,
                        string.IsNullOrEmpty(parameters.Callee) ? "<missing>" : parameters.Callee,
                        string.Join(", ", parameters.Filters.Select(f => $"{f.Key}={f.Value}")) );
                }
                

            }
            catch (Exception)
            {
                context.ErrorResult = new InternalServerErrorResult(request);
            }
        }

        private SrDiscoveryParameters GetSrDiscoveryParameters(NameValueCollection formData)
        {
            IList<KeyValuePair<string, string>> filters = formData.AllKeys
                .Where(k => !_nonFilterKeys.Contains(k.ToLower()))
                .Select(key => new KeyValuePair<string, string>(key, formData[key]))
                .ToList();

            bool.TryParse(formData["includeCodecsInCall"], out var includeCodecsInCall);

            return new SrDiscoveryParameters
            {
                Caller = formData["caller"] ?? string.Empty,
                Callee = formData["callee"] ?? string.Empty,
                IncludeCodecsInCall = includeCodecsInCall,
                Filters = filters
            };
        }
        
        public virtual Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

    }
}
