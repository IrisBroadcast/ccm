using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    public class CityRepository : BaseRepository<City, CityEntity>, ICityRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CityRepository(IAppCache cache) : base(cache)
        {
        }

        public override void Save(City city)
        {
            using (var db = GetDbContext())
            {
                CityEntity dbCity;

                if (city.Id != Guid.Empty)
                {
                    dbCity = db.Cities.SingleOrDefault(g => g.Id == city.Id);
                    if (dbCity == null)
                    {
                        throw new Exception("Region could not be found");
                    }

                    dbCity.Locations.Clear();

                }
                else
                {
                    dbCity = new CityEntity {Locations = new Collection<LocationEntity>()};
                    db.Cities.Add(dbCity);
                }

                dbCity.Name = city.Name;

                foreach (var location in city.Locations)
                {
                    var dbLocation = db.Locations.SingleOrDefault(l => l.Id == location.Id);
                    if (dbLocation != null)
                    {
                        dbCity.Locations.Add(dbLocation);
                    }
                }

                db.SaveChanges();
                city.Id = dbCity.Id;
            }
        }

        public override void Delete(Guid id)
        {
            using (var db = GetDbContext())
            {
                var dbCity = db.Cities.SingleOrDefault(g => g.Id == id);
                if (dbCity != null)
                {
                    dbCity.Locations.Clear();
                    db.Cities.Remove(dbCity);
                    db.SaveChanges();
                }
            }
        }

        public override City GetById(Guid id)
        {
            return GetById(id, c => c.Locations);
        }

        public override List<City> GetAll()
        {
            return GetList(c => true, c => c.Locations, c => c.Name);
        }

        public List<City> Find(string search)
        {
            search = (search ?? string.Empty).ToLower();

            return GetList(
                c => c.Name.ToLower().Contains(search), 
                c => c.Locations, 
                c => c.Name);
        }

        public override City MapToCoreObject(CityEntity dbCity)
        {
            return dbCity != null ? new City {
                    Id = dbCity.Id,
                    Name = dbCity.Name,
                    Locations = dbCity.Locations.Select(MapToLocation).ToList(),
                }
                : null;
        }

        public Location MapToLocation(LocationEntity dbLocation)
        {
            if (dbLocation == null) return null;

            return new Location
            {
                Id = dbLocation.Id,
                Name = dbLocation.Name,
                CreatedBy = dbLocation.CreatedBy,
                CreatedOn = dbLocation.CreatedOn,
                UpdatedBy = dbLocation.UpdatedBy,
                UpdatedOn = dbLocation.UpdatedOn,
            };
        }

    }
}