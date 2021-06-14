using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class RegionStatisticsModel
    {
        public DateTime StartTime { get; set; } = DateTime.UtcNow.AddMonths(-1);
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public Guid RegionId { get; set; }
        public string Name { get; set; }
    }
}
