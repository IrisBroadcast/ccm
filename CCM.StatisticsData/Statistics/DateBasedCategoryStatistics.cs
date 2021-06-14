using CCM.StatisticsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Statistics
{

    public class CategoryStatistics
    {
        public string Name { get; set; }
        public int NumberOfCalls { get; set; }
        public double TotalTimeForCalls { get; set; }
    }

    public class DateBasedCategoryStatistics
    {
        public DateTime Date { get; set; }
       //  public List<CategoryStatistics> CategoryStatisticsList { get; set; } = new List<CategoryStatistics>();

        // TODO List of RegionCategory instead of list above. RegionCategory should contain list of CategoryStatistics?
        public List<RegionCategory> RegionCategories { get; set; } = new List<RegionCategory>();

    }
    public class RegionCategory
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public List<CategoryStatistics> CategoryStatisticsList { get; set; } = new List<CategoryStatistics>();



        public List<CategoryStatistics> GetCategoryData(DateBasedCategoryCallEvent call, double duration)
        {
            var fromCategory = call.FromLocationCategory;
            var toCategory = call.ToLocationCategory;

           
            // Weighting categories. Location category is above Type category in hierarchy. 
            if (string.IsNullOrEmpty(fromCategory))
            {
                fromCategory = call.FromTypeCategory ?? "Ospecificerad";
            }
            if (string.IsNullOrEmpty(toCategory))
            {
                toCategory = call.ToTypeCategory ?? "Ospecificerad";
            }

            if (!CategoryStatisticsList.Any(x => x.Name == fromCategory))
            {
                CategoryStatisticsList.Add(new CategoryStatistics
                {
                    Name = fromCategory,
                    NumberOfCalls = 1,
                    TotalTimeForCalls = duration
                });
            }

            else
            {
                foreach (var item in CategoryStatisticsList)
                {
                    if (item.Name == fromCategory)
                    {
                        item.NumberOfCalls++;
                        item.TotalTimeForCalls += duration;
                    }
                }
            }

            if (!CategoryStatisticsList.Any(x => x.Name == toCategory))

            {
                CategoryStatisticsList.Add(new CategoryStatistics
                {
                    Name = toCategory,
                    NumberOfCalls = 1,
                    TotalTimeForCalls = duration
                });
            }

            else
            {
                foreach (var item in CategoryStatisticsList)
                {
                    if (item.Name == toCategory)
                    {
                        item.NumberOfCalls++;
                        item.TotalTimeForCalls += duration;
                    }
                }
            }
            return CategoryStatisticsList;
        }
    }
    public class DateBasedCategoryCallEvent
    {
        public DateTime Date { get; set; }
        public double Duration { get; set; }
        public string FromLocationCategory { get; set; }
        public string ToLocationCategory { get; set; }
        public string FromTypeCategory { get; set; }
        public string ToTypeCategory { get; set; }

        public static IEnumerable<DateBasedCategoryCallEvent> GetEvents(IList<CallHistoryEntity> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            return callHistories.SelectMany(callHistory => GetEvents(callHistory, reportPeriodStart, reportPeriodEnd));
        }

        private static IEnumerable<DateBasedCategoryCallEvent> GetEvents(CallHistoryEntity callHistory, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            var minDate = callHistory.Started >= reportPeriodStart ? callHistory.Started : reportPeriodStart;
            var maxDate = callHistory.Ended <= reportPeriodEnd ? callHistory.Ended : reportPeriodEnd;

            var currentDate = minDate.ToLocalTime().Date.ToUniversalTime();
            while (currentDate < maxDate)
            {
                var next = currentDate.AddDays(1.0);
                var duration = (maxDate > next ? next : maxDate) - (minDate < currentDate ? currentDate : minDate);

                yield return new DateBasedCategoryCallEvent
                {
                    Date = currentDate.ToLocalTime(),
                    FromLocationCategory = callHistory.FromCodecTypeCategory,
                    FromTypeCategory = callHistory.FromCodecTypeCategory,
                    ToLocationCategory = callHistory.ToLocationCategory,
                    ToTypeCategory = callHistory.ToCodecTypeCategory,
                    Duration = duration.TotalMinutes
                };
                currentDate = next;
            }
        }


    }
}

