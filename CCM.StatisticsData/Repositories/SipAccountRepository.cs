using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class SipAccountRepository : ISipAccountRepository
    {
        private readonly StatsDbContext _statsDbContext;
        public SipAccountRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }

        public List<SipAccountEntity> GetAll()
        {
            return _statsDbContext.SipAccounts
                //.Include(u => u.Owner)
                //.Include(u => u.CodecType)
                //.ToList()
                //.OrderBy(u => u.UserName)
                .ToList();
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
