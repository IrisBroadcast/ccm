using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Core.Entities.Statistics
{
    public class LocationStatistics
    {
        private readonly HashSet<DateTime> _maxSimultaneousEventDates = new HashSet<DateTime>();

        public double AverageTime
        {
            get { return NumberOfCalls == 0 ? 0 : TotaltTimeForCalls/this.NumberOfCalls; }
        }

        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public double MaxCallTime { get; private set; }
        public double MinCallTime { get; private set; }
        public int NumberOfCalls { get; private set; }
        public double TotaltTimeForCalls { get; private set; }
        public int MaxSimultaneousCalls { get; private set; }
        public int OngoingCalls { get; private set; }

        public IEnumerable<DateTime> MaxSimultaneousEventDates
        {
            get
            {
                return _maxSimultaneousEventDates.OrderBy(d => d);
            }
        }

        //public void AddTime(double timeInMinutes)
        //{
        //    if (timeInMinutes < MinCallTime && NumberOfCalls > 0)
        //    {
        //        MinCallTime = timeInMinutes;
        //    }
        //    else if (NumberOfCalls == 0)
        //    {
        //        MinCallTime = timeInMinutes;
        //    }

        //    if (timeInMinutes > MaxCallTime)
        //    {
        //        MaxCallTime = timeInMinutes;
        //    }
            
        //    TotaltTimeForCalls += timeInMinutes;
        //    NumberOfCalls++;
        //}

        public void AddEvent(LocationCallEvent callEvent, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            if (callEvent.EventType == CallEventType.Start)
            {
                OngoingCalls++;
                if (OngoingCalls == MaxSimultaneousCalls)
                {
                    _maxSimultaneousEventDates.Add(callEvent.StartTime.ToLocalTime().Date);
                }
                else if (OngoingCalls > MaxSimultaneousCalls)
                {
                    _maxSimultaneousEventDates.Clear();
                    _maxSimultaneousEventDates.Add(callEvent.StartTime.ToLocalTime().Date);
                    MaxSimultaneousCalls = OngoingCalls;
                }
                return;
            }
            OngoingCalls--;
            NumberOfCalls++;

            var duration = (callEvent.EndTime - callEvent.StartTime).TotalMinutes;

            if (NumberOfCalls == 1 || duration < MinCallTime)
            {
                MinCallTime = duration;
            }
            if (duration > MaxCallTime)
            {
                MaxCallTime = duration;
            }
            var durationInReportPeriod = ((callEvent.EndTime > reportPeriodEnd ? reportPeriodEnd : callEvent.EndTime) -
                           (callEvent.StartTime < reportPeriodStart ? reportPeriodStart : callEvent.StartTime)).TotalMinutes;

            TotaltTimeForCalls += durationInReportPeriod;
        }
    }

    public class LocationCallEvent
    {
        public DateTime EventTime { get; set; }
        public CallEventType EventType { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public static IEnumerable<LocationCallEvent> GetOrderedEvents(IList<CallHistory> callHistories,
            CallHistoryFilter filter)
        {
            return GetEvents(callHistories, filter).OrderBy(e => e.EventTime).ThenBy(e => (int)e.EventType);
        }

        public static IEnumerable<LocationCallEvent> GetEvents(IList<CallHistory> callHistories, CallHistoryFilter filter)
        {
            foreach (var call in callHistories)
            {
                if ((call.FromLocationId == call.ToLocationId && filter.IsMatch(call)) || filter.IsFromMatch(call))
                {
                    yield return
                        Create(CallEventType.Start, call, c => Tuple.Create(c.FromLocationId, c.FromLocationName));
                    yield return
                        Create(CallEventType.End, call, c => Tuple.Create(c.FromLocationId, c.FromLocationName));
                }
                if (call.FromLocationId != call.ToLocationId && filter.IsToMatch(call))
                {
                    yield return
                        Create(CallEventType.Start, call, c => Tuple.Create(c.ToLocationId, c.ToLocationName));
                    yield return
                        Create(CallEventType.End, call, c => Tuple.Create(c.ToLocationId, c.ToLocationName));
                }
            }
        }

        public static LocationCallEvent Create(CallEventType eventType, CallHistory call, Func<CallHistory, Tuple<Guid, string>> locationSelector)
        {
            var location = locationSelector(call);
            return new LocationCallEvent
            {
                EventTime = eventType == CallEventType.Start ? call.Started : call.Ended,
                EventType = eventType,
                EndTime = call.Ended,
                LocationId = location.Item1,
                LocationName = location.Item2,
                StartTime = call.Started
            };
        }

    }

    public enum CallEventType
    {
        End = 0,
        Start = 1
    }

    public class CallHistoryFilter
    {
        public Guid RegionId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid CodecTypeId { get; private set; }

        public CallHistoryFilter(Guid regionId, Guid ownerId, Guid codecTypeId)
        {
            RegionId = regionId;
            OwnerId = ownerId;
            CodecTypeId = codecTypeId;
        }

        public bool IsMatch(CallHistory callHistory)
        {
            return IsFromMatch(callHistory) || IsToMatch(callHistory);
        }

        public bool IsFromMatch(CallHistory callHistory)
        {
            return callHistory.FromLocationId != Guid.Empty 
                   && (RegionId == Guid.Empty || callHistory.FromRegionId == RegionId)
                   && (OwnerId == Guid.Empty || callHistory.FromOwnerId == OwnerId)
                   && (CodecTypeId == Guid.Empty || callHistory.FromCodecTypeId == CodecTypeId);
        }

        public bool IsToMatch(CallHistory callHistory)
        {
            return callHistory.ToLocationId != Guid.Empty
                   && (RegionId == Guid.Empty || callHistory.ToRegionId == RegionId)
                   && (OwnerId == Guid.Empty || callHistory.ToOwnerId == OwnerId)
                   && (CodecTypeId == Guid.Empty || callHistory.ToCodecTypeId == CodecTypeId);
        }
    }
}