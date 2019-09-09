using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.Home;

namespace CCM.Web.Mappers
{
    public class RegisteredUserAgentViewModelsProvider
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ICallRepository _callRepository;
        private readonly ISettingsManager _settingsManager;

        public RegisteredUserAgentViewModelsProvider(
            IRegisteredSipRepository registeredSipRepository,
            ICallRepository callRepository,
            ISettingsManager settingsManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _callRepository = callRepository;
            _settingsManager = settingsManager;
        }

        public IEnumerable<RegisteredUserAgentViewModel> GetAll()
        {
            var registeredUserAgents = _registeredSipRepository.GetRegisteredUserAgents();
            var sipDomain = _settingsManager.SipDomain;

            var calls = _callRepository.GetOngoingCalls(true);

            var userAgentsOnline = registeredUserAgents.Select(regSip =>
            {
                var result = new RegisteredUserAgentViewModel
                {
                    Sip = regSip.SipUri,
                    Id = regSip.Id,

                    DisplayName = DisplayNameHelper.GetDisplayName(regSip.DisplayName, regSip.UserDisplayName,
                        string.Empty, regSip.Username, regSip.SipUri, "", sipDomain),

                    Location = regSip.Location,
                    LocationShortName = regSip.LocationShortName,
                    Image = regSip.Image,
                    CodecTypeName = regSip.CodecTypeName,
                    CodecTypeColor = regSip.CodecTypeColor,
                    UserName = regSip.Username,
                    UserComment = regSip.UserComment,
                    RegionName = regSip.RegionName
                };

                var call = calls.FirstOrDefault(c => c.FromSip == regSip.SipUri || c.ToSip == regSip.SipUri);
                bool inCall = call != null;
                result.InCall = inCall;

                if (inCall)
                {
                    var isFromCaller = call.FromSip == regSip.SipUri;
                    result.InCallWithId = isFromCaller ? call.ToId : call.FromId;
                    result.InCallWithSip = isFromCaller ? call.ToSip : call.FromSip;
                    result.InCallWithName = isFromCaller ? call.ToDisplayName : call.FromDisplayName;
                }

                return result;
            }).ToList();

            return userAgentsOnline;
        }
    }
}