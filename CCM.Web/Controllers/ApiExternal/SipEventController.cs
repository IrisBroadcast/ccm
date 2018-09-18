using System.Web.Http;
using System.Web.Http.Cors;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Kamailio;
using CCM.Web.Infrastructure.SignalR;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SipEventController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISipMessageManager _sipMessageManager;
        private readonly IGuiHubUpdater _guiHubUpdater;
        private readonly IStatusHubUpdater _statusHubUpdater;

        public SipEventController(ISipMessageManager sipMessageManager, IGuiHubUpdater guiHubUpdater, IStatusHubUpdater statusHubUpdater)
        {
            _sipMessageManager = sipMessageManager;
            _guiHubUpdater = guiHubUpdater;
            _statusHubUpdater = statusHubUpdater;
        }

        // For test
        public string Get()
        {
            return "Hello. I'm a SIP event receiver";
        }

        public IHttpActionResult Post(dynamic message)
        {
            log.Debug("Incoming SIP message from Kamailio: {0}", message != null ? message.ToString() : "<null>");
            return Ok(message as object);
        }

    }
}