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
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Extensions;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.SipEvent.Messages;
using NLog;

namespace CCM.Core.SipEvent
{
    public class KamailioMessageManager : ISipMessageManager
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ICallRepository _callRepository;
        private readonly IRegisteredSipRepository _sipRepository;

        public KamailioMessageManager(IRegisteredSipRepository sipRepository, ICallRepository callRepository)
        {
            _sipRepository = sipRepository;
            _callRepository = callRepository;
        }
        
        public SipEventHandlerResult HandleSipMessage(KamailioMessageBase sipMessage)
        {
            if (log.IsInfoEnabled)
            {
                log.Debug("Parsed Kamailio Message {0}", sipMessage.ToDebugString());
            }

            switch (sipMessage)
            {
                case SipRegistrationMessage regMessage:
                {
                    if (regMessage.Expires == 0)
                    {
                        return UnregisterCodec(new KamailioRegistrationExpireMessage { SipAddress = regMessage.Sip });
                    }
                    return RegisterSip(regMessage);
                }
                case KamailioRegistrationExpireMessage expireMessage:
                    return UnregisterCodec(expireMessage);
                case KamailioDialogMessage dialogMessage:
                    return HandleDialog(dialogMessage);
                default:
                    log.Info("Unhandled Kamailio message: {0}", sipMessage.ToDebugString());
                    return NothingChangedResult;
            }
        }

        public SipEventHandlerResult RegisterSip(SipRegistrationMessage sipMessage)
        {
            var sip = new RegisteredSip
            {
                IP = sipMessage.Ip,
                Port = sipMessage.Port,
                ServerTimeStamp = sipMessage.UnixTimeStamp,
                SIP = sipMessage.Sip.UserAtHost,
                UserAgentHead = sipMessage.UserAgent,
                Username = sipMessage.Username,
                DisplayName = string.IsNullOrEmpty(sipMessage.ToDisplayName) ? sipMessage.FromDisplayName : sipMessage.ToDisplayName,
                Expires = sipMessage.Expires
            };

            return _sipRepository.UpdateRegisteredSip(sip);
        }

        private SipEventHandlerResult UnregisterCodec(KamailioRegistrationExpireMessage expireMessage)
        {
            return _sipRepository.DeleteRegisteredSip(expireMessage.SipAddress.UserAtHost);
        }

        private SipEventHandlerResult HandleDialog(KamailioDialogMessage kamailioDialogMessage)
        {
            switch (kamailioDialogMessage.Status)
            {
                case DialogStatus.Start:
                    return RegisterCall(kamailioDialogMessage);
                case DialogStatus.End:
                    // TODO: Check hangup reason. Only close calls where reason = Normal
                    // TODO: Handle timeout message and add warning to call but don't end it
                    return CloseCall(kamailioDialogMessage);
                case DialogStatus.SingleBye:
                    // TODO: Handle single bye message and close call
                    log.Info("Received SingleBye command from Kamailio. {0}", kamailioDialogMessage);
                    return NothingChangedResult;
                default:
                    return NothingChangedResult;
            }
        }

        public SipEventHandlerResult RegisterCall(KamailioDialogMessage sipMessage)
        {
            log.Debug("Register call from {0} to {1}, call id \"{2}\", hash id:\"{3}\", hash entry:\"{4}\"",
                sipMessage.FromSipUri.UserAtHost, sipMessage.ToSipUri.UserAtHost, sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry);

            if (_callRepository.CallExists(sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry))
            {
                log.Debug("Call with id {0}, hash id:{1}, hash entry:{2} already exists", sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry);
                return NothingChangedResult;
            }

            var call = new Call();

            // Om user-delen är numerisk antar vi att det är ett telefonnummer (trots att sip-adresser egentligen kan vara numeriska)
            var fromSip = sipMessage.FromSipUri.User.IsNumeric() ? sipMessage.FromSipUri.User : sipMessage.FromSipUri.UserAtHost;
            var from = _sipRepository.GetCachedRegisteredSips().SingleOrDefault(rs => rs.Sip == fromSip);
            call.FromSip = fromSip;
            call.FromDisplayName = sipMessage.FromDisplayName;
            call.FromId = from?.Id ?? Guid.Empty;

            var toSip = sipMessage.ToSipUri.User.IsNumeric() ? sipMessage.ToSipUri.User : sipMessage.ToSipUri.UserAtHost;
            var to = _sipRepository.GetCachedRegisteredSips().SingleOrDefault(rs => rs.Sip == toSip);
            call.ToSip = toSip;
            call.ToDisplayName = sipMessage.ToDisplayName;
            call.ToId = to?.Id ?? Guid.Empty;

            call.Started = DateTime.UtcNow;
            call.CallId = sipMessage.CallId;
            call.DlgHashId = sipMessage.HashId;
            call.DlgHashEnt = sipMessage.HashEntry;
            call.Updated = DateTime.UtcNow;
            call.ToTag = sipMessage.ToTag;
            call.FromTag = sipMessage.FromTag;
            call.IsPhoneCall = sipMessage.FromSipUri.User.IsNumeric() || sipMessage.ToSipUri.User.IsNumeric(); // Sätts till true om någon parts adress är numeriskt. 
            _callRepository.UpdateCall(call);
            return SipMessageResult(SipEventChangeStatus.CallStarted, call.Id);
        }

        public SipEventHandlerResult CloseCall(KamailioDialogMessage sipMessage)
        {
            log.Debug("Closing call with id:\"{0}\", hash id:\"{1}\", hash entry:\"{2}\"", sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry);

            try
            {
                CallInfo call = _callRepository.GetCallInfo(sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry);

                if (call == null)
                {
                    log.Warn("Unable to find call with call id: {0}, hash id:{1}, hash entry:{2}", sipMessage.CallId, sipMessage.HashId, sipMessage.HashEntry);
                    return NothingChangedResult;
                }

                if (call.Closed)
                {
                    log.Warn("Call with call id: {0} already closed", sipMessage.CallId);
                    return NothingChangedResult;
                }

                _callRepository.CloseCall(call.Id);
                return SipMessageResult(SipEventChangeStatus.CallClosed, call.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error while closing call with call id: {0}", sipMessage.CallId);
                return NothingChangedResult;
            }
        }
        
        private SipEventHandlerResult NothingChangedResult => SipMessageResult(SipEventChangeStatus.NothingChanged);
        private SipEventHandlerResult SipMessageResult(SipEventChangeStatus status) { return new SipEventHandlerResult() { ChangeStatus = status }; }
        private SipEventHandlerResult SipMessageResult(SipEventChangeStatus status, Guid id) { return new SipEventHandlerResult() { ChangeStatus = status, ChangedObjectId = id }; }

    }
}
