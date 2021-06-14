using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Services;
using CCM.StatisticsWeb.Statistics;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Pages
{
    public partial class LocationStatistics
    {
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<CodecType> CodecTypes { get; set; }
        public IEnumerable<Owner> Owners { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow.AddMonths(-1);
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public Guid RegionId { get; set; }
        public Guid CodecTypeId { get; set; }
        public Guid OwnerId { get; set; }

        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        public IEnumerable<LocationBasedStatistics> locationBasedStatistics { get; set; } = new List<LocationBasedStatistics>();
    }
}
