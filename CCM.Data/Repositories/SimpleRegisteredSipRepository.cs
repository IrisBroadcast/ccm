using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class SimpleRegisteredSipRepository : BaseRepository, ISimpleRegisteredSipRepository
    {
        private readonly ISettingsManager _settingsManager;

     public SimpleRegisteredSipRepository(ISettingsManager settingsManager, IAppCache cache) : base(cache)
        {
            _settingsManager = settingsManager;
        }

        public RegisteredSipInfo GetRegisteredSipInfoById(Guid id)
        {
            using (var db = GetDbContext())
            {
                Entities.RegisteredSipEntity dbSip = db.RegisteredSips
                    .Include(rs => rs.User)
                    .SingleOrDefault(r => r.Id == id);

                return dbSip == null
                    ? null
                    : new RegisteredSipInfo
                    {
                        SipAddress = dbSip.SIP,
                        DisplayName = DisplayNameHelper.GetDisplayName(dbSip.DisplayName,
                            dbSip.User != null ? dbSip.User.DisplayName : string.Empty, string.Empty, dbSip.Username,
                            dbSip.SIP, "", _settingsManager.SipDomain)
                    };
            }
        }

        public RegisteredSip GetRegisteredSipById(Guid id)
        {
            using (var db = GetDbContext())
            {
                var dbSip = db.RegisteredSips.SingleOrDefault(r => r.Id == id);
                return MapToRegisteredSip(dbSip);
            }
        }

        public List<Guid> GetRegisteredSipIdsForUser(Guid userId)
        {
            using (var db = GetDbContext())
            {
                return db.RegisteredSips
                    .Where(r => r.User.Id == userId)
                    .Select(rs => rs.Id)
                    .ToList();
            }
        }

        private RegisteredSip MapToRegisteredSip(RegisteredSipEntity dbRegisteredSip)
        {
            if (dbRegisteredSip == null)
            {
                return null;
            }

            var registredSip = new RegisteredSip
            {
                DisplayName = dbRegisteredSip.DisplayName,
                Expires = dbRegisteredSip.Expires,
                Id = dbRegisteredSip.Id,
                IP = dbRegisteredSip.IP,
                Port = dbRegisteredSip.Port,
                SIP = dbRegisteredSip.SIP,
                ServerTimeStamp = dbRegisteredSip.ServerTimeStamp,
                Updated = dbRegisteredSip.Updated,
                UserAgentHead = dbRegisteredSip.UserAgentHead,
                Username = dbRegisteredSip.Username,
            };

            return registredSip;
        }
        
    }
}
