/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Core.Entities.Statistics
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
        public List<RegionCategory> RegionCategories { get; set; } = new List<RegionCategory>();
    }

    public class RegionCategory
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public List<CategoryStatistics> CategoryStatisticsList { get; set; } = new List<CategoryStatistics>();

        public List<CategoryStatistics> GetCategoryData(DateBasedCategoryCallEvent call, double duration)
        {
            // Weighting categories. Location category is above Type category in hierarchy.

            var fromCategory = call.FromLocationCategory;
            if (string.IsNullOrEmpty(fromCategory))
            {
                fromCategory = call.FromTypeCategory ?? "Ospecificerad"; // TODO: localize
            }

            if (!CategoryStatisticsList.Any(x => x.Name == fromCategory))
            {
                // Category did not exist in list, add the new category
                CategoryStatisticsList.Add(new CategoryStatistics
                {
                    Name = fromCategory,
                    NumberOfCalls = 1,
                    TotalTimeForCalls = duration
                });
            }
            else
            {
                // Bump up data in list
                foreach (var item in CategoryStatisticsList)
                {
                    if (item.Name == fromCategory)
                    {
                        item.NumberOfCalls++;
                        item.TotalTimeForCalls += duration;
                    }
                }
            }

            var toCategory = call.ToLocationCategory;
            if (string.IsNullOrEmpty(toCategory))
            {
                toCategory = call.ToTypeCategory ?? "Ospecificerad"; // TODO: localize
            }

            if (!CategoryStatisticsList.Any(x => x.Name == toCategory))
            {
                // Category did not exist in list, add the new category
                CategoryStatisticsList.Add(new CategoryStatistics
                {
                    Name = toCategory,
                    NumberOfCalls = 1,
                    TotalTimeForCalls = duration
                });
            }
            else
            {
                // Bump up data in list
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

        public static IEnumerable<DateBasedCategoryCallEvent> GetEvents(IList<CallHistory> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            return callHistories.SelectMany(callHistory => GetEvents(callHistory, reportPeriodStart, reportPeriodEnd));
        }

        private static IEnumerable<DateBasedCategoryCallEvent> GetEvents(CallHistory callHistory, DateTime reportPeriodStart, DateTime reportPeriodEnd)
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
