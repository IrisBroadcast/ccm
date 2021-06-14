using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class CategoryStatisticsModel
    {
        public DateTime StartTime { get; set; } = DateTime.UtcNow.AddYears(-1).AddMonths(-1).AddDays(-20);
        public DateTime EndTime { get; set; } = DateTime.UtcNow.AddMonths(-11).AddDays(-7);
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
        public string CategoryName { get; set; }
    }
}
