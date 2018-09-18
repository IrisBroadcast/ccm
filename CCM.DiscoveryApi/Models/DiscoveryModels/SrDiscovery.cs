using System.Collections.Generic;
using System.Xml.Serialization;

namespace CCM.DiscoveryApi.Models.DiscoveryModels
{
    /// <summary>
    /// SR Discovery entity
    /// </summary>
    [XmlRoot("sr-discovery", Namespace = "", IsNullable = false)]
    public class SrDiscovery
    {
        [XmlArray("profiles")]
        [XmlArrayItem("profile", IsNullable = false)]
        public List<Profile> Profiles { get; set; }

        [XmlArray("filters")]
        [XmlArrayItem("filter", IsNullable = false)]
        public List<Filter> Filters { get; set; }

        [XmlArray("user-agents")]
        [XmlArrayItem("user-agent")]
        public List<UserAgent> UserAgents { get; set; }
    }
}