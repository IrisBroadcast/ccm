using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using LazyCache;

namespace CCM.Core.Cache
{
    public class CachedCallRepository : ICallRepository
    {
        private readonly ICallRepository _internalRepository;
        private readonly IAppCache _lazyCache;

        public CachedCallRepository(IAppCache cache, ICallRepository internalRepository)
        {
            _lazyCache = cache;
            _internalRepository = internalRepository;
        }

        public IList<OnGoingCall> GetOngoingCalls(bool anonomize) { return _internalRepository.GetOngoingCalls(anonomize); }
        public bool CallExists(string callId, string hashId, string hashEnt) { return _internalRepository.CallExists(callId, hashId, hashEnt); }
        public CallInfo GetCallInfo(string callId, string hashId, string hashEnt) { return _internalRepository.GetCallInfo(callId, hashId, hashEnt); }
        public CallInfo GetCallInfoById(Guid callId) { return _internalRepository.GetCallInfoById(callId); }
        public Call GetCallBySipAddress(string sipAddress) { return _internalRepository.GetCallBySipAddress(sipAddress); }

        public void UpdateCall(Call call)
        {
            _internalRepository.UpdateCall(call);
            // Some registered codecs may have changed state. Reload cache.
            _lazyCache.ClearRegisteredSips();
        }

        public void CloseCall(Guid callId)
        {
            _internalRepository.CloseCall(callId);
            // Some registered codecs may have changed state. Reload cache.
            _lazyCache.ClearRegisteredSips();
        }

    }
}