using System.Collections.Generic;
using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class UserAgent
    {
        [XmlAttribute("sip-id")]
        public string SipId { get; set; }

        [XmlAttribute("connected-to")]
        public string ConnectedTo { get; set; }

        [XmlArray("profile-rec")]
        [XmlArrayItem("profile-ref")]
        public List<UserAgentProfileRef> ProfileRec { get; set; }

        [XmlArray("meta-data")]
        [XmlArrayItem("data")]
        public List<UserAgentMetaData> MetaData { get; set; }
    }
}