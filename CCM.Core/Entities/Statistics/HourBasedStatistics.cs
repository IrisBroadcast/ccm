using System;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Core.Entities.Statistics
{
    public class HourBasedStatistics
    {
        public DateTime Date { get; private set; }
        public int Hour { get; private set; }
        public int MaxSimultaneousCalls { get { return _maxSimultaneousCallsPerDay == null ? 0 :  _maxSimultaneousCallsPerDay.Max(); } }
        public int OngoingCalls { get; set; }
        public int DayCount { get { return _maxSimultaneousCallsPerDay == null ? 1 : _maxSimultaneousCallsPerDay.Count; } }

        private List<int> _maxSimultaneousCallsPerDay; 

        public double AverageNumberOfSimultaneousCalls 
        {
            get { return _maxSimultaneousCallsPerDay == null || _maxSimultaneousCallsPerDay.Count == 0 ? 0.0 : _maxSimultaneousCallsPerDay.Sum() / (double)_maxSimultaneousCallsPerDay.Count; }
        }

        public int MedianNumberOfSimultaneousCalls 
        {
            get
            {
                if (_maxSimultaneousCallsPerDay == null || _maxSimultaneousCallsPerDay.Count == 0) return 0;
                if (_maxSimultaneousCallsPerDay.Count == 1) return _maxSimultaneousCallsPerDay[0];
                var orderedList = _maxSimultaneousCallsPerDay.OrderBy(i => i).ToList();
                var half = _maxSimultaneousCallsPerDay.Count/2;
                if (_maxSimultaneousCallsPerDay.Count%2 == 0)
                    return Math.Max(_maxSimultaneousCallsPerDay[half - 1], _maxSimultaneousCallsPerDay[half]);
                return _maxSimultaneousCallsPerDay[half];
            }
        }

        public string Label 
        {
            get { return string.Format("{0:00}", Hour); }
        }

        public bool AddEvent(HourBasedCallEvent callEvent)
        {
            if (_maxSimultaneousCallsPerDay == null)
                _maxSimultaneousCallsPerDay = new List<int>();
            if (_maxSimultaneousCallsPerDay.Count < 1)
                _maxSimultaneousCallsPerDay.Add(0);
            if (callEvent.EventTime > Date.AddHours(1)) return false;
            if (callEvent.EventType == CallEventType.Start)
            {
                OngoingCalls++;
                if (OngoingCalls > _maxSimultaneousCallsPerDay[0])
                    _maxSimultaneousCallsPerDay[0] = OngoingCalls;
                return true;
            }
            OngoingCalls--;
            return true;
        }

        public HourBasedStatistics CopyForAggregate()
        {
            return new HourBasedStatistics
            {
                Date = Date,
                Hour = Hour,
                _maxSimultaneousCallsPerDay = _maxSimultaneousCallsPerDay.ToList(),
                OngoingCalls = 0
            };
        }

        public void MergeWith(HourBasedStatistics stats)
        {
            _maxSimultaneousCallsPerDay.AddRange(stats._maxSimultaneousCallsPerDay);
        }

        public HourBasedStatistics GetNext()
        {
            return new HourBasedStatistics
            {
                Date = Date.AddHours(1.0),
                Hour = Date.AddHours(1.0).ToLocalTime().Hour,
                _maxSimultaneousCallsPerDay = new List<int> { OngoingCalls },
                OngoingCalls = OngoingCalls,
            };
        }

        public static HourBasedStatistics Create(DateTime date)
        {
            var hour = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            return new HourBasedStatistics
            {
                Date = hour,
                Hour = hour.ToLocalTime().Hour,
                _maxSimultaneousCallsPerDay = new List<int> {0},
                OngoingCalls = 0,
            };
        }

        public static List<HourBasedStatistics> Aggregate(IEnumerable<HourBasedStatistics> items)
        {
            var dictionary = new Dictionary<int, HourBasedStatistics>();
            foreach (var item in items)
            {
                if (!dictionary.ContainsKey(item.Hour))
                {
                    dictionary.Add(item.Hour, item.CopyForAggregate());
                }
                else
                {
                    dictionary[item.Hour].MergeWith(item);
                }
            }
            return dictionary.Values.OrderBy(v => v.Hour).ToList();
        }
    }

    public class HourBasedCallEvent
    {
        public DateTime EventTime { get; set; }
        public CallEventType EventType { get; set; }

        public static IEnumerable<HourBasedCallEvent> GetOrderedEvents(IList<CallHistory> callHistories,
            Guid locationId)
        {
            return GetEvents(callHistories, locationId).OrderBy(e => e.EventTime).ThenBy(e => (int)e.EventType);
        }

        public static IEnumerable<HourBasedCallEvent> GetEvents(IList<CallHistory> callHistories, Guid locationId)
        {
            foreach (var call in callHistories)
            {
                if (call.FromLocationId == locationId || call.ToLocationId == locationId)
                {
                    yield return
                        Create(CallEventType.Start, call);
                    yield return
                        Create(CallEventType.End, call);
                }
            }
        }

        public static HourBasedCallEvent Create(CallEventType eventType, CallHistory call)
        {
            return new HourBasedCallEvent
            {
                EventTime = eventType == CallEventType.Start ? call.Started : call.Ended,
                EventType = eventType,
            };
        }
    }

    public class HourBasedStatisticsForLocation
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public IList<HourBasedStatistics> Statistics { get; set; }

        public IEnumerable<IList<HourBasedStatistics>> GetSeries()
        {
            if (Statistics == null || Statistics.Count == 0)
                yield break;

            var currentSeries = new List<HourBasedStatistics>();
            var currentDate = DateTime.MinValue;
            foreach (var stats in Statistics)
            {
                var date = stats.Date.ToLocalTime().Date;
                if (currentDate != date)
                {
                    if (currentSeries.Count > 0)
                    {
                        yield return currentSeries;
                        currentSeries = new List<HourBasedStatistics>();
                    }
                    currentDate = date;
                }
                currentSeries.Add(stats);
            }
            if (currentSeries.Count > 0)
                yield return currentSeries;
        }
    }
}