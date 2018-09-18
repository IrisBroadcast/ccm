using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class LocalisedName
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("lang")]
        public string Lang { get; set; }
    }
}