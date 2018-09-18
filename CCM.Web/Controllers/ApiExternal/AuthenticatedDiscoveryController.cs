using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.Core.Interfaces;
using CCM.Web.Infrastructure;

namespace CCM.Web.Controllers.ApiExternal
{
    [SipAccountBasicAuthentication]
    [Authorize]
    [CamelCaseControllerConfig]
    [RoutePrefix("api/authenticateddiscovery")]
    public class AuthenticatedDiscoveryController : ApiController
    {
        private readonly IDiscoveryService _discoveryService;

        public AuthenticatedDiscoveryController(IDiscoveryService discoveryService)
        {
            _discoveryService = discoveryService;
        }

        [HttpGet]
        [Route("filters")]
        public List<FilterDto> Filters()
        {
            return _discoveryService.GetFilters();
        }

        [HttpGet]
        [Route("profiles")]
        public List<ProfileDto> Profiles()
        {
            return _discoveryService.GetProfiles();
        }

        [Route("useragents")]
        [HttpPost]
        public UserAgentsResultDto UserAgents(UserAgentSearchParamsDto searchParams)
        {
            if (searchParams == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserAgentsResultDto uaResult = _discoveryService.GetUserAgents(searchParams.Caller, searchParams.Callee, searchParams.Filters, searchParams.IncludeCodecsInCall);
            return uaResult;
        }

    }
}
