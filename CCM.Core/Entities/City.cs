using System.Collections.Generic;
using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class City : CoreEntityBase
    {
        public string Name { get; set; }
        public List<Location> Locations { get; set; }

        public City()
        {
            Locations = new List<Location>();
        }
    }
}