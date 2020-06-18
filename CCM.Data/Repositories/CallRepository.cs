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
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Enums;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using CCM.Data.Helpers;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    public class CallRepository : BaseRepository, ICallRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ICallHistoryRepository _callHistoryRepository;
        private readonly ISettingsManager _settingsManager;

        public CallRepository(ICallHistoryRepository callHistoryRepository, ISettingsManager settingsManager, IAppCache cache) : base(cache)
        {
            _callHistoryRepository = callHistoryRepository;
            _settingsManager = settingsManager;
        }

        public bool CallExists(string callId, string hashId, string hashEnt)
        {
            using (var db = GetDbContext())
            {
                return db.Calls.Any(c => c.SipCallID == callId && c.DlgHashId == hashId && c.DlgHashEnt == hashEnt);
            }
        }

        public void CloseCall(Guid callId)
        {
            using (var db = GetDbContext())
            {
                var dbCall = db.Calls.SingleOrDefault(c => c.Id == callId);

                if (dbCall == null)
                {
                    log.Warn("Trying to close call but call with id {0} doesn't exist", callId);
                    return;
                }

                dbCall.Updated = DateTime.UtcNow;
                dbCall.Closed = true;
                // TODO: Is it necessary to save this closing of the call, to then remove it later?
                db.SaveChanges();

                // Save call history
                var callHistory = MapToCallHistory(dbCall, _settingsManager.SipDomain);
                var success = _callHistoryRepository.Save(callHistory);

                if (success)
                {
                    // Remove the original call
                    db.Calls.Remove(dbCall);
                    db.SaveChanges();
                }
                else
                {
                    log.Error($"Unable to save call history with the call fromSip: {dbCall.FromSip}, toSip: {dbCall.ToSip}, hash id: {dbCall.DlgHashId}, hash ent: {dbCall.DlgHashEnt}");
                }
            }
        }

        public CallInfo GetCallInfo(string callId, string hashId, string hashEnt) // CallId, HashId and HashEntry is a unique key for calls in Kamailio
        {
            using (var db = GetDbContext())
            {
                var dbCall = db.Calls.SingleOrDefault(c => c.SipCallID == callId && c.DlgHashId == hashId && c.DlgHashEnt == hashEnt);
                return MapToCallInfo(dbCall);
            }
        }

        public CallInfo GetCallInfoById(Guid callId)
        {
            using (var db = GetDbContext())
            {
                var dbCall = db.Calls.SingleOrDefault(c => c.Id == callId);
                return MapToCallInfo(dbCall);
            }
        }

        private CallInfo MapToCallInfo(CallEntity dbCall)
        {
            return dbCall == null ? null : new CallInfo
            {
                Id = dbCall.Id,
                Started = dbCall.Started,
                FromSipAddress = dbCall.FromUsername,
                ToSipAddress = dbCall.ToUsername,
                FromId = dbCall.FromId ?? Guid.Empty,
                ToId = dbCall.ToId ?? Guid.Empty,
                Closed = dbCall.Closed
            };
        }

        public Call GetCallBySipAddress(string sipAddress)
        {
            if (string.IsNullOrEmpty(sipAddress))
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                var dbCall = db.Calls
                    .OrderByDescending(c => c.Updated) // Last call in case several happens to exist in database
                    .FirstOrDefault(c => !c.Closed && (c.FromUsername == sipAddress || c.ToUsername == sipAddress));

                return MapCall(dbCall);
            }
        }

        public void UpdateCall(Call call)
        {
            try
            {
                using (var db = GetDbContext())
                {
                    var dbCall = call.Id != Guid.Empty ? db.Calls.SingleOrDefault(c => c.Id == call.Id) : null;

                    if (dbCall == null)
                    {
                        var callId = Guid.NewGuid();
                        call.Id = callId;

                        dbCall = new CallEntity
                        {
                            Id = callId,
                            SipCallID = call.CallId,
                            DlgHashId = call.DlgHashId,
                            DlgHashEnt = call.DlgHashEnt,
                            ToTag = call.ToTag,
                            FromTag = call.FromTag,
                            Started = call.Started,
                            FromId = call.FromId,
                            FromUsername = call.FromSip,
                            FromDisplayName = call.FromDisplayName,
                            ToId = call.ToId,
                            ToUsername = call.ToSip,
                            ToDisplayName = call.ToDisplayName,
                            IsPhoneCall = call.IsPhoneCall
                        };

                        db.Calls.Add(dbCall);
                    }

                    // Common properties. Updated also for existing call
                    var updated = DateTime.UtcNow;
                    call.Updated = updated;
                    dbCall.Updated = updated;
                    dbCall.State = call.State;
                    dbCall.Closed = call.Closed;

                    var success = db.SaveChanges() > 0;

                    if (success && call.Closed)
                    {
                        // Call ended. Save call history and delete call from db
                        var callHistory = MapToCallHistory(dbCall, _settingsManager.SipDomain);
                        var callHistorySaved = _callHistoryRepository.Save(callHistory);

                        if (callHistorySaved)
                        {
                            // Remove the original call
                            db.Calls.Remove(dbCall);
                            db.SaveChanges();
                        }
                        else
                        {
                            log.Error($"Unable to save call history with call id: {call.CallId}, hash id: {call.DlgHashId}, hash ent: {call.DlgHashEnt}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error saving/updating call with call id: {call.CallId}, hash id: {call.DlgHashId}, hash ent: {call.DlgHashEnt}");
            }
        }

        public IReadOnlyCollection<OnGoingCall> GetOngoingCalls(bool anonymize)
        {
            using (var db = GetDbContext())
            {
                var dbCalls = db.Calls
                    .Include(c => c.FromSip)
                    .Include(c => c.FromSip.User)
                    .Include(c => c.FromSip.User.CodecType)
                    .Include(c => c.FromSip.Location)
                    .Include(c => c.FromSip.Location.Region)
                    .Include(c => c.ToSip)
                    .Include(c => c.ToSip.User)
                    .Include(c => c.ToSip.User.CodecType)
                    .Include(c => c.ToSip.Location)
                    .Include(c => c.ToSip.Location.Region)
                    .Where(call => !call.Closed).ToList();
                return dbCalls.Select(dbCall => MapToOngoingCall(dbCall, _settingsManager.SipDomain, anonymize)).ToList().AsReadOnly();
            }
        }

        private OnGoingCall MapToOngoingCall(CallEntity dbCall, string sipDomain, bool anonymize)
        {
            // TODO: Fix this mapping, and maybe redo the query?
            var fromDisplayName = CallDisplayNameHelper.GetDisplayName(dbCall.FromSip, dbCall.FromDisplayName, dbCall.FromUsername, sipDomain);
            var toDisplayName = CallDisplayNameHelper.GetDisplayName(dbCall.ToSip, dbCall.ToDisplayName, dbCall.ToUsername, sipDomain);

            var onGoingCall = new OnGoingCall
            {
                CallId = GuidHelper.GuidString(dbCall.Id),
                Started = dbCall.Started,
                FromId = GuidHelper.GuidString(dbCall.FromId),
                FromSip = anonymize ? DisplayNameHelper.AnonymizePhonenumber(dbCall.FromUsername) : dbCall.FromUsername,
                FromDisplayName = anonymize ? DisplayNameHelper.AnonymizeDisplayName(fromDisplayName) : fromDisplayName,
                FromCodecTypeColor = dbCall.FromSip?.User?.CodecType?.Color ?? string.Empty,
                FromCodecTypeName = dbCall.FromSip?.User?.CodecType?.Name ?? string.Empty,
                FromComment = dbCall.FromSip?.User?.Comment ?? string.Empty,
                FromLocationName = dbCall.FromSip?.Location?.Name ?? string.Empty,
                FromLocationShortName = dbCall.FromSip?.Location?.ShortName ?? string.Empty,
                FromRegionName = dbCall.FromSip?.Location?.Region?.Name ?? string.Empty,
                ToId = GuidHelper.GuidString(dbCall.ToId),
                ToSip = anonymize ? DisplayNameHelper.AnonymizePhonenumber(dbCall.ToUsername) : dbCall.ToUsername,
                ToDisplayName = anonymize ? DisplayNameHelper.AnonymizeDisplayName(toDisplayName) : toDisplayName,
                ToCodecTypeColor = dbCall.ToSip?.User?.CodecType?.Color ?? string.Empty,
                ToCodecTypeName = dbCall.ToSip?.User?.CodecType?.Name ?? string.Empty,
                ToComment = dbCall.ToSip?.User?.Comment ?? string.Empty,
                ToLocationName = dbCall.ToSip?.Location?.Name ?? string.Empty,
                ToLocationShortName = dbCall.ToSip?.Location?.ShortName ?? string.Empty,
                ToRegionName = dbCall.ToSip?.Location?.Region?.Name ?? string.Empty
            };

            return onGoingCall;
        }

        private CallHistory MapToCallHistory(CallEntity call, string sipDomain)
        {
            // TODO: Clean up this null checks
            var callHistory = new CallHistory()
            {
                CallId = call.Id,
                DlgHashEnt = call.DlgHashEnt,
                DlgHashId = call.DlgHashId,
                Ended = call.Updated,
                FromCodecTypeColor = call.FromSip?.User?.CodecType?.Color ?? string.Empty,
                FromCodecTypeId = call.FromSip?.User?.CodecType?.Id ?? Guid.Empty,
                FromCodecTypeName = call.FromSip?.User?.CodecType?.Name ?? string.Empty,
                FromComment = call.FromSip?.User?.Comment ?? string.Empty,
                FromDisplayName = CallDisplayNameHelper.GetDisplayName(call.FromSip, call.FromDisplayName, call.FromUsername, sipDomain),
                FromId = call.FromId ?? Guid.Empty,
                FromLocationComment = call.FromSip?.Location?.Comment ?? string.Empty,
                FromLocationId = call.FromSip?.Location?.Id ?? Guid.Empty,
                FromLocationName = call.FromSip?.Location?.Name ?? string.Empty,
                FromLocationShortName = call.FromSip?.Location?.ShortName ?? string.Empty,
                FromOwnerId = call.FromSip?.User?.Owner?.Id ?? Guid.Empty,
                FromOwnerName = call.FromSip?.User?.Owner?.Name ?? string.Empty,
                FromRegionId = call.FromSip?.Location?.Region?.Id ?? Guid.Empty,
                FromRegionName = call.FromSip?.Location?.Region?.Name ?? string.Empty,
                FromSip = call.FromSip?.SIP ?? call.FromUsername,
                FromTag = call.FromTag,
                FromUserAgentHead = call.FromSip?.UserAgentHeader ?? string.Empty,
                FromUsername = call.FromSip?.Username ?? call.FromUsername,
                SipCallId = call.SipCallID,
                Started = call.Started,
                ToCodecTypeColor = call.ToSip?.User?.CodecType?.Color ?? string.Empty,
                ToCodecTypeId = call.ToSip?.User?.CodecType?.Id ?? Guid.Empty,
                ToCodecTypeName = call.ToSip?.User?.CodecType?.Name ?? string.Empty,
                ToComment = call.ToSip?.User?.Comment ?? string.Empty,
                ToDisplayName = CallDisplayNameHelper.GetDisplayName(call.ToSip, call.ToDisplayName, call.ToUsername, sipDomain),
                ToId = call.ToId ?? Guid.Empty,
                ToLocationComment = call.ToSip?.Location?.Comment ?? string.Empty,
                ToLocationId = call.ToSip?.Location?.Id ?? Guid.Empty,
                ToLocationName = call.ToSip?.Location?.Name ?? string.Empty,
                ToLocationShortName = call.ToSip?.Location?.ShortName ?? string.Empty,
                ToOwnerId = call.ToSip?.User?.Owner?.Id ?? Guid.Empty,
                ToOwnerName = call.ToSip?.User?.Owner?.Name ?? string.Empty,
                ToRegionId = call.ToSip?.Location?.Region?.Id ?? Guid.Empty,
                ToRegionName = call.ToSip?.Location?.Region?.Name ?? string.Empty,
                ToSip = call.ToSip?.SIP ?? call.ToUsername,
                ToTag = call.ToTag,
                ToUserAgentHead = call.ToSip?.UserAgentHeader ?? string.Empty,
                ToUsername = call.ToSip?.Username ?? call.ToUsername,
                IsPhoneCall = call.IsPhoneCall
            };
            return callHistory;
        }

        private Call MapCall(CallEntity dbCall)
        {
            return dbCall == null ? null : new Call
            {
                FromId = dbCall.FromId ?? Guid.Empty,
                ToId = dbCall.ToId ?? Guid.Empty,
                Started = dbCall.Started,
                State = dbCall.State ?? SipCallState.NONE,
                Updated = dbCall.Updated,
                Id = dbCall.Id,
                CallId = dbCall.SipCallID,
                Closed = dbCall.Closed,
                From = MapRegisteredSip(dbCall.FromSip),
                To = MapRegisteredSip(dbCall.ToSip),
                FromSip = dbCall.FromUsername,
                ToSip = dbCall.ToUsername,
                FromTag = dbCall.FromTag,
                ToTag = dbCall.ToTag,
                DlgHashId = dbCall.DlgHashId,
                DlgHashEnt = dbCall.DlgHashEnt,
                IsPhoneCall = dbCall.IsPhoneCall
            };
        }

        private RegisteredSip MapRegisteredSip(RegisteredSipEntity dbSip)
        {
            var sip = dbSip == null ? null : new RegisteredSip()
            {
                Id = dbSip.Id,
                SIP = dbSip.SIP,
                DisplayName = dbSip.DisplayName,
                UserAgentHead = dbSip.UserAgentHeader,
                Username = dbSip.Username,
                User = MapUser(dbSip.User),
            };

            return sip;
        }

        private SipAccount MapUser(SipAccountEntity dbAccount)
        {
            return dbAccount == null ? null : new SipAccount()
            {
                Id = dbAccount.Id,
                UserName = dbAccount.UserName,
                DisplayName = dbAccount.DisplayName,
            };
        }
    }
}
