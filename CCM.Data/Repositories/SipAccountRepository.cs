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

        public void ImportSipAccountPasswords(List<UserInfo> userInfoList)
        {
            log.Info("Importerar lösenord för {0} SIP-konton", userInfoList.Count);

            try
            {
                using (var db = GetDbContext())
                {
                    foreach (var userInfo in userInfoList)
                    {
                        var dbUser = db.SipAccounts.SingleOrDefault(u => u.UserName == userInfo.UserName);

                        if (dbUser == null)
                        {
                            log.Warn("SIP-konto {0} saknas i CCM:s databas. Lägger till kontot.", userInfo.UserName);
                            var dbAccount = new SipAccountEntity
                            {
                                UserName = userInfo.UserName,
                                DisplayName = "",
                                Comment = "Konto importerat från Radius",
                                CodecType = db.CodecTypes.SingleOrDefault(c => c.Name == "Test"),
                                AccountLocked = false,
                                AccountType = SipAccountType.SipAccount,
                                Password = userInfo.Password
                            };

                            db.SipAccounts.Add(dbAccount);
                        }
                        else
                        {
                            log.Info("Importerar lösenord för SIP-konto {0}", dbUser.UserName);
                            dbUser.Password = userInfo.Password;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Fel då lösenord för SIP-konton importerades");
            }
        }
        
    }
}