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
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using LazyCache;

namespace CCM.Core.Cache
{
    public class CachedCallHistoryRepository : ICachedCallHistoryRepository
    {
        private readonly ICallHistoryRepository _internalRepository;
        private readonly IAppCache _lazyCache;
        private readonly ISettingsManager _settingsManager;

        public CachedCallHistoryRepository(
            IAppCache cache,
            ICallHistoryRepository internalRepository,
            ISettingsManager settingsManager)
        {
            _lazyCache = cache;
            _internalRepository = internalRepository;
            _settingsManager = settingsManager;
        }

        public bool Save(CallHistory callHistory)
        {
            //var success = _internalRepository.Save(callHistory);
            //_lazyCache.ClearCallHistory();
            //return success;
            return true;
        }

        public CallHistory GetById(Guid id)
        {
            return _internalRepository.GetById(id);
        }

        public CallHistory GetCallHistoryByCallId(Guid callId)
        {
            return _internalRepository.GetCallHistoryByCallId(callId);
        }

        public IList<OldCall> GetOldCalls(int callCount, bool anonymize)
        {
            // TODO: check if anonomize should be in cache saving...
            return _lazyCache.GetOrAddCallHistory(() => _internalRepository.GetOldCalls(callCount, anonymize), _settingsManager.CacheTimeLiveData);
        }

        public IList<OldCall> GetOldCallsFiltered(string region, string codecType, string sipAddress, string searchString, bool anonymize, bool onlyPhoneCalls, int callCount, bool limitByMonth)
        {
            return _internalRepository.GetOldCallsFiltered(region, codecType, sipAddress, searchString, anonymize, onlyPhoneCalls, callCount, limitByMonth);
        }

        #region Statistics
        private IReadOnlyList<CallHistory> GetOneYearCallHistory()
        {
            return _lazyCache.GetOrAddOneYearCallHistory(() => _internalRepository.GetOneYearCallHistory(), _settingsManager.CacheTimeLiveData);
        }

        public IList<CallHistory> GetCallHistoriesByDate(DateTime startDate, DateTime endDate)
        {
            if (startDate > DateTime.Now.AddYears(-1))
            {
                return _internalRepository.GetCallHistoriesByDate(GetOneYearCallHistory(), startDate, endDate);
            }
            return _internalRepository.GetCallHistoriesByDate(startDate, endDate);
        }

        public IList<CallHistory> GetCallHistoriesForRegion(DateTime startDate, DateTime endDate, Guid regionId)
        {
            if (startDate > DateTime.Now.AddYears(-1))
            {
                return _internalRepository.GetCallHistoriesForRegion(GetOneYearCallHistory(), startDate, endDate, regionId);
            }
            return _internalRepository.GetCallHistoriesForRegion(startDate, endDate, regionId);
        }

        public IList<CallHistory> GetCallHistoriesForRegisteredSip(DateTime startDate, DateTime endDate, string sipId)
        {
            if (startDate > DateTime.Now.AddYears(-1))
            {
                return _internalRepository.GetCallHistoriesForRegisteredSip(GetOneYearCallHistory(), startDate, endDate, sipId);
            }
            return _internalRepository.GetCallHistoriesForRegisteredSip(startDate, endDate, sipId);
        }

        public IList<CallHistory> GetCallHistoriesForCodecType(DateTime startDate, DateTime endDate, Guid codecTypeId)
        {
            if (startDate > DateTime.Now.AddYears(-1))
            {
                return _internalRepository.GetCallHistoriesForCodecType(GetOneYearCallHistory(), startDate, endDate, codecTypeId);
            }
            return _internalRepository.GetCallHistoriesForCodecType(startDate, endDate, codecTypeId);
        }

        public IList<CallHistory> GetCallHistoriesForLocation(DateTime startDate, DateTime endDate, Guid locationId)
        {
            if (startDate > DateTime.Now.AddYears(-1))
            {
                return _internalRepository.GetCallHistoriesForLocation(GetOneYearCallHistory(), startDate, endDate, locationId);
            }
            return _internalRepository.GetCallHistoriesForLocation(startDate, endDate, locationId);
        }
        #endregion Statistics
    }
}
