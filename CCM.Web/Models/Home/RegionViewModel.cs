using System;
namespace CCM.Web.Models.Home
{
    public class RegionViewModel
    {
        public Guid RegionId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}