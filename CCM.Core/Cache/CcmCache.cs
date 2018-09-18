using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Helpers;
using LazyCache;
using NLog;

namespace CCM.Core.Cache
{
    public class CcmCache : ICcmCache
    {
        private readonly IAppCache _cache;

        private const string CachedRegisteredSipsKey = "CachedRegisteredSip_List";
        private const string SettingsKey = "Settings";

        // Cache time in seconds
        public static int CacheTimeCachedRegisteredSips = ApplicationSettings.CacheTimeLiveData;
        public static int CacheTimeOngoingCalls = ApplicationSettings.CacheTimeLiveData;
        public static int CacheTimeOldCalls = ApplicationSettings.CacheTimeLiveData;

        public static int CacheTimeFilter = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeProfiles = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeSettings = ApplicationSettings.CacheTimeConfigData;
        
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CcmCache(IAppCache cache)
        {
            _cache = cache;
        }

        public IList<RegisteredSipDto> GetRegisteredSips()
        {
            throw new NotImplementedException();
        }

        public void ClearRegisteredSips()
        {
            throw new NotImplementedException();
        }

        public IList<Call> GetCalls()
        {
            throw new NotImplementedException();
        }

        public void ClearCalls()
        {
            throw new NotImplementedException();
        }

        public IList<Setting> GetSettings()
        {
            throw new NotImplementedException();
        }

        public void ClearSettings()
        {
            throw new NotImplementedException();
        }
    }
}