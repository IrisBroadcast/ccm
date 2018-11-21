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

        public SipAccount GetByRegisteredSip(Guid registeredSipId)
        {
            return _sipAccountRepository.GetByRegisteredSipId(registeredSipId);
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
