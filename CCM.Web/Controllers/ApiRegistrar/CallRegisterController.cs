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

using CCM.Core.Interfaces.Managers;
using CCM.Core.SipEvent;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using CCM.Core.Interfaces.Parser;
using CCM.Core.SipEvent.Messages;
using CCM.Web.Hubs;
using CCM.Web.Models.ApiRegistrar;

namespace CCM.Web.Controllers.ApiRegistrar
{
    /// <summary>
    /// Receives ongoing call information from external services, like WebRTC services.
    /// This is used so information will be stored in the statistics table.
    /// </summary>
    public class CallRegisterController : ControllerBase {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ISipEventParser _sipEventParser;
        private readonly IExternalStoreMessageManager _externalStoreMessageManager;
        private readonly IWebGuiHubUpdater _webGuiHubUpdater;
        private readonly ICodecStatusHubUpdater _codecStatusHubUpdater;
        private readonly ISettingsManager _settingsManager;

        public CallRegisterController(
            ISipEventParser sipEventParser,
            IExternalStoreMessageManager externalStoreMessageManager,
            IWebGuiHubUpdater webGuiHubUpdater,
            ICodecStatusHubUpdater codecStatusHubUpdater,
            ISettingsManager settingsManager)
        {
            _sipEventParser = sipEventParser;
            _externalStoreMessageManager = externalStoreMessageManager;
            _webGuiHubUpdater = webGuiHubUpdater;
            _codecStatusHubUpdater = codecStatusHubUpdater;
            _settingsManager = settingsManager;
        }

        [HttpGet]
        public string Index()
        {
            return $"Hello. I'm a call registration event receiver.";
        }

        [HttpPost]
        public IActionResult Index([FromBody] DialogRegistrationViewModel callEvent)
        {
            if (callEvent == null)
            {
                log.Warn("Call register controller received empty data");
                return BadRequest("Call register controller received empty data");
            }

            var sipMessage = new ExternalDialogMessage
            {
                CallId = callEvent.CallId,
                Status = ExternalDialogStatus.Start,
                Started = callEvent.Started,
                Ended = callEvent.Ended,
                IsPhoneCall = callEvent.IsPhoneCall,
                SDP = callEvent.SDP,
                FromId = callEvent.FromId,
                FromUsername = callEvent.FromUsername,
                FromDisplayName = callEvent.FromDisplayName,
                FromIPAddress = callEvent.FromIPAddress,
                FromCategory = callEvent.FromCategory,
                ToId = callEvent.ToId,
                ToUsername = callEvent.ToUsername,
                ToDisplayName = callEvent.ToDisplayName,
                ToIPAddress = callEvent.ToIPAddress,
                ToCategory = callEvent.ToCategory
            };

            SipEventHandlerResult result = _externalStoreMessageManager.HandleDialog(sipMessage);

            if (result == null)
            {
                log.Warn("External message was handled but result was null");
            }
            else if (result.ChangeStatus != SipEventChangeStatus.NothingChanged)
            {
                _webGuiHubUpdater.Update(result); // First web gui
                _codecStatusHubUpdater.Update(result); // Then codec status to external clients
            }

            return Ok();
        }
    }
}
