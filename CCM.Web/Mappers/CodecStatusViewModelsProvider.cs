using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.ApiExternal;
using CCM.Web.Models.Home;

namespace CCM.Web.Mappers
{
    public class CodecStatusViewModelsProvider
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ICallRepository _callRepository;
        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly ISettingsManager _settingsManager;

        public CodecStatusViewModelsProvider(
            IRegisteredSipRepository registeredSipRepository,
            ICallRepository callRepository,
            ISipAccountRepository sipAccountRepository,
            ISettingsManager settingsManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _callRepository = callRepository;
            _sipAccountRepository = sipAccountRepository;
            _settingsManager = settingsManager;
        }

        public IEnumerable<CodecStatusViewModel> GetAll()
        {
            var registeredUserAgents = _registeredSipRepository.GetRegisteredUserAgents();
            var sipDomain = _settingsManager.SipDomain;
            var ongoingCalls = _callRepository.GetOngoingCalls(true);

            var userAgentsOnline = registeredUserAgents.Select(regSip =>
            {
                string displayName = DisplayNameHelper.GetDisplayName(regSip.DisplayName, regSip.UserDisplayName,
                    string.Empty, regSip.Username, regSip.SipUri, "", sipDomain);

                var result = new CodecStatusViewModel
                {
                    SipAddress = regSip.SipUri,
                    Id = regSip.Id,
                    PresentationName = displayName,
                    DisplayName = displayName
                };

                var call = ongoingCalls.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;
                result.InCall = inCall;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    result.IsCallingPart = isFromCaller;
                    result.ConnectedToSipAddress = isFromCaller ? call.ToSip : call.FromSip;
                    result.ConnectedToPresentationName = isFromCaller
                        ? DisplayNameHelper.GetDisplayName(call.ToDisplayName, null, null, "", call.ToSip, "", sipDomain)
                        : DisplayNameHelper.GetDisplayName(call.FromDisplayName, null, null, "", call.FromSip, "", sipDomain);
                    result.ConnectedToLocation = isFromCaller ? call.ToLocationName : call.FromLocationName;
                    result.CallStartedAt = call.Started;
                }
                // TODO: In Call with DisplayName is lacking the actual Display name (on user) entered in CCM. Not sure the importance.

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
            var registeredUserAgents = _registeredSipRepository.GetRegisteredUserAgents();
            var sipDomain = _settingsManager.SipDomain;
            var ongoingCalls = _callRepository.GetOngoingCalls(true);

            var userAgentsOnline = registeredUserAgents.Select(regSip =>
            {
                string displayName = DisplayNameHelper.GetDisplayName(regSip.DisplayName, regSip.UserDisplayName,
                    string.Empty, regSip.Username, regSip.SipUri, "", sipDomain);

                var result = new CodecStatusViewModel
                {
                    SipAddress = regSip.SipUri,
                    Id = regSip.Id,
                    PresentationName = displayName,
                    DisplayName = displayName
                };

                var call = ongoingCalls.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;
                result.InCall = inCall;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    result.IsCallingPart = isFromCaller;
                    result.ConnectedToSipAddress = isFromCaller ? call.ToSip : call.FromSip;
                    result.ConnectedToPresentationName = isFromCaller
                        ? DisplayNameHelper.GetDisplayName(call.ToDisplayName, null, null, "", call.ToSip, "", sipDomain)
                        : DisplayNameHelper.GetDisplayName(call.FromDisplayName, null, null, "", call.FromSip, "", sipDomain);
                    result.ConnectedToLocation = isFromCaller ? call.ToLocationName : call.FromLocationName;
                    result.CallStartedAt = call.Started;
                }
                // TODO: In Call with DisplayName is lacking the actual Display name (on user) entered in CCM. Not sure the importance.

                result.State = regSip.Id == Guid.Empty
                    ? CodecState.NotRegistered
                    : (inCall ? CodecState.InCall : CodecState.Available);

                return result;
            }).ToList();

            // Add the offline accounts to the list
            var userAgentsIdsOnline = userAgentsOnline.Select(rs => rs.SipAddress);
            var sipAccounts = _sipAccountRepository.GetAll();
            var accountsNotOnline = sipAccounts.Where(a => !userAgentsIdsOnline.Contains(a.UserName));

            IEnumerable<CodecStatusViewModel> notRegisteredSips = accountsNotOnline.Select(a => new CodecStatusViewModel
            {
                Id = Guid.Empty,
                SipAddress = a.UserName,
                DisplayName = DisplayNameHelper.GetDisplayName("", a.DisplayName, string.Empty, "", a.UserName, "", sipDomain),
                State = CodecState.NotRegistered
            });

            return userAgentsOnline.Concat(notRegisteredSips).ToList();
        }
    }
}