using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Extensions;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using CCM.Web.Models.ApiExternal;
using CCM.WebCommon.Authentication;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    [CcmUserBasicAuthentication] // Enable Basic authentication for this controller.
    public class CallController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ICodecManager _codecManager;

        public CallController(IRegisteredSipRepository registeredSipRepository, ICodecManager codecManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _codecManager = codecManager;
        }

        [Authorize]
        public async Task<bool> Post(CallParameters callParameters)
        {
            log.Debug("Request to call. Caller:{0}, Callee:{1}, Profile:{2}", callParameters.Caller, callParameters.Callee, callParameters.Profile);

            var callerEmail = new SipUri(callParameters.Caller);

            string callee = callParameters.Callee; // Kan vara telefonnr (som saknar domän) eller sip-adress.
            if (!callee.IsNumeric())
            {
                // Sip-adress. Tolka.
                callee = new SipUri(callee).UserAtHost;
            }

            var codecInformation = GetCodecInformationBySipAddress(callerEmail.UserAtHost);
            if (codecInformation == null) { return false; }
            return await _codecManager.CallAsync(codecInformation, callee, callParameters.Profile);
        }

        private CodecInformation GetCodecInformationBySipAddress(string sipAddress)
        {
            var sip = _registeredSipRepository.GetCachedRegisteredSips().SingleOrDefault(s => s.Sip == sipAddress);
            return string.IsNullOrWhiteSpace(sip?.Api) ? null : new CodecInformation { Api = sip.Api, Ip = sip.IpAddress };
        }
    }
}
