using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class FilterOption
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("localised-name")]
        public LocalisedName LocalisedName { get; set; }
    }
}