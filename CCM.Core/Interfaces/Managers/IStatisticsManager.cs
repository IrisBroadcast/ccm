using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Statistics;

namespace CCM.Core.Interfaces.Managers
{
    public interface IStatisticsManager
    {
        List<CodecType> GetCodecTypes();
        List<LocationStatistics> GetLocationStatistics(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId, Guid codecTypeId);
        List<Owner> GetOwners();
        List<Region> GetRegions();
        List<DateBasedStatistics> GetRegionStatistics(DateTime startDate, DateTime endDate, Guid regionId);
        List<SipAccount> GetSipUsers();
        List<DateBasedStatistics> GetSipStatistics(DateTime startDate, DateTime endDate, Guid userId);
        IList<DateBasedStatistics> GetCodecTypeStatistics(DateTime startDate, DateTime endDate, Guid codecTypeId);
        IList<Location> GetLocationsForRegion(Guid regionId);
        HourBasedStatisticsForLocation GetHourStatisticsForLocation(DateTime startTime, DateTime endTime, Guid locationId, bool noAggregation);
    }
}