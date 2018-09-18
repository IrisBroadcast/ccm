using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using CCM.Core.Cache;
using CCM.Data.Entities;
using CCM.Data.Entities.Base;
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
            Database.SetInitializer<CcmDbContext>(null);
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
        public DbSet<LocalPasswordEntity> LocalPasswords { get; set; }
        public DbSet<CodecTypeEntity> CodecTypes { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<UserAgentProfileOrderEntity> UserAgentProfileOrders { get; set; }
        public DbSet<MetaTypeEntity> MetaTypes { get; set; }
        public DbSet<CallHistoryEntity> CallHistories { get; set; }
        public DbSet<CodecPresetEntity> CodecPresets { get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<StudioEntity> Studios { get; set; }


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
            return Thread.CurrentPrincipal?.Identity?.Name ?? "okänt";
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
                log.Debug("Föränding i konfigureringsdata sparad till databasen. Triggar full uppdatering av lokal cache");
                _cache.FullReload();
            }

            return saveChangesResult;
        }
    }
}