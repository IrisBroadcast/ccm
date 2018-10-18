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
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using CCM.Core.Entities;
using CCM.Core.Enums;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{

    public class SipAccountRepository : BaseRepository, ISipAccountRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public SipAccountRepository(IAppCache cache) : base(cache)
        {
        }

        public void Create(SipAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            using (var db = GetDbContext())
            {
                var dbAccount = new SipAccountEntity();
                SetEntityFromSipAccount(db, account, dbAccount);
                db.SipAccounts.Add(dbAccount);
                db.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var db = GetDbContext())
            {
                SipAccountEntity account = db.SipAccounts.SingleOrDefault(u => u.Id == id);

                if (account != null)
                {
                    account.RegisteredSips?.Clear();
                    db.SipAccounts.Remove(account);
                    db.SaveChanges();
                }
                else
                {
                    log.Info("Could not find user {0}", id);
                }
            }
        }

        public SipAccount GetById(Guid id)
        {
            using (var db = GetDbContext())
            {
                SipAccountEntity dbAccount = db.SipAccounts.SingleOrDefault(u => u.Id == id);
                return MapToSipAccount(dbAccount);
            }
        }

        public List<SipAccount> GetAllIncludingRelations()
        {
            using (var db = GetDbContext())
            {
                var users = db.SipAccounts
                    .Include(u => u.Owner)
                    .Include(u => u.CodecType)
                    .ToList();

                return users.Select(MapToSipAccount).OrderBy(u => u.UserName).ToList();
            }
        }
        public List<SipAccount> GetAll()
        {
            using (var db = GetDbContext())
            {
                return db.SipAccounts.ToList().Select(MapToSipAccount).OrderBy(u => u.UserName).ToList();
            }
        }
        public List<SipAccount> Find(string startsWith)
        {
            using (var db = GetDbContext())
            {
                var users = db.SipAccounts
                    .Where(u => u.UserName.Contains(startsWith))
                    .ToList();
                return users.Select(MapToSipAccount).OrderBy(u => u.UserName).ToList();
            }
        }

        public SipAccount GetByUserName(string userName)
        {
            using (var db = GetDbContext())
            {
                SipAccountEntity user = db.SipAccounts.SingleOrDefault(u => u.UserName == userName);
                return MapToSipAccount(user);
            }
        }

        public void Update(SipAccount ccmUser)
        {
            using (var db = GetDbContext())
            {
                SipAccountEntity dbUser = db.SipAccounts.SingleOrDefault(u => u.Id == ccmUser.Id);
                if (dbUser != null)
                {
                    SetEntityFromSipAccount(db, ccmUser, dbUser);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateComment(Guid id, string comment)
        {
            using (var db = GetDbContext())
            {
                SipAccountEntity dbUser = db.SipAccounts.SingleOrDefault(u => u.Id == id);

                if (dbUser != null)
                {
                    dbUser.Comment = comment;
                    db.SaveChanges();
                }
            }
        }

        public void UpdatePassword(Guid id, string password)
        {
            using (var db = GetDbContext())
            {
                SipAccountEntity dbUser = db.SipAccounts.SingleOrDefault(u => u.Id == id);

                if (dbUser != null)
                {
                    dbUser.Password = password;
                    db.SaveChanges();
                }
            }
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            using (var db = GetDbContext())
            {
                var user = await db.SipAccounts.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
                return user != null;
            }
        }

       

        private SipAccount MapToSipAccount(SipAccountEntity dbAccount)
        {
            return dbAccount == null ? null : new SipAccount
            {
                Id = dbAccount.Id,
                UserName = dbAccount.UserName,
                DisplayName = dbAccount.DisplayName,
                Comment = dbAccount.Comment,
                ExtensionNumber = dbAccount.ExtensionNumber,
                AccountLocked = dbAccount.AccountLocked,
                AccountType = dbAccount.AccountType,
                Owner = MapToOwner(dbAccount.Owner),
                CodecType = MaptoCodecType(dbAccount.CodecType)
            };
        }

        private Owner MapToOwner(OwnerEntity dbOwner)
        {
            return dbOwner != null ? new Owner { Id = dbOwner.Id, Name = dbOwner.Name } : null;
        }

        private CodecType MaptoCodecType(CodecTypeEntity dbCodecType)
        {
            return dbCodecType != null
                ? new CodecType { Id = dbCodecType.Id, Name = dbCodecType.Name, Color = dbCodecType.Color }
                : null;
        }

        private void SetEntityFromSipAccount(CcmDbContext cxt, SipAccount account, SipAccountEntity dbAccount)
        {
            dbAccount.Id = account.Id;
            dbAccount.UserName = account.UserName;
            dbAccount.Comment = account.Comment;
            dbAccount.ExtensionNumber = account.ExtensionNumber;
            dbAccount.DisplayName = account.DisplayName;
            dbAccount.Owner = account.Owner != null ? cxt.Owners.SingleOrDefault(o => o.Id == account.Owner.Id) : null;
            dbAccount.CodecType = account.CodecType != null ? cxt.CodecTypes.SingleOrDefault(c => c.Id == account.CodecType.Id) : null;
            dbAccount.AccountLocked = account.AccountLocked;
            dbAccount.AccountType = account.AccountType;

            if (!string.IsNullOrEmpty(account.Password))
            {
                dbAccount.Password = account.Password;
            }
        }
        
    }
}
