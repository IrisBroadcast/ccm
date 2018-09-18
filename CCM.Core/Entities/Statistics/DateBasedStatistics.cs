using System;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Core.Entities.Statistics
{
    public class DateBasedStatistics
    {
        public double AverageTime
        {
            get { return NumberOfCalls == 0 ? 0 : TotaltTimeForCalls/NumberOfCalls; }
        }

        public DateTime Date { get; set; }
        public double MaxCallTime { get; private set; }
        public double MinCallTime { get; private set; }
        public int NumberOfCalls { get; private set; }
        public double TotaltTimeForCalls { get; private set; }

        public void AddTime(double timeInMinutes)
        {
            if (timeInMinutes < MinCallTime && NumberOfCalls > 0)
            {
                MinCallTime = timeInMinutes;
            }
            else if (NumberOfCalls == 0)
            {
                MinCallTime = timeInMinutes;
            }

            if (timeInMinutes > MaxCallTime)
            {
                MaxCallTime = timeInMinutes;
            }

            TotaltTimeForCalls += timeInMinutes;
            NumberOfCalls++;
        }
    }

    public class DateBasedCallEvent
    {
        public DateTime Date { get; set; }
        public double Duration { get; set; }

        public static IEnumerable<DateBasedCallEvent> GetEvents(IList<CallHistory> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            return callHistories.SelectMany(callHistory => GetEvents(callHistory, reportPeriodStart, reportPeriodEnd));
        }

        private static IEnumerable<DateBasedCallEvent> GetEvents(CallHistory callHistory, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            var minDate = callHistory.Started >= reportPeriodStart ? callHistory.Started : reportPeriodStart;
            var maxDate = callHistory.Ended <= reportPeriodEnd ? callHistory.Ended : reportPeriodEnd;

            var currentDate = minDate.ToLocalTime().Date.ToUniversalTime();
            while (currentDate < maxDate)
            {
                var next = currentDate.AddDays(1.0);
                var duration = (maxDate > next ? next : maxDate) - (minDate < currentDate ? currentDate : minDate);
                yield return new DateBasedCallEvent { Date = currentDate.ToLocalTime(), Duration = duration.TotalMinutes};
                currentDate = next;
            }
        }
    }
}