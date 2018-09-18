using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public void ImportLocalPasswords()
        {
            log.Info("Importerar lokala lösenord");

            using (var db = GetDbContext())
            {
                var localPasswords = db.LocalPasswords.ToList();

                log.Info("Hittade {0} lokala lösenord", localPasswords.Count);

                foreach (var lp in localPasswords)
                {
                    var dbUser = db.Users.SingleOrDefault(u => u.Id == lp.UserId);

                    if (dbUser == null)
                    {
                        log.Warn("Kontot med id {0} saknas som CCM-användare", lp.UserId);
                    }
                    else
                    {
                        log.Info("Importerar lokalt lösenord för {0}", dbUser.UserName);
                        dbUser.PasswordHash = lp.Password;
                        var saltBytes = Encoding.UTF8.GetBytes(lp.Salt);
                        dbUser.Salt = Convert.ToBase64String(saltBytes);
                    }
                }

                db.SaveChanges();
            }
        }

        public void ImportCcmUserPasswords(List<UserInfo> userInfoList)
        {
            log.Info("Importerar lösenord för {0} CCM-användare", userInfoList.Count);

            try
            {
                using (var db = GetDbContext())
                {
                    foreach (var userInfo in userInfoList)
                    {
                        var dbUser = db.Users.SingleOrDefault(u => u.UserName.ToLower() == userInfo.UserName.ToLower());

                        if (dbUser == null)
                        {
                            log.Warn("Användare {0} saknas i CCM:s databas", userInfo.UserName);
                        }
                        else
                        {
                            log.Info("Importerar lösenord för CCM-användare {0}", dbUser.UserName);

                            // Convert radius hash to password hash + salt
                            var radiusHash = userInfo.Password;
                            byte[] radiusHashBytes = Convert.FromBase64String(radiusHash);
                            log.Debug("Radius Hash:" + radiusHash + " " + AsHexString(radiusHashBytes));

                            var pwdHash = radiusHashBytes.Take(16).ToArray();
                            var pwdHashString = Convert.ToBase64String(pwdHash);
                            log.Debug("PwdHash:" + pwdHashString + " " + AsHexString(pwdHash));

                            var saltBytes = radiusHashBytes.Skip(16).ToArray();
                            var salt64 = Convert.ToBase64String(saltBytes);
                            log.Debug("Salt:" + salt64 + " " + AsHexString(saltBytes));

                            dbUser.PasswordHash = pwdHashString;
                            dbUser.Salt = salt64;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Fel då lösenord för CCM-användare importerades");
            }
        }

        public static string AsHexString(byte[] bytes)
        {
            return String.Concat("0x", Array.ConvertAll(bytes, x => x.ToString("X2")));
        }
    }

}