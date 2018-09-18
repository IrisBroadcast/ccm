namespace CCM.Web.Models.Location
{
    using System.Collections.Generic;

    public class LocationIndexViewModel
    {
        public List<Core.Entities.Location> Locations { get; set; }
        public string Search { get; set; }
        public int SortBy { get; set; }
        public int Direction { get; set; }
    }
}