using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using CCM.StatisticsData.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly StatsDbContext _statsDbContext;
        private readonly ICallHistoryRepository _callHistoryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly IRegionRepository _regionRepository;

        public StatisticsRepository(StatsDbContext statsDbContext,
            ICallHistoryRepository callHistoryRepository,
            ILocationRepository locationRepository,
            ISipAccountRepository sipAccountRepository,
            IRegionRepository regionRepository)
        {
            _statsDbContext = statsDbContext;
            _callHistoryRepository = callHistoryRepository;
            _locationRepository = locationRepository;
            _sipAccountRepository = sipAccountRepository;
            _regionRepository = regionRepository;
        }


        public IEnumerable<DateBasedCategoryStatistics> GetCategoryStatistics(DateTime startTime, DateTime endTime)
        {

            var categoryStatistics = new List<DateBasedCategoryStatistics>();
            var callHistory = _callHistoryRepository.GetCallHistoriesByDate(startTime, endTime);

            if (callHistory == null)
            {
                return categoryStatistics;
            }

            categoryStatistics.AddRange(GenerateDateBasedCategoryStatistics(callHistory, startTime, endTime));

            return categoryStatistics;
        }

        public IEnumerable<DateBasedStatistics> GetCodecTypeStatistics(DateTime startTime, DateTime endTime, Guid codecTypeId)
        {
            var codecTypeStatistics = new List<DateBasedStatistics>();
            var callHistory = _callHistoryRepository.GetCallHistoriesForCodecType(startTime, endTime, codecTypeId);

            if (callHistory == null)
            {
                return codecTypeStatistics;
            }
            codecTypeStatistics.AddRange(GenerateDateBasedStatisticses(callHistory, startTime, endTime));

            return codecTypeStatistics.OrderBy(c => c.Date).ToList();

        }

        public IEnumerable<LocationBasedStatistics> GetLocationStatistics(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId)
        {
            var callHistory = _callHistoryRepository.GetCallHistoriesByDate(startTime, endTime);

            if (callHistory == null)
            {
                return new List<LocationBasedStatistics>();
            }
            var locations = new Dictionary<Guid, LocationBasedStatistics>();
            var filter = new CallHistoryFilter(regionId, ownerId);
            foreach (var callEvent in LocationCallEvent.GetOrderedEvents(callHistory, filter))
            {
                if (!locations.ContainsKey(callEvent.LocationId))
                    locations.Add(callEvent.LocationId, new LocationBasedStatistics { LocationId = callEvent.LocationId, LocationName = callEvent.LocationName });
                locations[callEvent.LocationId].AddEvent(callEvent, startTime, endTime);
            }

            var locationStatistics = locations.Values.ToList();

            AddMissingLocations(locationStatistics, regionId);

            return locationStatistics.OrderBy(l => l.LocationName).ToList();

        }

        public IEnumerable<DateBasedStatistics> GetRegionStatistics(DateTime startTime, DateTime endTime, Guid regionId)
        {
            var callHistories = _callHistoryRepository.GetCallHistoriesForRegion(startTime, endTime, regionId);

            if (callHistories == null)
            {
                return new List<DateBasedStatistics>();
            }

            var regionStatistics = GenerateDateBasedStatisticses(callHistories, startTime, endTime);
            return regionStatistics.OrderBy(r => r.Date).ToList();
        }

        public IEnumerable<DateBasedStatistics> GetSipStatistics(DateTime startTime, DateTime endTime, Guid sipId)
        {
            var user = _sipAccountRepository.GetSipById(sipId);
            var callHistories = user != null ? _callHistoryRepository.GetCallHistoriesForRegisteredSip(startTime, endTime, user.UserName) : new List<CallHistoryEntity>();

            var sipStatistics = GenerateDateBasedStatisticses(callHistories, startTime, endTime)
                .OrderBy(s => s.Date)
                .ToList();

            return sipStatistics;
        }

        private void AddMissingLocations(IList<LocationBasedStatistics> locationStatisticses, Guid regionId)
        {
            var locations = _locationRepository.GetAll();

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

        private IEnumerable<DateBasedStatistics> GenerateDateBasedStatisticses(IList<CallHistoryEntity> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
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

        private IEnumerable<DateBasedCategoryStatistics> GenerateDateBasedCategoryStatistics(IList<CallHistoryEntity> callHistories, DateTime reportPeriodStart, DateTime reportPeriodEnd)
        {
            if (!callHistories.Any()) return Enumerable.Empty<DateBasedCategoryStatistics>();

            var minDate = reportPeriodStart.ToLocalTime().Date; // callHistories.Min(c => c.Started);
            var endLocal = reportPeriodEnd.ToLocalTime();
            var maxDate = endLocal.Date == endLocal ? endLocal.Date.AddDays(-1) : endLocal.Date; // callHistories.Max(c => c.Started.ToLocalTime().Date);
            var dateList = new Dictionary<DateTime, DateBasedCategoryStatistics>();
            var dateBasedStatistics = new Dictionary<DateTime, RegionCategory>();
            var regStats = new RegionCategory();

            var date = minDate;

            while (date <= maxDate)
            {
                dateList.Add(date, new DateBasedCategoryStatistics { Date = date });
                dateBasedStatistics.Add(date, new RegionCategory());
                date = date.AddDays(1.0);
            }

            var regions = _regionRepository.GetAll();
            //List<DateBasedCategoryStatistics> dateList = new List<DateBasedCategoryStatistics>();
            List<CallHistoryEntity> regionHistory = new List<CallHistoryEntity>();

            List<RegionCategory> RegionCategoriesList = new List<RegionCategory>();

            foreach (var region in regions)
            {
                regionHistory = _callHistoryRepository.GetCallHistoriesForRegion(reportPeriodStart, reportPeriodEnd, region.Id);

                foreach (var callEvent in DateBasedCategoryCallEvent.GetEvents(regionHistory, reportPeriodStart, reportPeriodEnd))
                //foreach (var callEvent in DateBasedCategoryCallEvent.GetEvents(callHistories, reportPeriodStart, reportPeriodEnd))
                {
                    //if (!dateBasedStatistics.ContainsKey(callEvent.Date)) continue;
                    // dateBasedStatistics[callEvent.Date].AddTime(callEvent.Duration);
                    if (!dateList.ContainsKey(callEvent.Date)) continue;

                    // TODO If region.Name existerar - addera till dess namne, annars skapa ny?
                    //      if (!RegionCategoriesList.Any(x => x.Name == region.Name))
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
                       // foreach (var item in RegionCategoriesList)
                       foreach (var item in dateList[callEvent.Date].RegionCategories)
                        {
                            if (item.Name == region.Name && item.Date == callEvent.Date)
                            {
                                item.Date = callEvent.Date;
                                item.CategoryStatisticsList = dateBasedStatistics[callEvent.Date].GetCategoryData(callEvent, callEvent.Duration);
                            }
                        }
                    }

                    // RegionCategory reg = new RegionCategory { Name = region.Name, Date = date, CategoryStatisticsList = dateBasedStatistics[callEvent.Date].GetCategoryData(callEvent, callEvent.Duration) };

                    //    dateBasedStatistics[callEvent.Date].GetCategoryData(callEvent, callEvent.Duration);
                    //RegionCategoriesList.Add(reg);
                    // dateList.Add(new DateBasedCategoryStatistics {  RegionCategories = RegionCategoriesList } );
                  //  dateList[callEvent.Date].RegionCategories = RegionCategoriesList;
                }
                // }
            }
            return dateList.Values.OrderBy(d => d.Date);
            //return dateBasedStatistics.Values.OrderBy(d => d.Date);
        }
        
        public List<CallHistoryEntity> GetRegion(IList<CallHistoryEntity> callHistory, RegionEntity region)
        {
            List<CallHistoryEntity> callHistoriesForRegion = new List<CallHistoryEntity>();

            foreach (var call in callHistory)
            {
                if (call.FromRegionId == region.Id)
                {
                    callHistoriesForRegion.Add(call);
                }
                if (call.ToRegionId == region.Id)
                {
                    callHistoriesForRegion.Add(call);
                }
            }
            return callHistoriesForRegion;


            //foreach (var call in callHistory)
            //{


            //    if (!RegionCategories.Any(x => x.Name == call.FromRegionName))
            //    {
            //        RegionCategories.Add(new RegionCategory
            //        {
            //            Name = call.FromRegionName,

            //        });
            //    }

            //    else
            //    {
            //        foreach (var item in CategoryStatisticsList)
            //        {
            //            if (item.Name == fromCategory)
            //            {
            //                item.NumberOfCalls++;
            //                item.TotalTimeForCalls += duration;
            //            }
            //        }
            //    }

            //}

            //return callHistory;
        }

    }
}

