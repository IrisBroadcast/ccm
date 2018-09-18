using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.Filters
{
    public class FilterOptionV2
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}