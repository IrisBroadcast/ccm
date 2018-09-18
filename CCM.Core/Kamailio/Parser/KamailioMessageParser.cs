using System;
using System.Reflection;
using System.Text.RegularExpressions;
using CCM.Core.Interfaces;
using CCM.Core.Kamailio.Messages;
using NLog;

namespace CCM.Core.Kamailio.Parser
{
    public class KamailioMessageParser : IKamailioMessageParser
    {
        private readonly IKamailioDataParser _kamailioDataParser;
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private const int DefaultExpireValue = 120;


        public KamailioMessageParser(IKamailioDataParser kamailioDataParser)
        {
            _kamailioDataParser = kamailioDataParser;
        }

        public KamailioMessageBase Parse(string message)
        {
            var kamailioData = _kamailioDataParser.ParseToKamailioData(message);
            if (kamailioData == null)
            {
                return null;
            }

            switch (kamailioData.MessageType)
            {
                case KamailioMessageType.Request:
                    return ParseRegistration(kamailioData);
                case KamailioMessageType.Dialog:
                    return ParseDialog(kamailioData);
                case KamailioMessageType.RegExpire:
                    return ParseRegExpire(kamailioData);
            }
            return null;
        }

        private KamailioDialogMessage ParseDialog(KamailioData kamailioData)
        {
            DialogStatus dialogStatus;
            if (!Enum.TryParse(kamailioData.GetField("dstat"), true, out dialogStatus))
            {
                log.Warn("Unable to parse dstat field of Kamailio dialog message");
                return null;
            }

            var dialog = new KamailioDialogMessage
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

            // Fix för tomt ru-fält i kamailio-data
            if (dialog.ToSipUri == null || string.IsNullOrEmpty(dialog.ToSipUri.User))
            {
                dialog.ToSipUri = new SipUri(kamailioData.GetField("tu"));
            }

            return dialog;
        }

        private KamailioRegistrationMessage ParseRegistration(KamailioData kamailioData)
        {
            
            var registration = new KamailioRegistrationMessage()
            {
                Ip = kamailioData.GetField("si"),
                Port = ParseInt(kamailioData.GetField("sp")),
                UnixTimeStamp = ParseLong(kamailioData.GetField("TS")),
                Sip = new SipUri(kamailioData.GetField("fu")),
                UserAgent = kamailioData.GetField("ua"),
                Username = kamailioData.GetField("Au"),
                ToDisplayName = ParseDisplayName(kamailioData.GetField("tn")),
                Expires = ParseInt(kamailioData.GetField("Expires"), DefaultExpireValue),

                // Används inte
                ToUsername = kamailioData.GetField("rU"),
                RequestedSip = new SipUri(kamailioData.GetField("ru")),
                ReceivedIp = kamailioData.GetField("Ri"),
                ReceivedPort = ParseInt(kamailioData.GetField("Rp")),
                FromDisplayName = ParseDisplayName(kamailioData.GetField("fn")),
                CallId = kamailioData.GetField("ci"),
            };

            return registration;
        }

        private KamailioRegistrationExpireMessage ParseRegExpire(KamailioData kamailioData)
        {
            var expire = new KamailioRegistrationExpireMessage()
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