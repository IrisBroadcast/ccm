using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly StatsDbContext _statsDbContext;

        public OwnerRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }
        public List<OwnerEntity> GetAll()
        {
            return _statsDbContext.Owners.ToList();
        }
    }
}
