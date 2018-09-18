using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.ApiExternal;
using CCM.WebCommon.Authentication;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    [CcmUserBasicAuthentication] // Enable Basic authentication for this controller.
    public class HangupController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ICodecManager _codecManager;

        public HangupController(IRegisteredSipRepository registeredSipRepository, ICodecManager codecManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _codecManager = codecManager;
        }

        [Authorize]
        public async Task<bool> Post(HangupParameters hangupParameters) 
        {
            var codecInformation = GetCodecInformationBySipAddress(hangupParameters.SipAddress);
            if (codecInformation == null) { return false; }
            return await _codecManager.HangUpAsync(codecInformation);
        }

        private CodecInformation GetCodecInformationBySipAddress(string sipAddress)
        {
            var sip = _registeredSipRepository.GetCachedRegisteredSips().SingleOrDefault(s => s.Sip == sipAddress);

            if (sip == null || string.IsNullOrWhiteSpace(sip.Api))
            {
                return null;
            }

            return new CodecInformation() { Api = sip.Api, Ip = sip.IpAddress };
        }
    }
}
