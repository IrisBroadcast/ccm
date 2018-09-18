using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.Core.Interfaces;
using CCM.Web.Infrastructure;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    [CamelCaseControllerConfig]
    [RoutePrefix("api/discovery")]
    public class DiscoveryController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IDiscoveryService _discoveryService;

        public DiscoveryController(IDiscoveryService discoveryService)
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
