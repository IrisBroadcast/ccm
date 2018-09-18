using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Regions
{
    public class RegionViewModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Locations")]
        public List<LocationViewModel> Locations { get; set; }
    }
}