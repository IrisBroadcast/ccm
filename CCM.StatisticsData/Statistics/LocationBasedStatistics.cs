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

using CCM.StatisticsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCM.StatisticsData.Statistics
{
    public class LocationBasedStatistics
    {
        private readonly HashSet<DateTime> _maxSimultaneousEventDates = new HashSet<DateTime>();

        public double AverageTime
        {
            get { return NumberOfCalls == 0 ? 0 : TotaltTimeForCalls / NumberOfCalls; }
        }

        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public double MaxCallTime { get; private set; }
        public double MinCallTime { get; private set; }
        public int NumberOfCalls { get; private set; }
        public double TotaltTimeForCalls { get; private set; }
        public int MaxSimultaneousCalls { get; private set; }
        public int OngoingCalls { get; private set; }
        public List<CodecTypeData> CodecTypeDataList { get; set; } = new List<CodecTypeData>();



        public IEnumerable<DateTime> MaxSimultaneousEventDates
        {
            get
            {
                return _maxSimultaneousEventDates.OrderBy(d => d);
            }
        }

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

            var fromCodec = MapToCodecType(callEvent.FromCodecTypeId, callEvent.FromCodecTypeName);
            GetCodecTypeData(fromCodec);

            var toCodec = MapToCodecType(callEvent.ToCodecTypeId, callEvent.ToCodecTypeName);
            GetCodecTypeData(toCodec);

        }

        public CodecTypeData MapToCodecType(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                name = "Ospecificerad";
            }

            return new CodecTypeData
            {
                Id = id,
                Name = name,
                NumberOfCalls = 1
            };
        }

        public void GetCodecTypeData(CodecTypeData codec)
        {
            if (!CodecTypeDataList.Any(x => x.Id == codec.Id))
            {
                CodecTypeDataList.Add(codec);
            }
            else
            {
                foreach (var item in CodecTypeDataList)
                {
                    if (item.Id == codec.Id)
                    {
                        item.NumberOfCalls++;
                    }
                }
                //CodecTypeDataList = CodecTypeDataList.Where(x => x.Id == codec.Id).Select(x => { x.NumberOfCalls++; return x; }).ToList();
            }
            //return CodecTypeDataList;
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
        public CodecTypeData CodecTypeData { get; set; }
        //public List<CodecTypeData> CodecTypeDatas { get; set; }
        public Guid FromCodecTypeId { get; set; }
        public string FromCodecTypeName { get; set; }
        public Guid ToCodecTypeId { get; set; }
        public string ToCodecTypeName { get; set; }

        public static IEnumerable<LocationCallEvent> GetOrderedEvents(IList<CallHistoryEntity> callHistories,
            CallHistoryFilter filter)
        //public static IEnumerable<LocationCallEvent> GetOrderedEvents(IList<CallHistoryEntity> callHistories,
        //    CallHistoryFilter filter, List<CodecTypeData> codecTypeDatas)
        {
            return GetEvents(callHistories, filter).OrderBy(e => e.EventTime).ThenBy(e => (int)e.EventType);
            //return GetEvents(callHistories, filter, codecTypeDatas).OrderBy(e => e.EventTime).ThenBy(e => (int)e.EventType);
        }

        public static IEnumerable<LocationCallEvent> GetEvents(IList<CallHistoryEntity> callHistories, CallHistoryFilter filter)
        //public static IEnumerable<LocationCallEvent> GetEvents(IList<CallHistoryEntity> callHistories, CallHistoryFilter filter, List<CodecTypeData> codecTypeDatas)

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

        public static LocationCallEvent Create(CallEventType eventType, CallHistoryEntity call, Func<CallHistoryEntity, Tuple<Guid, string>> locationSelector)
        {
            var location = locationSelector(call);
            return new LocationCallEvent
            {
                EventTime = eventType == CallEventType.Start ? call.Started : call.Ended,
                EventType = eventType,
                EndTime = call.Ended,
                LocationId = location.Item1,
                LocationName = location.Item2,
                StartTime = call.Started,
                // CodecTypeData = GetCodecData(call, codecTypesDatas)
                FromCodecTypeId = call.FromCodecTypeId,
                ToCodecTypeId = call.ToCodecTypeId,
                FromCodecTypeName = call.FromCodecTypeName,
                ToCodecTypeName = call.ToCodecTypeName
            };
        }
        //    public static CodecTypeData GetCodecData(CallHistoryEntity call, List<CodecTypeData> codecTypeDatas)
        //    {
        //        if (!codecTypeDatas.Any(x => x.Id == call.FromCodecTypeId))
        //        {
        //            codecTypeDatas.Add(new CodecTypeData
        //            {
        //                Id = call.FromCodecTypeId,
        //                Name = call.FromCodecTypeName,
        //                NumberOfCalls = 1
        //            });
        //            return codecTypeDatas.FirstOrDefault(x => x.Id == call.FromCodecTypeId);
        //        }
        //        else
        //        {
        //            foreach (var item in codecTypeDatas.Where(x => x.Id == call.FromCodecTypeId))
        //            {
        //                item.NumberOfCalls++;
        //                return codecTypeDatas.FirstOrDefault(x => x.Id == call.FromCodecTypeId);

        //            }
        //        }

        //        if (!codecTypeDatas.Any(x => x.Id == call.ToCodecTypeId))
        //        {
        //            codecTypeDatas.Add(new CodecTypeData
        //            {
        //                Id = call.ToCodecTypeId,
        //                Name = call.ToCodecTypeName,
        //                NumberOfCalls = 1
        //            });
        //            return codecTypeDatas.FirstOrDefault(x => x.Id == call.ToCodecTypeId);
        //        }
        //        else
        //        {
        //            foreach (var item in codecTypeDatas.Where(x => x.Id == call.ToCodecTypeId))
        //            {
        //                item.NumberOfCalls++;
        //                return codecTypeDatas.FirstOrDefault(x => x.Id == call.ToCodecTypeId);
        //            }
        //        }
        //        return new CodecTypeData();
        //    }

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
        // public Guid CodecTypeId { get; private set; }

        //public CallHistoryFilter(Guid regionId, Guid ownerId, Guid codecTypeId)
        //{
        //    RegionId = regionId;
        //    OwnerId = ownerId;
        //    CodecTypeId = codecTypeId;
        //}

        public CallHistoryFilter(Guid regionId, Guid ownerId)
        {
            RegionId = regionId;
            OwnerId = ownerId;
        }

        public bool IsMatch(CallHistoryEntity callHistory)
        {
            return IsFromMatch(callHistory) || IsToMatch(callHistory);
        }

        public bool IsFromMatch(CallHistoryEntity callHistory)
        {
            return callHistory.FromLocationId != Guid.Empty
                   && (RegionId == Guid.Empty || callHistory.FromRegionId == RegionId)
                   && (OwnerId == Guid.Empty || callHistory.FromOwnerId == OwnerId);
            // && (CodecTypeId == Guid.Empty || callHistory.FromCodecTypeId == CodecTypeId);
        }

        public bool IsToMatch(CallHistoryEntity callHistory)
        {
            return callHistory.ToLocationId != Guid.Empty
                   && (RegionId == Guid.Empty || callHistory.ToRegionId == RegionId)
                   && (OwnerId == Guid.Empty || callHistory.ToOwnerId == OwnerId);
            //&& (CodecTypeId == Guid.Empty || callHistory.ToCodecTypeId == CodecTypeId);
        }
    }

    public class CodecTypeData
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public int NumberOfCalls { get; set; }
    }
}
