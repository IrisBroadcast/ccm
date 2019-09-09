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
using System.Threading.Tasks;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    public class CcmUserRepository : BaseRepository, ICcmUserRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CcmUserRepository(IAppCache cache) : base(cache)
        {
        }

        public bool Create(CcmUser ccmUser)
        {
            using (var db = GetDbContext())
            {
                var dbUser = new UserEntity();
                MapUserEntity(db, ccmUser, dbUser);

                db.Users.Add(dbUser);
                var result = db.SaveChanges();
                ccmUser.Id = dbUser.Id;
                return result >= 1;
            }
        }

        public bool Update(CcmUser ccmUser)
        {
            using (var db = GetDbContext())
            {
                var dbUser = db.Users.SingleOrDefault(u => u.Id == ccmUser.Id);
                if (dbUser == null)
                {
                    return false;
                }

                MapUserEntity(db, ccmUser, dbUser);

                var result = db.SaveChanges();
                return result == 1;
            }
        }

        public void UpdatePassword(Guid id, string passwordHash, string salt)
        {
            using (var db = GetDbContext())
            {
                UserEntity dbUser = db.Users.SingleOrDefault(u => u.Id == id);

                if (dbUser != null)
                {
                    dbUser.PasswordHash = passwordHash;
                    dbUser.Salt = salt;
                    db.SaveChanges();
                }
            }
        }

        public bool Delete(Guid userId)
        {
            using (var db = GetDbContext())
            {
                UserEntity user = db.Users.SingleOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return false;
                }

                db.Users.Remove(user);
                var result = db.SaveChanges();
                return result == 1;
            }
        }

        public CcmUser GetById(Guid userId)
        {
            using (var db = GetDbContext())
            {
                UserEntity user = db.Users.SingleOrDefault(u => u.Id == userId);
                return MapToCcmUser(user);
            }
        }

        public List<CcmUser> GetAll()
        {
            using (var db = GetDbContext())
            {
                var users = db.Users
                    .Include(u => u.Role)
                    .ToList();

                return users.Select(MapToCcmUser).OrderBy(u => u.UserName).ToList();
            }
        }

        public List<CcmUser> FindUsers(string startsWith)
        {
            using (var db = GetDbContext())
            {
                var users = db.Users
                    .Where(u => u.UserName.Contains(startsWith))
                    .ToList();
                return users.Select(MapToCcmUser).OrderBy(u => u.UserName).ToList();
            }
        }

        public CcmUser GetByUserName(string userName)
        {
            using (var db = GetDbContext())
            {
                UserEntity user = db.Users.SingleOrDefault(u => u.UserName == userName);
                return MapToCcmUser(user);
            }
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            using (var db = GetDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return false;
                }

                string hash = CryptoHelper.Md5HashSaltedPassword(password, user.Salt);
                var success = (hash == user.PasswordHash);

                if (!success)
                {
                    log.Info("Authentication failed for user {0}", username);
                }

                return success;

            }
        }

        public CcmRole GetUserRole(CcmUser ccmUser)
        {
            using (var db = GetDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Id == ccmUser.Id);

                return user?.Role == null ? null : new CcmRole { Id = user.Role.Id, Name = user.Role.Name };
            }
        }

        private static CcmUser MapToCcmUser(UserEntity dbUser)
        {
            return dbUser == null ? null : new CcmUser
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                Comment = dbUser.Comment,
                RoleId = dbUser.Role?.Id.ToString() ?? string.Empty,
                Role = dbUser.Role?.Name ?? string.Empty,
            };
        }

        private void MapUserEntity(CcmDbContext cxt, CcmUser ccmUser, UserEntity dbUser)
        {
            dbUser.UserName = ccmUser.UserName;
            dbUser.FirstName = ccmUser.FirstName;
            dbUser.LastName = ccmUser.LastName;
            dbUser.Comment = ccmUser.Comment;

            var role = string.IsNullOrWhiteSpace(ccmUser.RoleId) ? null : cxt.Roles.SingleOrDefault(r => r.Id == new Guid(ccmUser.RoleId));

            // Only admins allowed to assign admin role to account
            if (CurrentUserIsAdmin() || role == null || role.Name != Roles.Admin)
            {
                dbUser.Role = role;
            }

            if (!string.IsNullOrEmpty(ccmUser.Password))
            {
                var saltBytes = CryptoHelper.GenerateSaltBytes();
                dbUser.PasswordHash = CryptoHelper.Md5HashSaltedPassword(ccmUser.Password, saltBytes);
                dbUser.Salt = Convert.ToBase64String(saltBytes);
            }
        }
    }
}
