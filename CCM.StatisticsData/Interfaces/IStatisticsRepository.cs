using CCM.StatisticsData.Models;
using CCM.StatisticsData.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public interface IStatisticsRepository
    {
       // IEnumerable<LocationBasedStatistics> GetLocationStatistics(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId, Guid codecTypeId);
        IEnumerable<LocationBasedStatistics> GetLocationStatistics(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId);

        IEnumerable<DateBasedStatistics> GetCodecTypeStatistics(DateTime startTime, DateTime endTime, Guid codecTypeId);
        IEnumerable<DateBasedStatistics> GetRegionStatistics(DateTime startTime, DateTime endTime, Guid regionId);
        IEnumerable<DateBasedStatistics> GetSipStatistics(DateTime startTime, DateTime endTime, Guid sipId);
        IEnumerable<DateBasedCategoryStatistics> GetCategoryStatistics(DateTime startTime, DateTime endTime);
    }
}
