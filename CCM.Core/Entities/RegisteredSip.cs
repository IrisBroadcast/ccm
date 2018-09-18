using System;
using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class RegisteredSip : CoreEntityBase
    {
        // Properties from codec message
        public string SIP { get; set; }
        public string UserAgentHead { get; set; } // User agent-sträng kodaren skickar
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int Expires { get; set; }
        public long ServerTimeStamp { get; set; }

        // Properties set on server
        public DateTime Updated { get; set; }
        public Location Location { get; set; }
        public UserAgent UserAgent { get; set; }
        public SipAccount User { get; set; }
    }

}