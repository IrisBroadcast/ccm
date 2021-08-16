using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Statistics
{
    public class DateBasedCategoryStatistics
    {
        public DateTime Date { get; set; }
        public IEnumerable<RegionCategoryStatistics> RegionCategories { get; set; } = new List<RegionCategoryStatistics>();

    }

    public class RegionCategoryStatistics
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IEnumerable<CategoryStatistics> CategoryStatisticsList { get; set; } = new List<CategoryStatistics>();
    }

    public class CategoryStatistics
    {
        public string Name { get; set; }
        public int NumberOfCalls { get; set; }
        public double TotalTimeForCalls { get; set; }
    }


}

