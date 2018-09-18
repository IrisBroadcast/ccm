using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.Discovery;
using CCM.Core.Helpers;
using CCM.DiscoveryApi.Authentication;
using CCM.DiscoveryApi.Infrastructure;
using CCM.DiscoveryApi.Models;
using CCM.DiscoveryApi.Models.DiscoveryModels;
using CCM.DiscoveryApi.Services;
using NLog;

namespace CCM.DiscoveryApi.Controllers
{
    [DiscoveryControllerConfig]
    [DiscoveryAuthentication]
    [Authorize]
    public class DiscoveryController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IDiscoveryHttpService _discoveryService;

        public DiscoveryController()
        {
            _discoveryService = new DiscoveryHttpService();
        }


        [Route("~/filters")]
        [HttpPost]
        public async Task<SrDiscovery> Filters()
        {
            //using (new TimeMeasurer("Discovery Get filters"))
            {
                var filterDtos = await _discoveryService.GetFiltersAsync(Request);

                var filters = filterDtos.Select(f => new Filter
                {
                    Name = f.Name,
                    FilterOptions = f.Options.Select(fo => new FilterOption { Name = fo }).ToList()
                }).ToList();

                return new SrDiscovery { Filters = filters };
            }
        }

        [Route("~/profiles")]
        [HttpPost]
        public async Task<SrDiscovery> Profiles()
        {
            //using (new TimeMeasurer("Discovery Get profiles"))
            {
                var profileDtos = await _discoveryService.GetProfilesAsync(Request);
                var profiles = profileDtos.Select(p => new Profile { Name = p.Name, Sdp = p.Sdp }).ToList();
                return new SrDiscovery { Profiles = profiles };
            }
        }

        [Route("~/useragents")]
        [HttpPost]
        [DiscoveryParameterParser]
        public async Task<SrDiscovery> UserAgents()
        {
           var parameters = (SrDiscoveryParameters)Request.Properties["SRDiscoveryParameters"];

            var searchParams = new UserAgentSearchParamsDto
            {
                Caller = parameters.Caller,
                Callee = parameters.Callee,
                IncludeCodecsInCall = parameters.IncludeCodecsInCall,
                Filters = parameters.Filters
            };

            UserAgentsResultDto uaResult;

            //using (new TimeMeasurer("Discovery Get user agents"))
            {
                uaResult = await _discoveryService.GetUserAgentsAsync(searchParams, Request);
            }

            if (uaResult == null)
            {
                log.Info("No user agents found returned");
                return new SrDiscovery();
            }

            log.Debug("Returning {0} useragents and {1} profiles.", uaResult.UserAgents?.Count ?? 0, uaResult.Profiles?.Count ?? 0);

            var profiles = uaResult.Profiles?.Select(p => new Profile() { Name = p.Name, Sdp = p.Sdp }).ToList() ?? new List<Profile>();

            var userAgents = uaResult.UserAgents?.Select(ua => new UserAgent()
            {
                SipId = ua.SipId,
                ConnectedTo = ua.ConnectedTo,
                ProfileRec = ua.Profiles.Select(p => new UserAgentProfileRef { Name = p }).ToList(),
                MetaData = ua.MetaData.Select(m => new UserAgentMetaData() { Key = m.Key, Value = m.Value }).ToList()
            }).ToList() ?? new List<UserAgent>();

            return new SrDiscovery { UserAgents = userAgents, Profiles = profiles };
        }

    }
}