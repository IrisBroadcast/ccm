using System.Collections.Generic;

namespace CCM.Core.Discovery
{
    public class UserAgentSearchParamsDto
    {
        public string Caller { get; set; }
        public string Callee { get; set; }
        public IList<KeyValuePair<string, string>> Filters { get; set; }
        public bool IncludeCodecsInCall { get; set; }
    }
}