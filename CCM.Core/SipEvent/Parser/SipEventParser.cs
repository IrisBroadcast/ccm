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

using System.Text.RegularExpressions;
using CCM.Core.Interfaces.Kamailio;
using CCM.Core.SipEvent.Messages;
using NLog;

namespace CCM.Core.SipEvent.Parser
{
    public class SipEventParser : ISipEventParser
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();


        public KamailioMessageBase Parse(KamailioSipEvent sipEvent)
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
                Username = kamailioData.AuthUser, // Obsolete. To be removed.
                FromDisplayName = ParseDisplayName(kamailioData.FromDisplayName),
                ToDisplayName = ParseDisplayName(kamailioData.ToDisplayName),
                UserAgent = kamailioData.UserAgentHeader,
                //PhysicalCodecUri = new SipUri(kamailioData.PoolUri),

                Ip = kamailioData.Ip.SenderIp,
                Port = kamailioData.Ip.SenderPort,
                Expires = kamailioData.Expires,
                UnixTimeStamp = kamailioData.TimeStamp
            };

            return registration;
        }

        private KamailioRegistrationExpireMessage ParseRegExpire(KamailioSipEvent kamailioData)
        {
            var expire = new KamailioRegistrationExpireMessage()
            {
                SipAddress = new SipUri(kamailioData.FromUri),
                ReceivedIp = kamailioData.Ip.SenderIp
            };

            return expire;
        }

        private KamailioDialogMessage ParseDialog(KamailioSipEvent kamailioData)
        {
            // TODO: Parse correct fields

            DialogStatus dialogStatus = DialogStatus.Start;
            //if (!Enum.TryParse(kamailioData.GetField("dstat"), true, out dialogStatus))
            //{
            //    log.Warn("Unable to parse dstat field of Kamailio dialog message");
            //    return null;
            //}

            var dialog = new KamailioDialogMessage
            {

                Status = dialogStatus,
                CallId = kamailioData.CallId,
                //HashId = kamailioData.GetField("hashid"),
                //HashEntry = kamailioData.GetField("hashent"),
                FromDisplayName = ParseDisplayName(kamailioData.FromUri),
                ToDisplayName = ParseDisplayName(kamailioData.ToDisplayName),
                FromSipUri = new SipUri(kamailioData.FromUri),
                //ToSipUri = new SipUri(kamailioData.GetField("ru")),
                //FromTag = kamailioData.GetField("fot"),
                //ToTag = kamailioData.GetField("tot"),
                //Sdp = kamailioData.GetField("sdp"),
                //HangupReason = kamailioData.GetField("hr")
            };

            // Fix för tomt ru-fält i kamailio-data
            //if (dialog.ToSipUri == null || string.IsNullOrEmpty(dialog.ToSipUri.User))
            //{
            //    dialog.ToSipUri = new SipUri(kamailioData.GetField("tu"));
            //}

            return dialog;
        }


        public static string ParseDisplayName(string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : Regex.Unescape(s).Trim(' ', '"');
        }

    }
}