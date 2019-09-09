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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.Entities.Discovery;
using CCM.DiscoveryApi.Infrastructure;
using Newtonsoft.Json;
using NLog;

namespace CCM.DiscoveryApi.Services
{
    /// <summary>
    /// Retreives discovery data via CCM's REST service
    /// </summary>
    public class DiscoveryHttpService : ApiController, IDiscoveryHttpService
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Uri GetUrl(string action)
        {
            return new Uri(ApplicationSettings.CcmHost, $"api/authenticateddiscovery/{action}");
        }

        public async Task<List<FilterDto>> GetFiltersAsync(HttpRequestMessage originalRequest)
        {
            var url = GetUrl("filters");
            return await Send<List<FilterDto>>(url, HttpMethod.Get, originalRequest);
        }

        public async Task<List<ProfileDto>> GetProfilesAsync(HttpRequestMessage originalRequest)
        {
            var url = GetUrl("profiles");
            return await Send<List<ProfileDto>>(url, HttpMethod.Get, originalRequest);
        }

        public async Task<UserAgentsResultDto> GetUserAgentsAsync(UserAgentSearchParamsDto searchParams, HttpRequestMessage originalRequest)
        {
            var url = GetUrl("useragents");
            return await Send<UserAgentsResultDto>(url, HttpMethod.Post, originalRequest, searchParams);
        }

        private async Task<T> Send<T>(Uri url, HttpMethod method, HttpRequestMessage originalRequest, object data = null)
        {
            log.Debug("Getting discovery data from {0}", url);
            using (var client = new HttpClient())
            {
                HttpContent content = data != null ? 
                    new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") : null;

                var request = new HttpRequestMessage(method, url) {Content = content};
                request.Headers.Authorization = originalRequest.Headers.Authorization;

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        log.Debug("Failed to get discovery data. Response: {0} {1}", response.StatusCode, response.ReasonPhrase);
                    }
                    else
                    {
                        log.Warn("Failed to get discovery data. Response: {0} {1}", response.StatusCode, response.ReasonPhrase);
                    }
                    throw new HttpResponseException(response);
                }

                return await response.Content.ReadAsAsync<T>();
            }
        }

    }
}
