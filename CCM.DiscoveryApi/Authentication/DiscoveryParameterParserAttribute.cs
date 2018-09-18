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