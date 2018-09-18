using System.Collections.Generic;

namespace CCM.DiscoveryApi.Models
{
    public class SrDiscoveryParameters
    {
        public string Caller { get; set; }
        public string Callee { get; set; }
        public IList<KeyValuePair<string,string>> Filters { get; set; }
        public bool IncludeCodecsInCall { get; set; }
    }
}