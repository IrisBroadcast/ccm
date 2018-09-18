using System.Collections.Generic;
using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class Region : CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public List<Location> Locations { get; set; }

        public Region()
        {
            Locations = new List<Location>();
        }
    }
}