using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCM.DiscoveryApi.Models.DiscoveryV2Models.Filters
{
    public class FilterV2
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("options")]
        public List<FilterOptionV2> Options { get; set; }
    }
}