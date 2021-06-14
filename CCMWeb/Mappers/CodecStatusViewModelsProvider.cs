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
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.ApiExternal;

namespace CCM.Web.Mappers
{
    public class CodecStatusViewModelsProvider
    {
        private readonly ICachedRegisteredCodecRepository _cachedRegisteredCodecRepository;
        private readonly ICachedCallRepository _cachedCallRepository;
        private readonly ICachedSipAccountRepository _cachedSipAccountRepository;
        private readonly ISettingsManager _settingsManager;

        public CodecStatusViewModelsProvider(
            ICachedRegisteredCodecRepository cachedRegisteredCodecRepository,
            ICachedCallRepository cachedCallRepository,
            ICachedSipAccountRepository cachedSipAccountRepository,
            ISettingsManager settingsManager)
        {
            _cachedRegisteredCodecRepository = cachedRegisteredCodecRepository;
            _cachedCallRepository = cachedCallRepository;
            _cachedSipAccountRepository = cachedSipAccountRepository;
            _settingsManager = settingsManager;
        }

        public IEnumerable<CodecStatusViewModel> GetAll()
        {
            var sipDomain = _settingsManager.SipDomain;
            var registeredUserAgents = _cachedRegisteredCodecRepository.GetRegisteredUserAgents();
            var ongoingCalls = _cachedCallRepository.GetOngoingCalls(true);

            var userAgentsOnline = registeredUserAgents.Select(regSip =>
            {
                var result = new CodecStatusViewModel
                {
                    SipAddress = regSip.SipUri,
                    Id = regSip.Id,
                    PresentationName = DisplayNameHelper.GetDisplayName(regSip, sipDomain),
                    DisplayName = regSip.DisplayName
                };

                var call = ongoingCalls.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;
                result.InCall = inCall;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    result.IsCallingPart = isFromCaller;
                    result.ConnectedToSipAddress = isFromCaller ? call.ToSip : call.FromSip;
                    result.ConnectedToPresentationName = isFromCaller ? call.ToDisplayName : call.FromDisplayName;
                    result.ConnectedToLocation = isFromCaller ? call.ToLocationName : call.FromLocationName;
                    result.CallStartedAt = call.Started;
                }

                result.State = regSip.Id == Guid.Empty
                    ? CodecState.NotRegistered
                    : (inCall ? CodecState.InCall : CodecState.Available);

                return result;
            }).ToList();

            return userAgentsOnline;
        }

        public IEnumerable<CodecStatusViewModel> GetAllCodecsIncludeOffline()
        {
            // TODO: Try to remove this one as an endpoint
            var sipDomain = _settingsManager.SipDomain;
            var registeredUserAgents = _cachedRegisteredCodecRepository.GetRegisteredUserAgents();
            var ongoingCalls = _cachedCallRepository.GetOngoingCalls(true);

            var userAgentsOnline = registeredUserAgents.Select(regSip =>
            {
                var result = new CodecStatusViewModel
                {
                    SipAddress = regSip.SipUri,
                    Id = regSip.Id,
                    PresentationName = DisplayNameHelper.GetDisplayName(regSip, sipDomain),
                    DisplayName = regSip.DisplayName
                };

                var call = ongoingCalls.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;
                result.InCall = inCall;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    result.IsCallingPart = isFromCaller;
                    result.ConnectedToSipAddress = isFromCaller ? call.ToSip : call.FromSip;
                    result.ConnectedToPresentationName = isFromCaller ? call.ToDisplayName : call.FromDisplayName;
                    result.ConnectedToLocation = isFromCaller ? call.ToLocationName : call.FromLocationName;
                    result.CallStartedAt = call.Started;
                }

                result.State = regSip.Id == Guid.Empty
                    ? CodecState.NotRegistered
                    : (inCall ? CodecState.InCall : CodecState.Available);

                return result;
            }).ToList();

            // Add the offline accounts to the list
            var userAgentsIdsOnline = userAgentsOnline.Select(rs => rs.SipAddress);
            var sipAccounts = _cachedSipAccountRepository.GetAll();
            var accountsNotOnline = sipAccounts.Where(a => !userAgentsIdsOnline.Contains(a.UserName));

            IEnumerable<CodecStatusViewModel> notRegisteredSips = accountsNotOnline.Select(a => new CodecStatusViewModel
            {
                Id = Guid.Empty,
                SipAddress = a.UserName,
                DisplayName = DisplayNameHelper.GetDisplayName(a, sipDomain),
                State = CodecState.NotRegistered
            });

            return userAgentsOnline.Concat(notRegisteredSips).ToList();
        }
    }
}