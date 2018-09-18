namespace CCM.Core.Kamailio.Messages
{
    public class KamailioRegistrationMessage : KamailioMessageBase
    {
        public SipUri Sip { get; set; }
        public string FromDisplayName { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public SipUri RequestedSip { get; set; }
        public string ReceivedIp { get; set; } // Används inte
        public int ReceivedPort { get; set; } // Används inte
        public string UserAgent { get; set; }
        public string ToUsername { get; set; }
        public string Username { get; set; }
        public string CallId { get; set; }
        public string ToDisplayName { get; set; }
        public long UnixTimeStamp { get; set; }
        public int Expires { get; set; }

        public override string ToDebugString()
        {
            return string.Format(
                    "SIP:{0}, IP:{1}, Port:{2}, RequestedSip:{3}, ReceivedIp:{4}, ReceivedPort:{5}, UserAgent:{6}, ToUsername:{7}" +
                    ", Username:{8}, CallId:{9}, ToDisplayName:{10}, UnixTimeStamp:{11}, Expires:{12}, FromDisplayName:{13}",
                    Sip, Ip, Port, RequestedSip, ReceivedIp, ReceivedPort, UserAgent, ToUsername, Username, CallId, ToDisplayName, UnixTimeStamp, Expires, FromDisplayName);
        }
    }
}