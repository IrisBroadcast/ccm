using System.Collections.Generic;

namespace CCM.Core.Discovery
{
    public class UserAgentsResultDto
    {
        public List<ProfileDto> Profiles { get; set; }
        public List<UserAgentDto> UserAgents { get; set; }
    }
}