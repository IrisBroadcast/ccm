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
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Core.Managers
{
    public class RegisteredSipsManager : IRegisteredSipsManager
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ICallRepository _callRepository;
        private readonly IUserAgentRepository _userAgentRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IProfileGroupRepository _profileGroupRepository;

        private readonly ISettingsManager _settingsManager;

        public RegisteredSipsManager(
            IRegisteredSipRepository registeredSipRepository,
            ICallRepository callRepository,
            IUserAgentRepository userAgentRepository,
            ILocationRepository locationRepository,
            IProfileGroupRepository profileGroupRepository,
            ISettingsManager settingsManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _callRepository = callRepository;
            _userAgentRepository = userAgentRepository;
            _locationRepository = locationRepository;
            _profileGroupRepository = profileGroupRepository;

            _settingsManager = settingsManager;
        }

        public IEnumerable<RegisteredUserAgentAndProfilesDiscovery> GetRegisteredUserAgentsAndProfiles()
        {
            var sipDomain = _settingsManager.SipDomain;

            // Registered user agents
            IEnumerable<RegisteredUserAgentDiscovery> registeredUserAgentsList = _registeredSipRepository.GetRegisteredUserAgentsDiscovery();

            if (registeredUserAgentsList == null)
            {
                return null;
            }

            // User agent types and profiles for each User Agent
            IDictionary<Guid, UserAgentAndProfiles> userAgentsTypesList = _userAgentRepository.GetUserAgentsTypesAndProfiles();

            // Locations and profile groups
            IDictionary<Guid, LocationAndProfiles> locationsAndProfileGroupList = _locationRepository.GetLocationsAndProfiles();

            // Profile groups
            IReadOnlyList<ProfileGroup> profileGroupsList = _profileGroupRepository.GetAll();

            // Ongoing calls
            IReadOnlyCollection<OnGoingCall> callsList = _callRepository.GetOngoingCalls(true);

            return registeredUserAgentsList.Select(regSip =>
            {
                // The sort order is most important as it decides the order
                // of recommended profiles in the Discovery service.
                // Sorting is based on the sort order of the location.

                // Match register user agent user agent profiles
                var profilesUserAgent = Enumerable.Empty<string>();
                if (regSip.UserAgentId != null &&
                    userAgentsTypesList.TryGetValue(regSip.UserAgentId.Value, out var profilesUa))
                {
                    profilesUserAgent = profilesUa.Profiles.OrderBy(z => z.SortIndex).Select(y => y.Name); // TODO: No sort is done here...it's done earlier. trust? Is it the right sort?
                }

                // Match location profiles and add profile names to locations profile groups
                int? profilesLocationSortWeight = null;
                var profilesLocation = Enumerable.Empty<string>();
                if (regSip.LocationId != null &&
                    locationsAndProfileGroupList.TryGetValue(regSip.LocationId.Value, out var profileLoc))
                {
                    //var locMatch = profileGroupsList.FirstOrDefault(x => x.Id == profileLoc.ProfileGroupId)?.Profiles.OrderBy(z => z.SortIndex).Select(y => y.Name);
                    var locMatch = profileGroupsList.FirstOrDefault(x => x.Id == profileLoc.ProfileGroupId)?.Profiles.Select(y => y.Name);
                    if (locMatch != null)
                    {
                        profilesLocation = locMatch;
                    }

                    // Get location profile group sort weight 
                    var locMatchGroup = profileGroupsList.FirstOrDefault(x => x.Id == profileLoc.ProfileGroupId);
                    if (locMatchGroup != null)
                    {
                        profilesLocationSortWeight = locMatchGroup.GroupSortWeight;
                    }
                }

                IList<string> filteredProfiles = profilesLocation.Intersect(profilesUserAgent).ToList();

                // Call information
                var call = callsList.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;

                string inCallWithId = string.Empty;
                string inCallWithSip = string.Empty;
                string inCallWithName = string.Empty;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    inCallWithId = isFromCaller ? call.ToId : call.FromId;
                    inCallWithSip = isFromCaller ? call.ToSip : call.FromSip;
                    inCallWithName = isFromCaller ? call.ToDisplayName : call.FromDisplayName;
                }

                // Registered user agent
                var dispName = DisplayNameHelper.GetDisplayName(regSip.DisplayName, regSip.UserDisplayName,
                    string.Empty, regSip.Username, regSip.SipUri, "", sipDomain);

                return new RegisteredUserAgentAndProfilesDiscovery(
                    id: regSip.Id,
                    sipUri: regSip.SipUri,
                    displayName: dispName,
                    username: regSip.Username,
                    ipAddress: regSip.IpAddress,
                    userAgentHeader: regSip.UserAgentHeader,
                    userAgentName: regSip.UserAgentName,
                    locationName: regSip.LocationName,
                    locationShortName: regSip.LocationShortName,
                    regionName: regSip.RegionName,
                    cityName: regSip.CityName,
                    userOwnerName: regSip.UserOwnerName,
                    userDisplayName: regSip.UserDisplayName,
                    codecTypeName: regSip.CodecTypeName,
                    metaData: regSip.MetaData,
                    orderedProfiles: filteredProfiles,
                    locationProfileGroupSortWeight: profilesLocationSortWeight,
                    inCall: inCall,
                    inCallWithId: inCallWithId,
                    inCallWithSip: inCallWithSip,
                    inCallWithName: inCallWithName);
            }).ToList();
        }
    }
}
