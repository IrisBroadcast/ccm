using System.Collections.Generic;
using CCM.Core.Discovery;

namespace CCM.Core.Interfaces
{
    public interface IDiscoveryService
    {
        UserAgentsResultDto GetUserAgents(string caller, string callee, IList<KeyValuePair<string, string>> filters, bool includeCodecsInCall = false);
        List<ProfileDto> GetProfiles();
        List<FilterDto> GetFilters();
    }
}