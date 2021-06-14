using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Pages;
using CCM.StatisticsWeb.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Services
{
    public interface IStatisticsDataService
    {
        Task<IEnumerable<Region>> GetRegions();
        Task<IEnumerable<CodecType>> GetCodecTypes();
        Task<IEnumerable<Owner>> GetOwners();
        Task<IEnumerable<SipAccount>> GetSipAccounts();
        //Task <LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid ownerId, Guid codecTypeId, DateTime startTime, DateTime endTime);
        Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid ownerId, DateTime startTime, DateTime endTime);

        Task<IEnumerable<DateBasedStatistics>> GetCodecTypeStatistics(Guid codecTypeId, DateTime startTime, DateTime endTime);
        Task<IEnumerable<DateBasedStatistics>> GetRegionStatistics(Guid regionId, DateTime startTime, DateTime endTime);
        Task<IEnumerable<DateBasedStatistics>> GetSipStatistics(Guid sipId, DateTime startTime, DateTime endTime);
        Task<IEnumerable<DateBasedCategoryStatistics>> GetCategories(DateTime startTime, DateTime endTime);
    }
}
