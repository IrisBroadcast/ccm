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
    /// Parses incoming SIP registrar messages in string format.
    /// Separated with '::', this is the first and oldest format.
    /// We are now trying to move to JSON.
    /// The format on the incoming messages is custom made
    /// and the details can be found in the 'connect' project
    /// on Github. Be sure to check that you are using the right
    /// version of 'connect' that formats the messages with string
    /// separation. Or try to see if you can implement the JSON version.
    /// </summary>
    public class KamailioMessageParser : IKamailioMessageParser
    {
        private readonly IKamailioDataParser _kamailioDataParser;
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private const int DefaultExpireValue = 120;

        public KamailioMessageParser(IKamailioDataParser kamailioDataParser)
        {
            _kamailioDataParser = kamailioDataParser;
        }

        public SipMessageBase Parse(string message)
        {
            var kamailioData = _kamailioDataParser.ParseToKamailioData(message);
            if (kamailioData == null)
            {
                return null;
            }

            switch (kamailioData.MessageType)
            {
                case SipEventMessageType.Request:
                    return ParseRegistration(kamailioData);
                case SipEventMessageType.Dialog:
                    return ParseDialog(kamailioData);
                case SipEventMessageType.RegExpire:
                    return ParseRegExpire(kamailioData);
            }
            return null;
        }

        private SipDialogMessage ParseDialog(KamailioData kamailioData)
        {
            DialogStatus dialogStatus;
            if (!Enum.TryParse(kamailioData.GetField("dstat"), true, out dialogStatus))
            {
                log.Warn("Unable to parse dstat field of Kamailio dialog message");
                return null;
            }

            var dialog = new SipDialogMessage
            {
                Status = dialogStatus,
                CallId = kamailioData.GetField("ci"),
                HashId = kamailioData.GetField("hashid"),
                HashEntry = kamailioData.GetField("hashent"),
                FromDisplayName = ParseDisplayName(kamailioData.GetField("fn")),
                ToDisplayName = ParseDisplayName(kamailioData.GetField("tn")),
                FromSipUri = new SipUri(kamailioData.GetField("fu")),
                ToSipUri = new SipUri(kamailioData.GetField("ru")),
                FromTag = kamailioData.GetField("fot"),
                ToTag = kamailioData.GetField("tot"),
                Sdp = kamailioData.GetField("sdp"),
                HangupReason = kamailioData.GetField("hr")
            };

            // Fix for empty ru-field in Kamailio data
            if (dialog.ToSipUri == null || string.IsNullOrEmpty(dialog.ToSipUri.User))
            {
                dialog.ToSipUri = new SipUri(kamailioData.GetField("tu"));
            }

            return dialog;
        }

        private SipRegistrationMessage ParseRegistration(KamailioData kamailioData)
        {
            var registration = new SipRegistrationMessage()
            {
                Ip = kamailioData.GetField("si"),
                Port = ParseInt(kamailioData.GetField("sp")),
                UnixTimeStamp = ParseLong(kamailioData.GetField("TS")),
                Sip = new SipUri(kamailioData.GetField("fu")),
                FromDisplayName = ParseDisplayName(kamailioData.GetField("fn")),
                UserAgent = kamailioData.GetField("ua"),
                Username = kamailioData.GetField("Au"),
                ToDisplayName = ParseDisplayName(kamailioData.GetField("tn")),
                Expires = ParseInt(kamailioData.GetField("Expires"), DefaultExpireValue),

                // Not in use
                //ToUsername = kamailioData.GetField("rU"),
                //RequestedSip = new SipUri(kamailioData.GetField("ru")),
                //ReceivedIp = kamailioData.GetField("Ri"),
                //ReceivedPort = ParseInt(kamailioData.GetField("Rp")),
                //CallId = kamailioData.GetField("ci"),
            };

            return registration;
        }

        private SipRegistrationExpireMessage ParseRegExpire(KamailioData kamailioData)
        {
            // TODO: Is this one ever called?
            var expire = new SipRegistrationExpireMessage()
            {
                SipAddress = new SipUri(kamailioData.GetField("aor")),
                ReceivedIp = kamailioData.GetField("ip"),
            };

            return expire;
        }


        public static string ParseDisplayName(string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : Regex.Unescape(s).Trim(' ', '"');
        }

        private int ParseInt(string s, int defaultValue = 0)
        {
            int i;
            if (int.TryParse(s, out i))
            {
                return i;    
            }
            return defaultValue;
            
        }

        private long ParseLong(string s)
        {
            long i;
            long.TryParse(s, out i);
            return i;
        }

    }
}
