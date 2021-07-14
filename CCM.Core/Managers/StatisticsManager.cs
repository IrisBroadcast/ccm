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
using CCM.Core.Entities;
using CCM.Core.Entities.Statistics;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Core.Managers
{

    public class StatisticsManager : IStatisticsManager
    {
        private readonly ICachedCallHistoryRepository _cachedCallHistoryRepository;
        private readonly ICodecTypeRepository _codecTypeRepository;
        private readonly ICachedLocationRepository _cachedLocationRepository;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICachedSipAccountRepository _cachedSipAccountRepository;

        public StatisticsManager(
            ICachedCallHistoryRepository cachedCallHistoryRepository,
            ICodecTypeRepository codecTypeRepository,
            IOwnersRepository ownersRepository,
            IRegionRepository regionRepository,
            ICachedLocationRepository cachedLocationRepository,
            ICachedSipAccountRepository cachedSipAccountRepository)
        {
            _cachedCallHistoryRepository = cachedCallHistoryRepository;
            _codecTypeRepository = codecTypeRepository;
            _ownersRepository = ownersRepository;
            _regionRepository = regionRepository;
            _cachedLocationRepository = cachedLocationRepository;
            _cachedSipAccountRepository = cachedSipAccountRepository;
        }

        public List<CodecType> GetCodecTypes()
        {
            return _codecTypeRepository.GetAll(false);
        }

        public List<LocationBasedStatistics> GetLocationStatistics(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId, Guid codecTypeId)
        {
            var callHistories = _cachedCallHistoryRepository.GetCallHistoriesByDate(startTime, endTime);

            if (callHistories == null)
            {
                return new List<LocationBasedStatistics>();
            }

            var locations = new Dictionary<Guid, LocationBasedStatistics>();
            var filter = new CallHistoryFilter(regionId, ownerId, codecTypeId);
            foreach (var callEvent in LocationCallEvent.GetOrderedEvents(callHistories, filter))
            {
                if (!locations.ContainsKey(callEvent.LocationId))
                    locations.Add(callEvent.LocationId, new LocationBasedStatistics { LocationId = callEvent.LocationId, LocationName = callEvent.LocationName});
                locations[callEvent.LocationId].AddEvent(callEvent, startTime, endTime);
            }

            var locationStatisticses = locations.Values.ToList();
            AddMissingLocations(locationStatisticses, regionId);

            return locationStatisticses.OrderBy(l => l.LocationName).ToList();
        }

        public List<Owner> GetOwners()
        {
            return _ownersRepository.GetAll();
        }

        public List<Region> GetRegions()
        {
            return _regionRepository.GetAll();
        }

        public IEnumerable<DateBasedCategoryStatistics> GetCategoryStatistics(DateTime startTime, DateTime endTime)
        {

            var categoryStatistics = new List<DateBasedCategoryStatistics>();
            var callHistory = _cachedCallHistoryRepository.GetCallHistoriesByDate(startTime, endTime);

            if (callHistory == null)
            {
                return categoryStatistics;
            }

            categoryStatistics.AddRange(GenerateDateBasedCategoryStatistics(callHistory, startTime, endTime));

            return categoryStatistics;
        }

        private IEnumerable<DateBasedCategoryStatistics> GenerateDateBasedCategoryStatistics(IList<CallHistory> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            if (!callHistories.Any()) return Enumerable.Empty<DateBasedCategoryStatistics>();

            var minDate = reportPeriodStart.ToLocalTime().Date; // callHistories.Min(c => c.Started);
            var endLocal = reportPeriodEnd.ToLocalTime();
            var maxDate = endLocal.Date == endLocal ? endLocal.Date.AddDays(-1) : endLocal.Date; // callHistories.Max(c => c.Started.ToLocalTime().Date);
            var dateList = new Dictionary<DateTime, DateBasedCategoryStatistics>();
            var dateBasedStatistics = new Dictionary<DateTime, RegionCategory>();

            var date = minDate;

            while (date <= maxDate)
            {
                dateList.Add(date, new DateBasedCategoryStatistics { Date = date });
                dateBasedStatistics.Add(date, new RegionCategory());
                date = date.AddDays(1.0);
            }

            var regions = _regionRepository.GetAll();
            IList<CallHistory> regionHistory = new List<CallHistory>();

            //List<RegionCategory> RegionCategoriesList = new List<RegionCategory>();

            foreach (var region in regions)
            {
                regionHistory = _cachedCallHistoryRepository.GetCallHistoriesForRegion(reportPeriodStart, reportPeriodEnd, region.Id);

                foreach (var callEvent in DateBasedCategoryCallEvent.GetEvents(regionHistory, reportPeriodStart, reportPeriodEnd))
                {
                    if (!dateList.ContainsKey(callEvent.Date)) continue;

                    if (!dateList[callEvent.Date].RegionCategories.Any(x => x.Name == region.Name))
                    {

                        dateList[callEvent.Date].RegionCategories.Add(
                            new RegionCategory
                            {
                                Name = region.Name,
                                Date = callEvent.Date,
                                CategoryStatisticsList = dateBasedStatistics[callEvent.Date].GetCategoryData(callEvent, callEvent.Duration)

                            });
                    }
                    else
                    {
                        foreach (var item in dateList[callEvent.Date].RegionCategories)
                        {
                            if (item.Name == region.Name && item.Date == callEvent.Date)
                            {
                                item.Date = callEvent.Date;
                                item.CategoryStatisticsList = dateBasedStatistics[callEvent.Date].GetCategoryData(callEvent, callEvent.Duration);
                            }
                        }
                    }
                }
            }
            return dateList.Values.OrderBy(d => d.Date);
        }

        public IList<DateBasedStatistics> GetRegionStatistics(DateTime startDate, DateTime endDate, Guid regionId)
        {
            var callHistories = _cachedCallHistoryRepository.GetCallHistoriesForRegion(startDate, endDate, regionId);

            if (callHistories == null)
            {
                return new List<DateBasedStatistics>();
            }

            var regionStatistics = GenerateDateBasedStatisticses(callHistories, startDate, endDate);
            return regionStatistics.OrderBy(r => r.Date).ToList();
        }

        public List<SipAccount> GetSipUsers()
        {
            return _cachedSipAccountRepository.GetAll();
        }

        public IList<DateBasedStatistics> GetSipAccountStatistics(DateTime startDate, DateTime endDate, Guid userId)
        {
            var user = _cachedSipAccountRepository.GetById(userId);
            var callHistories = user != null ? _cachedCallHistoryRepository.GetCallHistoriesForRegisteredSip(startDate, endDate, user.UserName) : new List<CallHistory>();

            var sipStatistics = GenerateDateBasedStatisticses(callHistories, startDate, endDate)
                .OrderBy(s => s.Date)
                .ToList();

            return sipStatistics;
        }

        public IList<DateBasedStatistics> GetCodecTypeStatistics(DateTime startDate, DateTime endDate, Guid codecTypeId)
        {
            var codecTypeStatistics = new List<DateBasedStatistics>();

            var callHistories = _cachedCallHistoryRepository.GetCallHistoriesForCodecType(startDate, endDate, codecTypeId);

            if (callHistories == null)
            {
                return codecTypeStatistics;
            }

            codecTypeStatistics.AddRange(GenerateDateBasedStatisticses(callHistories, startDate, endDate));

            return codecTypeStatistics.OrderBy(c => c.Date).ToList();
        }

        public IList<Location> GetLocationsForRegion(Guid regionId)
        {
            return
                _cachedLocationRepository.GetAll()
                    .Where(l => regionId == Guid.Empty || (l.Region != null && l.Region.Id == regionId))
                    .ToList();
        }

        public HourBasedStatisticsForLocation GetHourStatisticsForLocation(DateTime startTime, DateTime endTime, Guid locationId, bool noAggregation)
        {
            var location = _cachedLocationRepository.GetById(locationId);
            if (location == null)
            {
                return new HourBasedStatisticsForLocation
                {
                    LocationId = locationId,
                    LocationName = "",
                    Statistics = new List<HourBasedStatistics>()
                };
            }
            var allStats = new List<HourBasedStatistics>();
            var callHistories = _cachedCallHistoryRepository.GetCallHistoriesForLocation(startTime, endTime, locationId);
            var current = HourBasedStatistics.Create(startTime);
            allStats.Add(current);
            foreach (var callEvent in HourBasedCallEvent.GetOrderedEvents(callHistories, locationId).Where(e => e.EventTime < endTime))
            {
                while (!current.AddEvent(callEvent))
                {
                    current = current.GetNext();
                    allStats.Add(current);
                }
            }

            return new HourBasedStatisticsForLocation
            {
                LocationId = locationId,
                LocationName = location.Name,
                Statistics = noAggregation ? allStats : HourBasedStatistics.Aggregate(allStats)
            };
        }

        private void AddMissingLocations(IList<LocationBasedStatistics> locationStatisticses, Guid regionId)
        {
            var locations = _cachedLocationRepository.GetAll();

            foreach (var location in locations)
            {
                if (locationStatisticses.All(l => l.LocationId != location.Id))
                {
                    if (regionId == Guid.Empty || (location.Region != null && location.Region.Id == regionId))
                    {
                        locationStatisticses.Add(new LocationBasedStatistics() { LocationName = location.Name, LocationId = location.Id });
                    }
                }
            }
        }

        private IEnumerable<DateBasedStatistics> GenerateDateBasedStatisticses(IList<CallHistory> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            if (!callHistories.Any()) return Enumerable.Empty<DateBasedStatistics>();

            var minDate = reportPeriodStart.ToLocalTime().Date; // callHistories.Min(c => c.Started);
            var endLocal = reportPeriodEnd.ToLocalTime();
            var maxDate = endLocal.Date == endLocal ? endLocal.Date.AddDays(-1) : endLocal.Date; // callHistories.Max(c => c.Started.ToLocalTime().Date);

            var dateBasedStatisticses = new Dictionary<DateTime, DateBasedStatistics>();

            var date = minDate;
            while (date <= maxDate)
            {
                dateBasedStatisticses.Add(date, new DateBasedStatistics { Date = date });
                date = date.AddDays(1.0);
            }

            foreach (var callEvent in DateBasedCallEvent.GetEvents(callHistories, reportPeriodStart, reportPeriodEnd))
            {
                if (!dateBasedStatisticses.ContainsKey(callEvent.Date)) continue;
                dateBasedStatisticses[callEvent.Date].AddTime(callEvent.Duration);
            }

            return dateBasedStatisticses.Values.OrderBy(d => d.Date);
        }
    }
}
