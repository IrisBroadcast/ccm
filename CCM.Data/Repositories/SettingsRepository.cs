using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public SettingsRepository(IAppCache cache) : base(cache)
        {
        }

        public List<Setting> GetAll()
        {
            using (var db = GetDbContext())
            {
                //log.Debug("Hash codes. Settings repository: {0}, Context:{1}", this.GetHashCode(), Context.GetHashCode());

                var settings = db.Settings.ToList();

                var list = settings.Select(setting => new Setting
                {
                    Name = setting.Name,
                    Id = setting.Id,
                    Value = setting.Value,
                    Description = setting.Description
                }).ToList();

                //log.Debug("Getting settings from database. {0}", string.Join(" ", list.Select(s => string.Format("{0}:{1}", s.Name,s.Value))));

                return list;
            }
        }

        public void Save(List<Setting> settings, string userName)
        {
            using (var db = GetDbContext())
            {
                DbSet<Entities.SettingEntity> existing = db.Settings;

                foreach (Setting setting in settings)
                {
                    Entities.SettingEntity dbSetting = existing.SingleOrDefault(s => s.Id == setting.Id);

                    if (dbSetting != null && dbSetting.Value != setting.Value)
                    {
                        dbSetting.Value = setting.Value;
                        dbSetting.UpdatedOn = DateTime.UtcNow;
                        dbSetting.UpdatedBy = userName;
                    }
                }

                db.SaveChanges();
            }
        }

    }
}