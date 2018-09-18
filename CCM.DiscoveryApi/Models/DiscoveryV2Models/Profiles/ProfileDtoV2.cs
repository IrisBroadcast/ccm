using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.Profiles
{
    public class ProfileDtoV2
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sdp")]
        public string Sdp { get; set; }
    }
}