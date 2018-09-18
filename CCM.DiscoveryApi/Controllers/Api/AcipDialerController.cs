using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.DiscoveryApi.Infrastructure;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.Filters;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.Profiles;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.UserAgents;
using CCM.DiscoveryApi.Services;

namespace CCM.DiscoveryApi.Controllers.Api
{
    [RoutePrefix("api/acipdialer")]
    public class AcipDialerController : ApiController
    {
        private readonly IDiscoveryHttpService _discoveryService;

        public AcipDialerController()
        {
            _discoveryService = new DiscoveryHttpService();
        }

        [HttpGet]
        [Route("filters")]
        public async Task<List<FilterV2>> Filters()
        {
            AddDummyAuthenticationHeader();

            List<FilterDto> filterDtos = await _discoveryService.GetFiltersAsync(Request);

            var filters = filterDtos.Select(f => new FilterV2
            {
                Name = f.Name,
                Options = f.Options.Select(o => new FilterOptionV2 { Name = o }).ToList()
            }).ToList();

            return filters;
        }


        [HttpGet]
        [Route("profiles")]
        public async Task<List<ProfileDtoV2>> Profiles()
        {
            AddDummyAuthenticationHeader();

            var profileDtos = await _discoveryService.GetProfilesAsync(Request);

            var profiles = profileDtos
                .Select(p => new ProfileDtoV2 { Name = p.Name, Sdp = p.Sdp })
                .ToList();

            return profiles;
        }

        [HttpPost]
        [Route("useragents")]
        public async Task<UserAgentsResultV2> UserAgents(UserAgentSearchParamsV2 searchParams)
        {
            if (searchParams == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var searchParamsDto = new UserAgentSearchParamsDto
            {
                Caller = searchParams.Caller,
                Callee = searchParams.Callee,
                IncludeCodecsInCall = searchParams.IncludeCodecsInCall,
                Filters = searchParams.Filters
            };

            AddDummyAuthenticationHeader();

            UserAgentsResultDto uaResult = await _discoveryService.GetUserAgentsAsync(searchParamsDto, Request);

            var result = new UserAgentsResultV2
            {
                Profiles = uaResult.Profiles.Select(p => new ProfileDtoV2 { Name = p.Name, Sdp = p.Sdp }).ToList(),
                UserAgents = uaResult.UserAgents.Select(ua => new UserAgentDtoV2
                {
                    SipId = ua.SipId,
                    ConnectedTo = ua.ConnectedTo,
                    Profiles = ua.Profiles,
                    MetaData = ua.MetaData.Select(m => new KeyValuePairDtoV2(m.Key, m.Value)).ToList()
                }).ToList()
            };

            return result;
        }
        
        private void AddDummyAuthenticationHeader()
        {
            var authString = AuthenticationHelper.GetBasicAuthorizationString(ApplicationSettings.DiscoveryUsername,
                ApplicationSettings.DiscoveryPassword);
            Request.Headers.Add(HttpRequestHeader.Authorization.ToString(), authString);
        }

    }
}