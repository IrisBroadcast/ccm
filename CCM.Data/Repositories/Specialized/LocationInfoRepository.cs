using System.Collections.Generic;
using System.Linq;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories.Specialized;
using LazyCache;

namespace CCM.Data.Repositories.Specialized
{
    public class LocationInfoRepository : BaseRepository, ILocationInfoRepository
    {
        public LocationInfoRepository(IAppCache cache) : base(cache)
        {
        }

        public List<LocationInfo> GetAll()
        {
            using (var db = GetDbContext())
            {
                return db.Locations.Select(l => new LocationInfo()
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToList();
            }
        }
    }
}