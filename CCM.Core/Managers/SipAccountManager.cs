using System;
using System.Collections.Generic;
using CCM.Core.Cache;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using LazyCache;
using NLog;

namespace CCM.Core.Managers
{
    public class SipAccountManager : ISipAccountManager
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly IAppCache _cache;

        public SipAccountManager(ISipAccountRepository ccmUserRepository, IAppCache cache)
        {
            _sipAccountRepository = ccmUserRepository;
            _cache = cache;
        }

        public void Create(SipAccount account)
        {
            if (_sipAccountRepository.GetByUserName(account.UserName) != null)
            {
                log.Warn("Can't create user. Username {0} already exists in CCM database", account.UserName);
                throw new ApplicationException("User name already exists.");
            }

            _sipAccountRepository.Create(account);
        }

        public bool Delete(Guid id)
        {
            _sipAccountRepository.Delete(id);
            return true;
        }

        public SipAccount GetById(Guid id)
        {
            return _sipAccountRepository.GetById(id);
        }

        public List<SipAccount> GetAll()
        {
            return _sipAccountRepository.GetAllIncludingRelations() ?? new List<SipAccount>();
        }

        public List<SipAccount> Find(string startsWith)
        {
            return _sipAccountRepository.Find(startsWith) ?? new List<SipAccount>();
        }

        public SipAccount GetByUserName(string userName)
        {
            return _sipAccountRepository.GetByUserName(userName);
        }

        public void Update(SipAccount account)
        {
            _sipAccountRepository.Update(account);
        }

        public void UpdateComment(Guid id, string comment)
        {
            _sipAccountRepository.UpdateComment(id, comment);

            // Invalidate registered sip in cache
            _cache.ClearRegisteredSips();
        }
        
        public void UpdatePassword(Guid id, string password)
        {
            _sipAccountRepository.UpdatePassword(id, password);
        }
        
    }
}