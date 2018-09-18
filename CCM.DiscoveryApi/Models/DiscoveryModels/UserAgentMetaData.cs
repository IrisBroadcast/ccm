using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class UserAgentMetaData
    {
        [XmlElement("localised-value")]
        public LocalisedName LocalisedName { get; set; }

        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}