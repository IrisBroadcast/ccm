using CCM.StatisticsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData
{
    public class StatsDbContext : DbContext
    {

        public StatsDbContext(DbContextOptions<StatsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/keys?tabs=fluent-api

            modelBuilder.Entity<ProfileGroupProfileOrdersEntity>()
                .HasKey(c => new { c.ProfileGroupId, c.ProfileId });
            // [Key, ForeignKey("ProfileGroup"), Column("ProfileGroup_Id", Order = 0)]
            // public Guid ProfileGroupId { get; set; }

            // [Key, ForeignKey("Profile"), Column("Profile_Id", Order = 1)]
            // public Guid ProfileId { get; set; }

            modelBuilder.Entity<CallHistoryEntity>().HasKey(c => new { c.Id });


            modelBuilder.Entity<UserAgentProfileOrderEntity>().HasKey(c => new { c.UserAgentId, c.ProfileId });
            //[Key, ForeignKey("UserAgent"), Column("UserAgentId", Order = 0)]
            //public Guid UserAgentId { get; set; }

            //[Key, ForeignKey("Profile"), Column("ProfileId", Order = 1)]
            //public Guid ProfileId { get; set; }


            // CodecType matching Users
            modelBuilder.Entity<CodecTypeEntity>(entity =>
            {
                entity.HasMany(ct => ct.SipAccounts);
            });
            //modelBuilder.Entity<SipAccountEntity>(entity =>
            //{
            //    entity.HasOne<CodecTypeEntity>();
            //});
            //modelBuilder.Entity<SipAccountEntity>().HasOne<CodecTypeEntity>().WithMany().HasForeignKey(ex => ex.CodecTypeId).HasConstraintName("CodecType_Id");

        }
        public DbSet<RegisteredCodecEntity> RegisteredCodecs { get; set; }
        public DbSet<CallEntity> Calls { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SipAccountEntity> SipAccounts { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<ProfileCodecEntity> Profiles { get; set; }
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
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
