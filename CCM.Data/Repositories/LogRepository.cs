using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        public LogRepository(IAppCache cache) : base(cache)
        {
        }

        public async Task<List<Log>> GetLastAsync(int nrOfRows, string application, DateTime? startTime, DateTime? endTime, int minLevel, string search, Guid activityId)
        {
            using (var db = GetDbContext())
            {
                List<LogEntity> dbLogRows;

                if (activityId != Guid.Empty)
                {
                    dbLogRows = await db.Logs
                        .Where(l => l.ActivityId == activityId)
                        .OrderByDescending(l => l.Id)
                        .ToListAsync();
                }
                else
                {
                    IQueryable<LogEntity> dbLogQuery = db.Logs.Where(l => l.LevelValue >= minLevel);

                    if (!string.IsNullOrEmpty(application))
                    {
                        dbLogQuery = dbLogQuery.Where(r => r.Application == application);
                    }

                    if (startTime != null)
                    {
                        dbLogQuery = dbLogQuery.Where(r => r.Date >= startTime.Value);
                    }

                    if (endTime != null)
                    {
                        dbLogQuery = dbLogQuery.Where(r => r.Date <= endTime.Value);
                    }

                    if (!string.IsNullOrEmpty(search))
                    {
                        dbLogQuery = dbLogQuery.Where(r => r.Message.Contains(search));
                    }

                    dbLogRows = await dbLogQuery
                        .OrderByDescending(l => l.Id)
                        .Take(nrOfRows)
                        .ToListAsync();
                }

                return dbLogRows.Select(r => new Log
                {
                    Id = r.Id,
                    Date = r.Date,
                    Level = r.Level,
                    Callsite = r.Callsite,
                    Message = r.Message,
                    Exception = r.Exception,
                    Application = r.Application,
                    ActivityId = r.ActivityId
                }).ToList();
            }
        }

        public async Task<IList<LogInfo>> GetLogInfoAsync()
        {
            using (var db = GetDbContext())
            {
                var logInfo = await db.Logs
                    .GroupBy(l => l.Application)
                    .Select(g => new LogInfo() { Application = g.Key, Count = g.Count(), MinDate = g.Min(x => x.Date) })
                    .ToListAsync();

                return logInfo;
            }
        }

        public void DeleteOldest(int nrOfRowsToDelete = 100)
        {
            using (var db = GetDbContext())
            {
                var sql = $"DELETE FROM `Logs` ORDER BY Id LIMIT {nrOfRowsToDelete};";
                db.Database.ExecuteSqlCommand(sql);
            }
        }
    }
}