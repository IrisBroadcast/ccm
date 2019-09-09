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
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using CCM.Core.Cache;
using CCM.Data.Entities;
using CCM.Data.Entities.Base;
using CCM.Data.Entities.DocumentDb;
using LazyCache;
using NLog;

namespace CCM.Data
{
    public class CcmDbContext : DbContext
    {
        private readonly IAppCache _cache;
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CcmDbContext(IAppCache cache)
        {
            _cache = cache;

            if(ConfigurationManager.AppSettings["Environment"] == "Initiate")
            {
                //Database create database with any off these
                //Database.SetInitializer<CcmDbContext>(new CreateDatabaseIfNotExists<CcmDbContext>());
                //Database.SetInitializer<CcmDbContext>(new DropCreateDatabaseIfModelChanges<CcmDbContext>());
                //Database.SetInitializer<CcmDbContext>(new DropCreateDatabaseAlways<CcmDbContext>());
            }
            else
            {
                //Database initial creation (off) use for production
                Database.SetInitializer<CcmDbContext>(null);
            }
        }

        public DbSet<RegisteredSipEntity> RegisteredSips { get; set; }
        public DbSet<CallEntity> Calls { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SipAccountEntity> SipAccounts { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<ProfileGroupEntity> ProfileGroups { get; set; }
        public DbSet<ProfileGroupProfileOrdersEntity> ProfileGroupProfileOrders { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<OwnerEntity> Owners { get; set; }
        public DbSet<UserAgentEntity> UserAgents { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<FilterEntity> Filters { get; set; }
        public DbSet<CodecTypeEntity> CodecTypes { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<UserAgentProfileOrderEntity> UserAgentProfileOrders { get; set; }
        public DbSet<MetaTypeEntity> MetaTypes { get; set; }
        public DbSet<CallHistoryEntity> CallHistories { get; set; }
        public DbSet<CodecPresetEntity> CodecPresets { get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<StudioEntity> Studios { get; set; }

        // Document Db Entities
        public DbSet<CodecControlApiEntity> ApiDefinition { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAgentEntity>()
                .HasMany(e => e.CodecPresets)
                .WithMany(e => e.UserAgents)
                .Map(m => m.ToTable("CodecPresetUserAgents").MapLeftKey("UserAgent_Id").MapRightKey("CodecPreset_Id"));
        }

        protected string CurrentUserName()
        {
            return Thread.CurrentPrincipal?.Identity?.Name ?? "unknown";
        }

        public override int SaveChanges()
        {
            // Set changed/updated info
            var entities = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();
            foreach (var entity in entities)
            {
                var timeStamp = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).Id = Guid.NewGuid();
                    ((EntityBase)entity.Entity).CreatedOn = timeStamp;
                    ((EntityBase)entity.Entity).CreatedBy = CurrentUserName();
                }

                ((EntityBase)entity.Entity).UpdatedOn = timeStamp;
                ((EntityBase)entity.Entity).UpdatedBy = CurrentUserName();
            }

            // Should invalidate cache?
            bool shouldInvalidateCache = ChangeTracker.Entries().Any(
                x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
                );

            var saveChangesResult = base.SaveChanges();

            shouldInvalidateCache = shouldInvalidateCache && saveChangesResult > 0;

            if (shouldInvalidateCache)
            {
                log.Debug("Changes in data saved to database. Trigger full cache update.");
                // TODO: ZZZ This is too wide. Since it is affecting both saving of data and changing registered sips and everything else
                _cache.FullReload();
            }

            return saveChangesResult;
        }
    }
}
