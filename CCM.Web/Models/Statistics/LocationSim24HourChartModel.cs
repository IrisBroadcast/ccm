using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Entities;

namespace CCM.Web.Models.Statistics
{
    public class LocationSim24HourChartModel
    {
        [Display(ResourceType = typeof(Resources), Name = "Location")]
        public List<ChartLocationModel> Locations { get; set; }

        public bool LoadChartImage 
        {
            get { return LocationId != Guid.Empty; }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RegionId { get; set; }
        public Guid LocationId { get; set; }
    }

    public class ChartLocationModel 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}