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

using CCM.Core.Entities.Discovery;
using CCM.DiscoveryApi.Models;
using CCM.DiscoveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCM.DiscoveryApi.Models.Discovery;

namespace CCM.DiscoveryApi.Controllers
{
    [Produces("application/xml")]
    [Authorize("BasicAuthenticationDiscoveryV1")]
    public class DiscoveryController : ControllerBase
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IDiscoveryHttpService _discoveryService;

        public DiscoveryController(IDiscoveryHttpService discoveryHttpService)
        {
            _discoveryService = discoveryHttpService;
        }

        [Route("~/filters")]
        [HttpPost]
        public async Task<DiscoveryResponse> Filters()
        {
            log.Trace("Discovery API - requesting 'filters'");

            var filterDtos = await _discoveryService.GetFiltersAsync(Request);

            var filters = filterDtos.Select(f => new DiscoveryFilter
            {
                Name = f.Name,
                FilterOptions = f.Options.Select(o => new DiscoveryFilterOption { Name = o }).ToList()
            }).ToList();

            return new DiscoveryResponse { Filters = filters };
        }

        [Route("~/profiles")]
        [HttpPost]
        public async Task<DiscoveryResponse> Profiles()
        {
            log.Trace("Discovery API - requesting 'profiles'");

            var profileDtos = await _discoveryService.GetProfilesAsync(Request);

            var profiles = profileDtos
                .Select(p => new DiscoveryProfile { Name = p.Name, Sdp = p.Sdp })
                .ToList();
            return new DiscoveryResponse { Profiles = profiles };
        }

        [Route("~/useragents")]
        [HttpPost]
        public async Task<DiscoveryResponse> UserAgents([FromForm]DiscoveryUserAgentRequest srDiscoveryParameters)
        {
            if (Request.ContentType != "application/x-www-form-urlencoded")
            {
                log.Warn("Wrong content type, expecting 'application/x-www-form-urlencoded'");
            }

            var searchParams = new UserAgentSearchParamsDto
            {
                Caller = srDiscoveryParameters.Caller,
                Callee = srDiscoveryParameters.Callee,
                IncludeCodecsInCall = srDiscoveryParameters.IncludeCodecsInCall,
                Filters = srDiscoveryParameters.Filters
            };

            log.Trace("Discovery API - requesting 'useragents'", searchParams);

            UserAgentsResultDto uaResult = await _discoveryService.GetUserAgentsAsync(searchParams, Request);

            if (uaResult == null)
            {
                log.Info("No user agents returned for DiscoveryV1");
                return new DiscoveryResponse();
            }

            var profiles = uaResult.Profiles?.Select(p => new DiscoveryProfile() { Name = p.Name, Sdp = p.Sdp }).ToList() ?? new List<DiscoveryProfile>();

            var userAgents = uaResult.UserAgents?.Select(ua => new DiscoveryUserAgent()
            {
                SipId = ua.SipId,
                ConnectedTo = ua.ConnectedTo,
                ProfileRec = ua.Profiles.Select(p => new DiscoveryUserAgentProfileRef { Name = p }).ToList(),
                MetaData = ua.MetaData.Select(m => new DiscoveryUserAgentMetaData() { Key = m.Key, Value = m.Value }).ToList()
            }).ToList() ?? new List<DiscoveryUserAgent>();

            log.Debug("Discovery V1 Returning {0} useragents and {1} profiles", uaResult.UserAgents?.Count ?? 0, uaResult.Profiles?.Count ?? 0);

            return new DiscoveryResponse { UserAgents = userAgents, Profiles = profiles };
        }

    }
}
