using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.DiscoveryApi.Authentication;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.Filters;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.Profiles;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.UserAgents;
using CCM.DiscoveryApi.Services;
using NLog;

namespace CCM.DiscoveryApi.Controllers
{
    [BasicPreAuthentication]
    [Authorize]
    public class DiscoveryV2Controller : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IDiscoveryHttpService _discoveryService;

        public DiscoveryV2Controller()
        {
            _discoveryService = new DiscoveryHttpService();
        }

        [HttpGet]
        [Route("~/v2/filters")]
        public async Task<List<FilterV2>> Filters()
        {
            List<FilterDto> filterDtos = await _discoveryService.GetFiltersAsync(Request);

            var filters = filterDtos.Select(f => new FilterV2
            {
                Name = f.Name,
                Options = f.Options.Select(o => new FilterOptionV2 {Name = o}).ToList()
            }).ToList();

            return filters;
        }

        [HttpGet]
        [Route("~/v2/profiles")]
        public async Task<List<ProfileDtoV2>> Profiles()
        {
            var profileDtos = await _discoveryService.GetProfilesAsync(Request);

            var profiles = profileDtos
                .Select(p => new ProfileDtoV2() { Name = p.Name, Sdp = p.Sdp })
                .ToList();

            return profiles;
        }

        [HttpPost]
        [Route("~/v2/useragents")]
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

    }
}