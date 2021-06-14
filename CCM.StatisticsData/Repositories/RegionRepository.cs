using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly StatsDbContext _statsDbContext;

        public RegionRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }

        public List<RegionEntity> GetAll()
        {
            var dbRegions = _statsDbContext.Regions
                .Include(r => r.Locations)
                .ToList();
            return dbRegions;
        }
    }
}
