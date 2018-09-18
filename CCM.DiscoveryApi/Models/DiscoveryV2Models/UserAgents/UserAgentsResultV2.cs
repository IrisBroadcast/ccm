using System.Collections.Generic;
using CCM.DiscoveryApi.Models.DiscoveryV2Models.Profiles;
using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.UserAgents
{
    public class UserAgentsResultV2
    {
        [JsonProperty("profiles")]
        public List<ProfileDtoV2> Profiles { get; set; }

        [JsonProperty("userAgents")]
        public List<UserAgentDtoV2> UserAgents { get; set; }
    }
}