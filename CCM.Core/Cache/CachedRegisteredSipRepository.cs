using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using LazyCache;
using NLog;
using System.Linq;
using CCM.Core.CodecControl.Entities;

namespace CCM.Core.Cache
{
    public class CachedRegisteredSipRepository : IRegisteredSipRepository
    {
        #region Members and constructor
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IRegisteredSipRepository _internalRepository;
        private readonly IAppCache _lazyCache;

        public CachedRegisteredSipRepository(IAppCache cache, IRegisteredSipRepository internalRepository)
        {
            _lazyCache = cache;
            _internalRepository = internalRepository;
        }
        #endregion

        public KamailioMessageHandlerResult UpdateRegisteredSip(RegisteredSip registeredSip)
        {
            var result = _internalRepository.UpdateRegisteredSip(registeredSip);

            // When reregistration of codec already in cache, just update timestamp
            if (result.ChangeStatus == KamailioMessageChangeStatus.NothingChanged && result.ChangedObjectId != Guid.Empty)
            {
                var regSip = GetCachedRegisteredSips().FirstOrDefault(rs => rs.Id == result.ChangedObjectId);
                if (regSip != null)
                {
                    regSip.Updated = DateTime.UtcNow;
                    return result;
                }
            }

            // Otherwise reload cache.
            _lazyCache.ClearRegisteredSips();
            return result;
        }
        
        public List<RegisteredSipDto> GetCachedRegisteredSips()
        {
            return _lazyCache.GetOrAddRegisteredSips(() => _internalRepository.GetCachedRegisteredSips());
        }
        
        public KamailioMessageHandlerResult DeleteRegisteredSip(string sipAddress)
        {
            var result = _internalRepository.DeleteRegisteredSip(sipAddress);

            if (result.ChangeStatus == KamailioMessageChangeStatus.CodecRemoved)
            {
                _lazyCache.ClearRegisteredSips();
            }

            return result;
        }

        public List<CodecInformation> GetCodecInformationList()
        {
            return _internalRepository.GetCodecInformationList();
        }

        public CodecInformation GetCodecInformation(string sipAddress)
        {
            return _internalRepository.GetCodecInformation(sipAddress);
        }

    }
}