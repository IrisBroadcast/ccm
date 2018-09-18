using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Helpers;
using LazyCache;
using NLog;

namespace CCM.Core.Cache
{
    public static class LazyCacheExtensions
    {
        #region RegisteredSips

        // Cache time in seconds
        public static int CacheTimeCachedRegisteredSips = ApplicationSettings.CacheTimeLiveData;
        public static int CacheTimeOngoingCalls = ApplicationSettings.CacheTimeLiveData;
        public static int CacheTimeOldCalls = ApplicationSettings.CacheTimeLiveData;

        public static int CacheTimeFilter = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeProfiles = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeSettings = ApplicationSettings.CacheTimeConfigData;

        private const string RegisteredSipsKey = "RegisteredSips";

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static List<RegisteredSipDto> GetOrAddRegisteredSips(this IAppCache cache, Func<List<RegisteredSipDto>> registeredSipsLoader)
        {
            using (new TimeMeasurer("GetCachedRegisteredSips from cache."))
            {
                var list = cache.GetOrAdd(RegisteredSipsKey, registeredSipsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeCachedRegisteredSips));
                return list;
            }
        }
        
        public static void ClearRegisteredSips(this IAppCache cache)
        {
            log.Debug("Removing registered sips from cache");
            cache.Remove(RegisteredSipsKey);
        }

        #endregion
      
        #region Settings

        private const string SettingsKey = "Settings";

        public static List<Setting> GetSettings(this IAppCache cache, Func<List<Setting>> loader)
        {
            var list = cache.GetOrAdd(SettingsKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeSettings));
            return list;
        }


        public static void ResetSettings(this IAppCache cache)
        {
            log.Debug("Removing settings from cache");
            cache.Remove(SettingsKey);
        }

        #endregion

        #region LocationNetworks

        private const string LocationNetworksKey = "LocationNetworks";

        public static List<LocationNetwork> GetOrAddLocationNetworks(this IAppCache cache, Func<List<LocationNetwork>> loader)
        {
            var list = cache.GetOrAdd(LocationNetworksKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeSettings));
            return list;
        }

        public static void ResetLocationNetworks(this IAppCache cache)
        {
            log.Debug("Removing location networks from cache");
            cache.Remove(LocationNetworksKey);
        }

        #endregion

        #region AvailableFilters

        private const string AvailableFiltersKey = "AvailableFilters";

        public static IList<AvailableFilter> GetAvailableFilters(this IAppCache cache, Func<IList<AvailableFilter>> loader)
        {
            var list = cache.GetOrAdd(AvailableFiltersKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeFilter));
            return list;
        }

        public static void ResetAvailableFilters(this IAppCache cache)
        {
            log.Debug("Removing available filters from cache");
            cache.Remove(AvailableFiltersKey);
        }

        #endregion

        #region Profiles

        private const string ProfilesKey = "Profiles";

        public static IList<ProfileNameAndSdp> GetProfiles(this IAppCache cache, Func<IList<ProfileNameAndSdp>> loader)
        {
            return cache.GetOrAdd(ProfilesKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeProfiles));
        }

        public static void ResetProfiles(this IAppCache cache)
        {
            log.Debug("Removing profiles from cache");
            cache.Remove(ProfilesKey);
        }

        #endregion

        #region Full reload

        public static void FullReload(this IAppCache cache)
        {
            cache.ClearRegisteredSips();
            cache.ResetLocationNetworks();
            cache.ResetSettings();
        }

        #endregion

    }
}