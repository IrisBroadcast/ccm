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
using System.Text.RegularExpressions;
using CCM.Core.Interfaces.Kamailio;
using CCM.Core.SipEvent.Messages;
using NLog;

namespace CCM.Core.SipEvent.Parser
{
    /// <summary>
    /// Parses incoming SIP registrar messages in JSON format.
    /// The format on the incoming messages is custom made
    /// and the details can be found in the 'connect' project
    /// on Github. Be sure to check that you are using the right
    /// version of 'connect' that formats the messages in JSON.
    /// </summary>
    public class SipEventParser : ISipEventParser
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public SipMessageBase Parse(KamailioSipEvent sipEvent)
        {
            switch (sipEvent.Event)
            {
                case SipEventType.Register:
                    return ParseRegistration(sipEvent);
                case SipEventType.Dialog:
                    return ParseDialog(sipEvent);
                case SipEventType.RegExpire:
                    return ParseRegExpire(sipEvent);
            }
            return null;
        }

        private SipRegistrationMessage ParseRegistration(KamailioSipEvent kamailioData)
        {
            var registration = new SipRegistrationMessage()
            {
                Sip = new SipUri(kamailioData.FromUri),
                Username = kamailioData.AuthUser, // TODO: Obsolete. To be removed.
                FromDisplayName = ParseDisplayName(kamailioData.FromDisplayName),
                ToDisplayName = ParseDisplayName(kamailioData.ToDisplayName),
                UserAgent = kamailioData.UserAgentHeader,
                Registrar = kamailioData.Registrar,
                RegType = kamailioData.RegType,
                Ip = kamailioData.Ip.SenderIp,
                Port = kamailioData.Ip.SenderPort,
                Expires = kamailioData.Expires,
                UnixTimeStamp = kamailioData.TimeStamp
            };

            return registration;
        }

        private SipRegistrationExpireMessage ParseRegExpire(KamailioSipEvent kamailioData)
        {
            var expire = new SipRegistrationExpireMessage()
            {
                SipAddress = new SipUri(kamailioData.FromUri),
                ReceivedIp = kamailioData.Ip.SenderIp
            };

            return expire;
        }

        private SipDialogMessage ParseDialog(KamailioSipEvent kamailioData)
        {
            if (!Enum.TryParse(kamailioData.DialogState, true, out DialogStatus dialogStatus))
            {
                log.Warn($"Dialog state field = {kamailioData.DialogState} of Kamailio dialog message");
                return null;
            }

            var dialog = new SipDialogMessage
            {
                Status = dialogStatus,
                CallId = kamailioData.CallId,
                HashId = kamailioData.DialogHashId,
                HashEntry = kamailioData.DialogHashEntry,
                FromDisplayName = ParseDisplayName(kamailioData.FromDisplayName),
                ToDisplayName = ParseDisplayName(kamailioData.ToDisplayName),
                FromSipUri = new SipUri(kamailioData.FromUri),
                ToSipUri = new SipUri(kamailioData.RequestUri), //new SipUri(kamailioData.ToUri),
                FromTag = kamailioData.FromTag,
                ToTag = kamailioData.ToTag,
            };

            return dialog;
        }

        public static string ParseDisplayName(string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : Regex.Unescape(s).Trim(' ', '"');
        }

    }
}