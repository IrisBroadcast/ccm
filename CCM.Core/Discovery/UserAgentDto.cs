using System.Collections.Generic;

namespace CCM.Core.Discovery
{
    public class UserAgentDto
    {
        public string SipId { get; set; }
        public string PresentationName { get; set; } // Endast i internt API i JSON-format
        public string ConnectedTo { get; set; }
        public bool InCall { get; set; }
        public List<string> Profiles { get; set; }
        public List<KeyValuePair<string,string>> MetaData { get; set; }
    }
}