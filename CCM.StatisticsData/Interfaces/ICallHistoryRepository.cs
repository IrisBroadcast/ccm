using CCM.StatisticsData.Models;
using CCM.StatisticsData.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Interfaces
{
    public interface ICallHistoryRepository
    {
        public List<CallHistoryEntity> GetCallHistoriesByDate(DateTime startTime, DateTime endTime);
        public List<CallHistoryEntity> GetCallHistoriesForCodecType(DateTime startTime, DateTime endTime, Guid codecTypeId);
        public List<CallHistoryEntity> GetCallHistoriesForRegion(DateTime startTime, DateTime endTime, Guid regionId);
        public List<CallHistoryEntity> GetCallHistoriesForRegisteredSip(DateTime startTime, DateTime endTime, string sipId);

        public SipAccountEntity GetSipById(Guid userId);
    }
}
