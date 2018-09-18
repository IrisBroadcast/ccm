using System.Collections.Generic;
using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    public class Filter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("localised-name")]
        public LocalisedName LocalisedName { get; set; }

        [XmlElement("option")]
        public List<FilterOption> FilterOptions { get; set; }
    }
}