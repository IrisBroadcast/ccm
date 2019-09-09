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
using CCM.Core.Interfaces.Repositories;
using LazyCache;

namespace CCM.Core.Cache
{
    public class CachedCallHistoryRepository : ICallHistoryRepository
    {
        private readonly ICallHistoryRepository _internalRepository;
        private readonly IAppCache _lazyCache;

        public CachedCallHistoryRepository(IAppCache cache, ICallHistoryRepository internalRepository)
        {
            _lazyCache = cache;
            _internalRepository = internalRepository;
        }

        public bool Save(CallHistory callHistory)
        {
            var success = _internalRepository.Save(callHistory);
            _lazyCache.ClearCallHistory();
            return success;
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
            return _lazyCache.GetOrAddCallHistory(() => _internalRepository.GetOldCalls(callCount, anonymize));
        }

        public IList<OldCall> GetOldCallsFiltered(string region, string codecType, string sipAddress, string searchString, bool anonymize, bool onlyPhoneCalls, int callCount)
        {
            return _internalRepository.GetOldCallsFiltered(region, codecType, sipAddress, searchString, anonymize,
                onlyPhoneCalls, callCount);
        }

        public IList<CallHistory> GetCallHistoriesByDate(DateTime startTime, DateTime endTime)
        {
            return _internalRepository.GetCallHistoriesByDate(startTime, endTime);
        }

        public IList<CallHistory> GetCallHistoriesForRegion(DateTime startDate, DateTime endDate, Guid regionId)
        {
            return _internalRepository.GetCallHistoriesForRegion(startDate, endDate, regionId);
        }

        public IList<CallHistory> GetCallHistoriesForRegisteredSip(DateTime startDate, DateTime endDate, string sipId)
        {
            return _internalRepository.GetCallHistoriesForRegisteredSip(startDate, endDate, sipId);
        }

        public IList<CallHistory> GetCallHistoriesForCodecType(DateTime startDate, DateTime endDate, Guid codecTypeId)
        {
            return _internalRepository.GetCallHistoriesForCodecType(startDate, endDate, codecTypeId);
        }

        public IList<CallHistory> GetCallHistoriesForLocation(DateTime startDate, DateTime endDate, Guid locationId)
        {
            return _internalRepository.GetCallHistoriesForLocation(startDate, endDate, locationId);
        }

    }
}
