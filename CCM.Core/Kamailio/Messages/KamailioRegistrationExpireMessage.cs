namespace CCM.Core.Kamailio.Messages
{
    public class KamailioRegistrationExpireMessage : KamailioMessageBase
    {
        public SipUri SipAddress { get; set; }         // Sip-URL
        public string ReceivedIp { get; set; }  // Ip-nummer som meddelandet skickades ifrån

        public override string ToDebugString()
        {
            return string.Format("SipAddress:{0}, ReceivedIp:{1}", SipAddress, ReceivedIp);
        }
    }
}