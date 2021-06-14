using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class CallHistoryRepository : ICallHistoryRepository
    {
        private readonly StatsDbContext _statsDbContext;

        public CallHistoryRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }


        public List<CallHistoryEntity> GetCallHistoriesByDate(DateTime startTime, DateTime endTime)
        {
            return GetFiltered(c => c.Started < endTime && c.Ended >= startTime);
        }
        private List<CallHistoryEntity> GetFiltered(Expression<Func<CallHistoryEntity, bool>> filterExpression)
        {
            var dbCallHistories = _statsDbContext.CallHistories
                .AsNoTracking()
                .Where(filterExpression)
                .ToList();
            return dbCallHistories.ToList();
        }
        public List<CallHistoryEntity> GetCallHistoriesForCodecType(DateTime startDate, DateTime endDate, Guid codecTypeId)
        {
            return codecTypeId == Guid.Empty
                ? GetFiltered(c => c.Started < endDate && c.Ended >= startDate)
                : GetFiltered(c => c.Started < endDate && c.Ended >= startDate && (c.FromCodecTypeId == codecTypeId || c.ToCodecTypeId == codecTypeId));
        }

        public List<CallHistoryEntity> GetCallHistoriesForRegion(DateTime startDate, DateTime endDate, Guid regionId)
        {
            return regionId == Guid.Empty ?
                GetFiltered(c => c.Started < endDate && c.Ended >= startDate) :
                GetFiltered(c => c.Started < endDate && c.Ended >= startDate && (c.FromRegionId == regionId || c.ToRegionId == regionId));
        }
        public List<CallHistoryEntity> GetCallHistoriesForRegisteredSip(DateTime startTime, DateTime endTime, string sipId)
        {
            return GetFiltered(c => c.Started < endTime && c.Ended >= startTime && (c.FromSip == sipId || c.ToSip == sipId));
        }

        public SipAccountEntity GetSipById(Guid sipId)
        {
                var db = _statsDbContext.SipAccounts
                    .Include(x => x.Owner)
                    .Include(x => x.CodecType)
                    .SingleOrDefault(u => u.Id == sipId);
            return db;
         
        }
    }
}
