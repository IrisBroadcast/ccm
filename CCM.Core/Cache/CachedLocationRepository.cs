using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using LazyCache;

namespace CCM.Core.Cache
{
    public class CachedLocationRepository : ILocationRepository
    {
        private readonly IAppCache _lazyCache;
        private readonly ILocationRepository _internalRepository;

        public CachedLocationRepository(IAppCache lazyCache, ILocationRepository internalRepository)
        {
            _lazyCache = lazyCache;
            _internalRepository = internalRepository;
        }

        public Location GetById(Guid id) { return _internalRepository.GetById(id); }
        public List<Location> GetAll() { return _internalRepository.GetAll(); }
        public List<Location> FindLocations(string searchString) { return _internalRepository.FindLocations(searchString); }

        public void Save(Location location)
        {
            _internalRepository.Save(location);
            _lazyCache.ResetLocationNetworks();
        }

        public void Delete(Guid id)
        {
            _internalRepository.Delete(id);
            _lazyCache.ResetLocationNetworks();
        }

        public List<LocationNetwork> GetAllLocationNetworks() { 
            return _lazyCache.GetOrAddLocationNetworks( () => _internalRepository.GetAllLocationNetworks() );
        }
    }
}