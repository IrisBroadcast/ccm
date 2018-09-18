using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    /// <summary>
    /// Profile entity
    /// </summary>
    public class Profile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("sdp")]
        public string Sdp { get; set; }

        [XmlElement("localised-name")]
        public LocalisedName LocalisedName { get; set; }
    }
}