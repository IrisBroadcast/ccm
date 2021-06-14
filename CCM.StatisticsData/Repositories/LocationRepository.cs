using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly StatsDbContext _statsDbContext;

        public LocationRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }
        public List<LocationEntity> GetAll()
        {
            var dbLocations = _statsDbContext.Locations
           .Include(l => l.City)
           .Include(l => l.Region)
           .Include(l => l.ProfileGroup)
           .ToList();

            return dbLocations;
        }
    }
}
