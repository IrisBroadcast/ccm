using System.Collections.Generic;
using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class Profile : CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sdp { get; set; }
        public ICollection<ProfileGroupInfo> Groups { get; set; }
        public List<UserAgent> UserAgents { get; set; }
        public int SortIndex { get; set; }

        public Profile()
        {
            UserAgents = new List<UserAgent>();
        }
    }
}