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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using CCM.Core.Entities;
using CCM.Core.Entities.Registration;
using CCM.Core.Enums;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.SipEvent;
using CCM.Data.Entities;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    /// <summary>
    /// Handles Registered User-Agents database / cache operations
    /// </summary>
    public class RegisteredSipRepository : BaseRepository, IRegisteredSipRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IMetaRepository _metaRepository;
        private readonly IUserAgentRepository _userAgentRepository;
        private readonly ISipAccountManager _sipAccountManager;
        public ILocationManager LocationManager { get; set; } // TODO: Set like this?
        public ISettingsManager SettingsManager { get; set; }

        public RegisteredSipRepository(
            ISettingsManager settingsManager,
            ILocationManager locationManager,
            IMetaRepository metaRepository,
            IUserAgentRepository userAgentRepository,
            ISipAccountManager sipAccountManager,
            IAppCache cache)
            : base(cache)
        {
            _metaRepository = metaRepository;
            _userAgentRepository = userAgentRepository;
            _sipAccountManager = sipAccountManager;
            LocationManager = locationManager;
            SettingsManager = settingsManager;
        }

        public SipEventHandlerResult UpdateRegisteredSip(UserAgentRegistration registration)
        {
            // Return value indicates if
            // 1. Codec been added
            // 2. Codec existed but registration has relevant changes
            // 3. Codec existed and registration is identical = NothingChanged

            try
            {
                using (var db = GetDbContext())
                {
                    var dbSip = db.RegisteredSips.SingleOrDefault(rs => rs.SIP == registration.SipUri);
                    // Is it a new registration?
                    if (dbSip == null)
                    {
                        if (registration.ExpirationTimeSeconds == 0)
                        {
                            // Unregistration of not registered user-agent. Do nothing.
                            log.Debug($"User-agent nothing changed, unregistration of not registered user-agent '{registration.SipUri}'");
                            return SipEventHandlerResult.NothingChanged;
                        }

                        // New registration of user-agent
                        dbSip = new RegisteredSipEntity { Id = Guid.NewGuid() };
                        db.RegisteredSips.Add(dbSip);
                    }

                    // Match and map
                    var userAgentId = GetUserAgentId(registration.UserAgentHeader);
                    dbSip.UserAgentId = userAgentId;
                    dbSip.UserAgentHeader = registration.UserAgentHeader;

                    var locationId = LocationManager.GetLocationIdByIp(registration.IpAddress);
                    dbSip.Location_LocationId = locationId != Guid.Empty ? locationId : (Guid?) null;

                    var registeredSipUsername = registration.Username.ToLower().Trim();
                    dbSip.Username = registeredSipUsername;

                    // TODO: Cache test
                    //var sipAccount = db.SipAccounts.FirstOrDefault(u => u.UserName.ToLower() == registeredSipUsername);
                    var sipAccount = _sipAccountManager.GetSipAccountByUserName(registeredSipUsername);
                    dbSip.User_UserId = sipAccount?.Id;
                    
                    dbSip.SIP = registration.SipUri;
                    dbSip.DisplayName = registration.DisplayName;
                    dbSip.IP = registration.IpAddress;
                    dbSip.Port = registration.Port;
                    dbSip.ServerTimeStamp = registration.ServerTimeStamp;
                    dbSip.Updated = DateTime.UtcNow;
                    dbSip.Expires = registration.ExpirationTimeSeconds;
                    dbSip.Registrar = registration.Registrar;

                    var changeStatus = GetChangeStatus(db, dbSip);
                    db.SaveChanges();

                    return new SipEventHandlerResult
                    {
                        ChangeStatus = changeStatus,
                        ChangedObjectId = dbSip.Id,
                        SipAddress = dbSip.SIP
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error while updating registered sip {0}", registration.SipUri);
                return SipEventHandlerResult.NothingChanged;
            }
        }

        public SipEventHandlerResult DeleteRegisteredSip(string sipAddress)
        {
            if (string.IsNullOrEmpty(sipAddress))
            {
                log.Debug("User-agent nothing changed, delete registered user-agent is empty");
                return SipEventHandlerResult.NothingChanged;
            }

            sipAddress = sipAddress.ToLower();

            try
            {
                using (var db = GetDbContext())
                {
                    // Registrations should be unique based on sipAddress,
                    // but to be safe we don't assume this.
                    var dbSips = db.RegisteredSips.Where(rs => rs.SIP == sipAddress)
                        .OrderByDescending(rs => rs.Updated);

                    if (!dbSips.Any())
                    {
                        log.Error("User-agent nothing changed, could not delete user-agent that's not registered");
                        return new SipEventHandlerResult
                        {
                            ChangeStatus = SipEventChangeStatus.NothingChanged,
                            SipAddress = sipAddress
                        };
                    }

                    var changedObjectId = dbSips.FirstOrDefault()?.Id ?? Guid.Empty;
                    db.RegisteredSips.RemoveRange(dbSips);
                    db.SaveChanges();

                    log.Debug($"User-agent removed '{sipAddress}'");
                    return new SipEventHandlerResult
                    {
                        ChangeStatus = SipEventChangeStatus.CodecRemoved,
                        ChangedObjectId = changedObjectId,
                        SipAddress = sipAddress
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error while unregistering codec with sip address {0}", sipAddress);
                return SipEventHandlerResult.NothingChanged;
            }
        }

        private SipEventChangeStatus GetChangeStatus(DbContext cxt, RegisteredSipEntity dbSip)
        {
            var entry = cxt.Entry(dbSip);

            if (entry.State == EntityState.Added)
            {
                log.Debug($"User-agent added '{dbSip.SIP}'");
                return SipEventChangeStatus.CodecAdded;
            }

            if (entry.State == EntityState.Deleted || CodecWasExpired(entry))
            {
                // There is a problem here, CodecWasExpired just tells us that it was expired when a new registration is sent. Since this is not checked anywhere else
                log.Debug($"User-agent expired or deleted '{dbSip.SIP}', CodecWasExpired={CodecWasExpired(entry)}, Entry.State={entry.State.ToString()}, Expires={dbSip.Expires}");
                return SipEventChangeStatus.CodecRemoved;
            }

            if (entry.State == EntityState.Modified && dbSip.Expires == 0)
            {
                log.Error($"User-agent removed, expired '{dbSip.SIP}', this should not happen");
                return SipEventChangeStatus.CodecRemoved;
            }

            if (entry.State == EntityState.Modified && HasRelevantChange(entry))
            {
                log.Debug($"User-agent updated, relevant changes '{dbSip.SIP}'");
                return SipEventChangeStatus.CodecUpdated;
            }

            log.Debug($"User-agent nothing changed '{dbSip.SIP}'");
            return SipEventChangeStatus.NothingChanged;
        }

        /// <summary>
        /// Returns true if registration time expired
        /// </summary>
        private bool CodecWasExpired(DbEntityEntry<RegisteredSipEntity> entry)
        {
            var maxRegistrationAge = SettingsManager.MaxRegistrationAge;
            var expireTime = DateTime.UtcNow.AddSeconds(-maxRegistrationAge);
            return entry.OriginalValues.GetValue<DateTime>(nameof(entry.Entity.Updated)) < expireTime;
        }

        /// <summary>
        /// Checks if the updating data, should be used to also trigger cache reload.
        /// Since SaveChanges triggers a full reload of the cache
        /// </summary>
        private bool HasRelevantChange(DbEntityEntry<RegisteredSipEntity> entry)
        {
            var changedProperties = GetChangedProperties(entry);
            // Remove non-relevant properties
            changedProperties.Remove(nameof(entry.Entity.Updated));
            changedProperties.Remove(nameof(entry.Entity.ServerTimeStamp));
            changedProperties.Remove(nameof(entry.Entity.Expires));

            return changedProperties.Any();
        }

        private IList<string> GetChangedProperties(DbEntityEntry entry)
        {
            if (entry.State != EntityState.Modified)
                return new List<string>();

            return entry.OriginalValues.PropertyNames
                .Where(propertyName => !Equals(entry.OriginalValues[propertyName], entry.CurrentValues[propertyName]))
                .ToList();
        }

        public RegisteredSip Single(Expression<Func<RegisteredSipEntity, bool>> expression)
        {
            // TODO: Only for tests??? 
            using (var db = GetDbContext())
            {
                RegisteredSipEntity dbRegisteredSip = db.RegisteredSips.SingleOrDefault(expression);

                return dbRegisteredSip == null
                    ? null
                    : new RegisteredSip
                    {
                        Id = dbRegisteredSip.Id,
                        DisplayName = dbRegisteredSip.DisplayName,
                        Expires = dbRegisteredSip.Expires,
                        IP = dbRegisteredSip.IP,
                        Port = dbRegisteredSip.Port,
                        SIP = dbRegisteredSip.SIP,
                        ServerTimeStamp = dbRegisteredSip.ServerTimeStamp,
                        Updated = dbRegisteredSip.Updated,
                        UserAgentHead = dbRegisteredSip.UserAgentHeader,
                        Username = dbRegisteredSip.Username,
                    };
            }
        }

        public IEnumerable<RegisteredUserAgentDiscovery> GetRegisteredUserAgentsDiscovery()
        {
            DateTime maxAge = DateTime.UtcNow.AddSeconds(-SettingsManager.MaxRegistrationAge);

            // Get available meta data properties
            List<MetaType> metaList = _metaRepository.GetAll();

            using (var db = GetDbContext())
            {
                IEnumerable<RegisteredSipEntity> dbSip = db.RegisteredSips
                    .Include(rs => rs.Location)
                    .Include(rs => rs.Location.Region)
                    .Include(rs => rs.Location.City)
                    .Include(rs => rs.User)
                    .Include(rs => rs.User.Owner)
                    .Include(rs => rs.User.CodecType)
                    .Include(rs => rs.UserAgent)
                    .Where(r => r.Updated >= maxAge)
                    .ToList();

                return dbSip.Select(x =>
                    {
                        return new RegisteredUserAgentDiscovery(
                            id: x.Id,
                            updated: x.Updated,
                            sipUri: x.SIP,
                            displayName: x.DisplayName,
                            username: x.Username,
                            ipAddress: x.IP,
                            userAgentHeader: x.UserAgentHeader,
                            userAgentId: x.UserAgentId,
                            userAgentName: x.UserAgent?.Name,
                            locationId: x.Location_LocationId,
                            locationName: x.Location?.Name,
                            locationShortName: x.Location?.ShortName,
                            regionName: x.Location?.Region?.Name,
                            cityName: x.Location?.City?.Name,
                            userOwnerName: x.User?.Owner?.Name,
                            userDisplayName: x.User?.DisplayName,
                            codecTypeName: x.User?.CodecType?.Name,
                            metaData: GetMetaData(metaList, x));
                    })
                    .ToList();
            }
        }

        public IEnumerable<RegisteredUserAgent> GetRegisteredUserAgents()
        {
            DateTime maxAge = DateTime.UtcNow.AddSeconds(-SettingsManager.MaxRegistrationAge);

            using (var db = GetDbContext())
            {
                var result = db.RegisteredSips
                    .Where(x => x.Updated >= maxAge)
                    .Select(x =>
                        new
                        {
                            SIP = x.SIP,
                            Id = x.Id,
                            DisplayName = x.DisplayName,
                            LocationName = x.Location.Name,
                            LocationShortName = x.Location.ShortName,
                            Image = x.UserAgent.Image,
                            CodecTypeName = x.User.CodecType.Name,
                            CodecTypeColor = x.User.CodecType.Color,
                            Username = x.Username,
                            UserDisplayName = x.User.DisplayName,
                            UserComment = x.User.Comment,
                            RegionName = x.Location.Region.Name
                        })
                    .ToList();

                return result.Select(x =>
                    {
                        return new RegisteredUserAgent(
                            sipUri: x.SIP,
                            id: x.Id,
                            displayName: x.DisplayName,
                            location: x.LocationName,
                            locationShortName: x.LocationShortName,
                            image: x.Image,
                            codecTypeName: x.CodecTypeName,
                            codecTypeColor: x.CodecTypeColor,
                            username: x.Username,
                            userDisplayName: x.UserDisplayName,
                            userComment: x.UserComment,
                            regionName: x.RegionName);
                    })
                    .ToList();
            }
        }

        //private void AddCallInfoToCachedRegisteredSip(CcmDbContext db, IEnumerable<RegisteredSipDto> registeredSips)
        //{
        //    var sipDomain = SettingsManager.SipDomain;

        //    var dbCalls = db.Calls
        //        .Include(c => c.FromSip)
        //        .Include(c => c.FromSip.Location)
        //        .Include(c => c.FromSip.UserAgent)
        //        .Include(c => c.ToSip)
        //        .Include(c => c.ToSip.Location)
        //        .Include(c => c.ToSip.UserAgent)
        //        .Where(call => !call.Closed)
        //        .ToList();

        //    foreach (var item in registeredSips)
        //    {
        //        var sip = item;
        //        var call = dbCalls.FirstOrDefault(c => c.ToUsername == sip.Sip || c.FromUsername == sip.Sip);
        //        if (call == null)
        //        {
        //            sip.InCall = false;
        //            continue;
        //        }

        //        sip.InCall = true;
        //        sip.IsPhoneCall = call.IsPhoneCall;
        //        sip.CallStartedAt = call.Started;

        //        if (call.FromUsername == sip.Sip)
        //        {
        //            sip.InCallWithId = GuidHelper.GuidString(call.ToId);
        //            sip.InCallWithName =
        //                CallDisplayNameHelper.GetDisplayName(call.ToSip, call.ToDisplayName, call.ToUsername,
        //                    sipDomain);
        //            sip.InCallWithSip = call.ToUsername;
        //            sip.InCallWithLocation = call.ToSip != null && call.ToSip.Location != null
        //                ? call.ToSip.Location.Name
        //                : string.Empty;
        //            sip.InCallWithHasGpo = call.ToSip != null && call.ToSip.UserAgent != null &&
        //                                   !string.IsNullOrEmpty(call.ToSip.UserAgent.GpoNames);
        //            sip.IsCallingPart = true;
        //        }
        //        else
        //        {
        //            sip.InCallWithId = GuidHelper.GuidString(call.FromId);
        //            sip.InCallWithName = CallDisplayNameHelper.GetDisplayName(call.FromSip, call.FromDisplayName,
        //                call.FromUsername, sipDomain);
        //            sip.InCallWithSip = call.FromUsername;
        //            sip.InCallWithLocation = call.FromSip != null && call.FromSip.Location != null
        //                ? call.FromSip.Location.Name
        //                : string.Empty;
        //            sip.InCallWithHasGpo = call.FromSip != null && call.FromSip.UserAgent != null &&
        //                                   !string.IsNullOrEmpty(call.FromSip.UserAgent.GpoNames);
        //            sip.IsCallingPart = false;
        //        }
        //    }
        //}

        private List<KeyValuePair<string, string>> GetMetaData(List<MetaType> metaList, RegisteredSipEntity sip)
        {
            metaList = metaList ?? new List<MetaType>();

            var userAgentMetaDataList = metaList
                .Select(meta =>
                    new KeyValuePair<string, string>(meta.Name,
                        MetadataHelper.GetPropertyValue(sip, meta.FullPropertyName)))
                .Where(m => !string.IsNullOrEmpty(m.Value))
                .ToList();

            return userAgentMetaDataList;
        }

        private Guid? GetUserAgentId(string userAgent)
        {
            userAgent = (userAgent ?? string.Empty).Trim();

            var allUserAgents = _userAgentRepository.GetAll()
                .Where(ua => !string.IsNullOrEmpty(ua.Identifier))
                .Select(ua => new UserAgentInfo(
                    userAgentId: ua.Id,
                    identifier: ua.Identifier,
                    matchType: ua.MatchType
                )).ToList();

            var dbUserAgent = allUserAgents.FirstOrDefault(u =>
                u.MatchType == MatchType.Begins_With && userAgent.StartsWith(u.Identifier));

            if (dbUserAgent != null)
            {
                return dbUserAgent.UserAgentId;
            }

            dbUserAgent = allUserAgents.FirstOrDefault(u =>
                u.MatchType == MatchType.Ends_With && userAgent.EndsWith(u.Identifier));

            if (dbUserAgent != null)
            {
                return dbUserAgent.UserAgentId;
            }

            dbUserAgent = allUserAgents.FirstOrDefault(u =>
                u.MatchType == MatchType.Contains && userAgent.Contains(u.Identifier));

            return dbUserAgent?.UserAgentId;
        }

        public IEnumerable<RegisteredUserAgentCodecInformation> GetRegisteredUserAgentsCodecInformation()
        {
            // TODO: This could maybe be built up in a manager for external api codec information
            DateTime maxAge = DateTime.UtcNow.AddSeconds(-SettingsManager.MaxRegistrationAge);

            try
            {
                using (var db = GetDbContext())
                {
                    var result = db.RegisteredSips
                        .Where(x => !string.IsNullOrEmpty(x.UserAgent.Api)) // TODO: is this really.. necessary?
                        .Where(x => x.Updated >= maxAge)
                        .Select(x =>
                            new
                            {
                                Sip = x.SIP,
                                Ip = x.IP,
                                UserAgentApi = x.UserAgent.Api,
                                UserAgentGpoNames = x.UserAgent.GpoNames,
                                UserAgentInputs = x.UserAgent.Inputs,
                                UserAgentNrOfGpos = x.UserAgent.NrOfGpos
                            })
                        .ToList();

                    // Only include latest registrations per sip address
                    var groupBy = result.GroupBy(x => x.Sip).ToList();
                    var groupedList = groupBy.Select(g => g.First())
                        .OrderBy(rs => rs.Sip)
                        .ToList();

                    return groupedList.Select(x =>
                    {
                        return new RegisteredUserAgentCodecInformation(
                            sipAddress: x.Sip,
                            ip: x.Ip,
                            api: x.UserAgentApi,
                            gpoNames: x.UserAgentGpoNames,
                            nrOfInputs: x.UserAgentInputs,
                            nrOfGpos: x.UserAgentNrOfGpos);
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Problem when returning GetRegisteredUserAgentsCodecInformation");
                return null;
            }
        }

    }
}
