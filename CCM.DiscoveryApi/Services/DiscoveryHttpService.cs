using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.DiscoveryApi.Infrastructure;
using Newtonsoft.Json;
using NLog;

namespace CCM.DiscoveryApi.Services
{
    // Retreives discovery data via CCM's REST service.

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
                    new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") : 
                    null;

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