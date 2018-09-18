using System.Collections.Generic;
using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class ProfileGroup: CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Profile> Profiles { get; set; }

        public ProfileGroup()
        {
            Profiles = new List<Profile>();
        }
    }
}