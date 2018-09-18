using System;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Cors;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Kamailio;
using CCM.Web.Infrastructure.SignalR;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KamailioEventController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISipMessageManager _sipMessageManager;
        private readonly IGuiHubUpdater _guiHubUpdater;
        private readonly IStatusHubUpdater _statusHubUpdater;

        public KamailioEventController(ISipMessageManager sipMessageManager, IGuiHubUpdater guiHubUpdater, IStatusHubUpdater statusHubUpdater)
        {
            _sipMessageManager = sipMessageManager;
            _guiHubUpdater = guiHubUpdater;
            _statusHubUpdater = statusHubUpdater;
        }

        // For test
        public string Get()
        {
            return "Hello. I'm a Kamailio event receiver";
        }

        public IHttpActionResult Post([FromBody] string message)
        {
            log.Debug("Incoming Kamailio message: {0}", message);

            if (string.IsNullOrWhiteSpace(message))
            {
                log.Warn("Kamailio event controller received empty data");
                return BadRequest();
            }

            KamailioMessageHandlerResult result = _sipMessageManager.HandleMessage(message);

            if (result == null)
            {
                log.Warn("Kamailio message was handled but result was null");
            }
            else if (result.ChangeStatus != KamailioMessageChangeStatus.NothingChanged)
            {
                _guiHubUpdater.Update(result); // First web gui
                _statusHubUpdater.Update(result); // Then codec status to external clients
            }

            return Ok();
        }

    }
}