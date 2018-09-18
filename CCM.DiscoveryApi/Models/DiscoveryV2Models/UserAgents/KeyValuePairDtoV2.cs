using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.UserAgents
{
    public class KeyValuePairDtoV2
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        public KeyValuePairDtoV2(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}