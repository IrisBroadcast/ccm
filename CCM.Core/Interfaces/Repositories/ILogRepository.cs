using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ILogRepository
    {
        Task<List<Log>> GetLastAsync(int nrOfRows, string application, DateTime? startTime, DateTime? endTime, int minLevel, string search, Guid activityId);
        Task<IList<LogInfo>> GetLogInfoAsync();
        void DeleteOldest(int nrOfRowsToDelete = 100);
    }
}