/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
        // Cache time in seconds
        public static int CacheTimeLiveData = ApplicationSettings.CacheTimeLiveData;

        public static int CacheTimeFilter = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeProfiles = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeProfileGroups = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeLocations = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeUserAgents = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeUserAgentsAndProfiles = ApplicationSettings.CacheTimeConfigData;
        public static int CacheTimeSipAccounts = ApplicationSettings.CacheTimeConfigData;

        // Cache keys
        private const string RegisteredUserAgentsDiscoveryKey = "RegisteredUserAgentsDiscovery";
        private const string RegisteredUserAgentsCodecInformationKey = "RegisteredUserAgentsCodecInformation";
        private const string RegisteredUserAgentsKey = "RegisteredUserAgents";
        private const string OngoingCallsKey = "OngoingCalls";
        private const string CallHistoryKey = "CallHistory";
        private const string SettingsKey = "Settings";
        private const string LocationNetworksKey = "LocationNetworks";
        private const string LocationsAndProfilesKey = "LocationsAndProfiles";
        private const string AvailableFiltersKey = "AvailableFilters";
        private const string AllProfileNamesAndSdpKey = "AllProfileNamesAndSdp";
        private const string ProfileGroupsKey = "ProfileGroups";
        private const string UserAgentsKey = "UserAgents";
        private const string UserAgentsAndProfilesKey = "UserAgentsAndProfiles";
        private const string SipAccountsKey = "SipAccounts";

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        // TODO: Make some decisions on how we deal with replication changes that doesn't trigger a reload of cache if this instance was not the one saving changes.

        #region SipAccounts
        public static List<SipAccount> GetOrAddSipAccounts(this IAppCache cache, Func<List<SipAccount>> sipAccountsLoader)
        {
            return cache.GetOrAdd(SipAccountsKey, sipAccountsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeSipAccounts));
        }

        public static void ClearSipAccounts(this IAppCache cache)
        {
            log.Debug("Removing sip accounts from cache");
            cache.Remove(SipAccountsKey);
        }
        #endregion

        #region RegisteredUserAgents
        public static IEnumerable<RegisteredUserAgent> GetOrAddRegisteredUserAgents(this IAppCache cache, Func<IEnumerable<RegisteredUserAgent>> registeredUserAgentsLoader)
        {
            return cache.GetOrAdd(RegisteredUserAgentsKey, registeredUserAgentsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLiveData));
        }

        public static IEnumerable<RegisteredUserAgentDiscovery> GetOrAddRegisteredUserAgentsDiscovery(this IAppCache cache, Func<IEnumerable<RegisteredUserAgentDiscovery>> registeredUserAgentsDiscoveryLoader)
        {
            return cache.GetOrAdd(RegisteredUserAgentsDiscoveryKey, registeredUserAgentsDiscoveryLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLiveData));
        }

        public static IEnumerable<RegisteredUserAgentCodecInformation> GetOrAddRegisteredUserAgentsCodecInformation(this IAppCache cache, Func<IEnumerable<RegisteredUserAgentCodecInformation>> registeredUserAgentsCodecInformationLoader)
        {
            return cache.GetOrAdd(RegisteredUserAgentsCodecInformationKey, registeredUserAgentsCodecInformationLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLiveData));
        }

        public static void ClearRegisteredUserAgents(this IAppCache cache)
        {
            log.Debug("Removing registered user agents from cache");
            cache.Remove(RegisteredUserAgentsKey);

            log.Debug("Removing registered user agents discovery from cache");
            cache.Remove(RegisteredUserAgentsDiscoveryKey);

            log.Debug("Removing registered user agents codec information from cache");
            cache.Remove(RegisteredUserAgentsCodecInformationKey);
        }
        #endregion

        #region OngoingCalls
        public static IReadOnlyCollection<OnGoingCall> GetOrAddOngoingCalls(this IAppCache cache, Func<IReadOnlyCollection<OnGoingCall>> ongoingCallsLoader)
        {
            return cache.GetOrAdd(OngoingCallsKey, ongoingCallsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLiveData));
        }

        public static void ClearOngoingCalls(this IAppCache cache)
        {
            log.Debug("Removing ongoing calls from cache");
            cache.Remove(OngoingCallsKey);
        }
        #endregion

        #region CallHistory
        public static IList<OldCall> GetOrAddCallHistory(this IAppCache cache, Func<IList<OldCall>> callHistoryLoader)
        {
            return cache.GetOrAdd(CallHistoryKey, callHistoryLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLiveData));
        }

        public static void ClearCallHistory(this IAppCache cache)
        {
            log.Debug("Removing call history from cache");
            cache.Remove(CallHistoryKey);
        }
        #endregion

        #region Settings
        public static List<Setting> GetOrAddSettings(this IAppCache cache, Func<List<Setting>> settingsLoader)
        {
            return cache.GetOrAdd(SettingsKey, settingsLoader);
        }

        public static void ClearSettings(this IAppCache cache)
        {
            log.Debug("Removing settings from cache");
            cache.Remove(SettingsKey);
        }
        #endregion

        #region LocationNetworks
        public static List<LocationNetwork> GetOrAddLocationNetworks(this IAppCache cache, Func<List<LocationNetwork>> loader)
        {
            var list = cache.GetOrAdd(LocationNetworksKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLocations));
            return list;
        }

        public static Dictionary<Guid, LocationAndProfiles> GetOrAddLocationsAndProfiles(this IAppCache cache, Func<Dictionary<Guid, LocationAndProfiles>> loader)
        {
            return cache.GetOrAdd(LocationsAndProfilesKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeLocations));
        }

        public static void ClearLocationNetworks(this IAppCache cache)
        {
            log.Debug("Removing location networks from cache");
            cache.Remove(LocationNetworksKey);

            log.Debug("Removing locations and profiles from cache");
            cache.Remove(LocationsAndProfilesKey);
        }
        #endregion

        #region AvailableFilters
        public static IList<AvailableFilter> GetAvailableFilters(this IAppCache cache, Func<IList<AvailableFilter>> loader)
        {
            // TODO: Should not be called like this?!
            var list = cache.GetOrAdd(AvailableFiltersKey, loader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeFilter));
            return list;
        }

        public static void ClearAvailableFilters(this IAppCache cache)
        {
            log.Debug("Removing available filters from cache");
            cache.Remove(AvailableFiltersKey);
        }
        #endregion

        #region Profiles
        public static IList<ProfileNameAndSdp> GetOrAddAllProfileNamesAndSdp(this IAppCache cache, Func<IList<ProfileNameAndSdp>> allProfileNamesAndSdpLoader)
        {
            return cache.GetOrAdd(AllProfileNamesAndSdpKey, allProfileNamesAndSdpLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeProfiles));
        }

        public static void ClearProfiles(this IAppCache cache)
        {
            log.Debug("Removing all profile names and sdp from cache");
            cache.Remove(AllProfileNamesAndSdpKey);
        }
        #endregion

        #region ProfileGroups
        public static List<ProfileGroup> GetOrAddProfileGroups(this IAppCache cache, Func<List<ProfileGroup>> profileGroupsLoader)
        {
            return cache.GetOrAdd(ProfileGroupsKey, profileGroupsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeProfileGroups));
        }

        public static void ClearProfileGroups(this IAppCache cache)
        {
            log.Debug("Removing profile groups from cache");
            cache.Remove(ProfileGroupsKey);
        }
        #endregion

        #region UserAgent
        public static List<UserAgent> GetOrAddUserAgents(this IAppCache cache, Func<List<UserAgent>> userAgentsLoader)
        {
            return cache.GetOrAdd(UserAgentsKey, userAgentsLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeUserAgents));
        }

        public static Dictionary<Guid, UserAgentAndProfiles> GetOrAddUserAgentsAndProfiles(this IAppCache cache, Func<Dictionary<Guid, UserAgentAndProfiles>> userAgentsAndProfilesLoader)
        {
            return cache.GetOrAdd(UserAgentsAndProfilesKey, userAgentsAndProfilesLoader, DateTimeOffset.UtcNow.AddSeconds(CacheTimeUserAgentsAndProfiles));
        }

        public static void ClearUserAgents(this IAppCache cache)
        {
            // INFO: Cleared if User Agent or Profiles are changed in database
            log.Debug("Removing user agents from cache");
            cache.Remove(UserAgentsKey);

            log.Debug("Removing user agents and profiles from cache");
            cache.Remove(UserAgentsAndProfilesKey);
        }
        #endregion

        #region Full reload
        public static void FullReload(this IAppCache cache)
        {
            log.Info("Full cache reload");
            // TODO: Maybe divide this function in to more granularity
            cache.ClearSipAccounts();
            cache.ClearRegisteredUserAgents(); // Make sure that this one is cleared if user agents change
            cache.ClearOngoingCalls();
            cache.ClearCallHistory();
            cache.ClearLocationNetworks();
            cache.ClearSettings();
            cache.ClearAvailableFilters();
            cache.ClearProfiles();
            cache.ClearProfileGroups();
            cache.ClearUserAgents();
        }
        #endregion

    }
}
