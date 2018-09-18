using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.UserAgents
{
    public class UserAgentDtoV2
    {
        [JsonProperty("sipId")]
        public string SipId { get; set; }

        [JsonProperty("connectedTo")]
        public string ConnectedTo { get; set; }

        [JsonProperty("profiles")]
        public List<string> Profiles { get; set; }

        [JsonProperty("metadata")]  
        public List<KeyValuePairDtoV2> MetaData { get; set; }
    }
}