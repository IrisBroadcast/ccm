using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class UserAgentProfileRef
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}