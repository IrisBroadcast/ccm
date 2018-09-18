using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using LazyCache;
using NLog;

namespace CCM.Core.Cache
{
    public class CachedSettingsRepository : ISettingsRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISettingsRepository _internalRepository;
        private readonly IAppCache _lazyCache;

        private const string SettingsKey = "Settings";

        public CachedSettingsRepository(IAppCache cache, ISettingsRepository internalRepository)
        {
            _lazyCache = cache;
            _internalRepository = internalRepository;
        }

        public List<Setting> GetAll()
        {
            return _lazyCache.GetOrAdd(SettingsKey, () => _internalRepository.GetAll());
        }

        public void Save(List<Setting> settings, string userName)
        {
            _internalRepository.Save(settings, userName);
            ClearCache();
        }

        private void ClearCache()
        {
            _lazyCache.Remove(SettingsKey);
        }

    }
}