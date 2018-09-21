using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class UserAgentRepository : BaseRepository, IUserAgentRepository
    {
        public UserAgentRepository(IAppCache cache) : base(cache)
        {
        }

        public void Save(UserAgent userAgent)
        {
            using (var db = GetDbContext())
            {
                UserAgentEntity dbUserAgent = null;

                if (userAgent.Id != Guid.Empty)
                {
                    dbUserAgent = db.UserAgents
                        .Include(ua => ua.OrderedProfiles)
                        .SingleOrDefault(a => a.Id == userAgent.Id);
                }

                if (dbUserAgent == null)
                {
                    dbUserAgent = new UserAgentEntity()
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = userAgent.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    userAgent.Id = dbUserAgent.Id;
                    userAgent.CreatedOn = dbUserAgent.CreatedOn;
                    db.UserAgents.Add(dbUserAgent);
                }

                if (dbUserAgent.OrderedProfiles != null)
                {
                    dbUserAgent.OrderedProfiles.Clear();
                }

                dbUserAgent.Ax = userAgent.Ax;
                dbUserAgent.Height = userAgent.Height;
                dbUserAgent.Identifier = userAgent.Identifier;
                dbUserAgent.Image = userAgent.Image;
                dbUserAgent.Name = userAgent.Name;
                dbUserAgent.Width = userAgent.Width;
                dbUserAgent.MatchType = userAgent.MatchType;
                dbUserAgent.UpdatedBy = userAgent.UpdatedBy;
                dbUserAgent.UpdatedOn = DateTime.UtcNow;
                dbUserAgent.Api = userAgent.Api;
                dbUserAgent.Lines = userAgent.Lines;
                dbUserAgent.Inputs = userAgent.Inputs;
                dbUserAgent.NrOfGpos = userAgent.NrOfGpos;
                dbUserAgent.MaxInputDb = userAgent.InputMaxDb;
                dbUserAgent.MinInputDb = userAgent.InputMinDb;
                dbUserAgent.UserInterfaceLink = userAgent.UserInterfaceLink;
                dbUserAgent.Comment = userAgent.Comment;
                dbUserAgent.InputGainStep = userAgent.InputGainStep;
                dbUserAgent.GpoNames = userAgent.GpoNames;
                dbUserAgent.UserInterfaceIsOpen = userAgent.UserInterfaceIsOpen;
                dbUserAgent.UseScrollbars = userAgent.UseScrollbars;

                userAgent.UpdatedOn = dbUserAgent.UpdatedOn;

                GetProfileEntitiesFromProfiles(db, dbUserAgent, userAgent.Profiles);
                GetDbCodecPresetsFromCodecPresets(db, dbUserAgent, userAgent.CodecPresets);

                db.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var db = GetDbContext())
            {
                var userAgent = db.UserAgents.SingleOrDefault(a => a.Id == id);

                if (userAgent != null)
                {
                    db.UserAgents.Remove(userAgent);
                    db.SaveChanges();
                }
            }
        }

        public UserAgent GetById(Guid id)
        {
            using (var db = GetDbContext())
            {
                var dbUserAgent = db.UserAgents.SingleOrDefault(a => a.Id == id);
                return dbUserAgent == null ? null : MapToUserAgent(dbUserAgent);
            }
        }

        public List<UserAgent> GetAll()
        {
            using (var db = GetDbContext())
            {
                var dbUserAgents = db.UserAgents
                    .Include(ua => ua.OrderedProfiles.Select(o => o.Profile))
                    .Include(ua => ua.CodecPresets)
                    .ToList();

                return dbUserAgents.Select(MapToUserAgent).OrderBy(a => a.Name).ToList();
            }
        }

        public List<UserAgent> Find(string search)
        {
            using (var db = GetDbContext())
            {
                var dbUserAgents = db.UserAgents
                    .Include(ua => ua.OrderedProfiles.Select(o => o.Profile))
                    .Include(ua => ua.CodecPresets)
                    .Where(u => u.Name.ToLower().Contains(search.ToLower()) ||
                                u.Identifier.ToLower().Contains(search.ToLower()))
                    .ToList();

                return dbUserAgents.Select(MapToUserAgent).OrderBy(a => a.Name).ToList();
            }
        }


        private UserAgent MapToUserAgent(UserAgentEntity dbUserAgent)
        {
            return new UserAgent()
            {
                Ax = dbUserAgent.Ax,
                UserInterfaceLink = dbUserAgent.UserInterfaceLink,
                Height = dbUserAgent.Height,
                Id = dbUserAgent.Id,
                Identifier = dbUserAgent.Identifier,
                Image = dbUserAgent.Image,
                Name = dbUserAgent.Name,
                Width = dbUserAgent.Width,
                MatchType = dbUserAgent.MatchType,
                CreatedBy = dbUserAgent.CreatedBy,
                CreatedOn = dbUserAgent.CreatedOn,
                UpdatedBy = dbUserAgent.UpdatedBy,
                UpdatedOn = dbUserAgent.UpdatedOn,
                Api = dbUserAgent.Api,
                Lines = dbUserAgent.Lines,
                Inputs = dbUserAgent.Inputs,
                NrOfGpos = dbUserAgent.NrOfGpos,
                InputMaxDb = dbUserAgent.MaxInputDb,
                InputMinDb = dbUserAgent.MinInputDb,
                Comment = dbUserAgent.Comment,
                InputGainStep = dbUserAgent.InputGainStep,
                GpoNames = dbUserAgent.GpoNames,
                UserInterfaceIsOpen = dbUserAgent.UserInterfaceIsOpen,
                UseScrollbars = dbUserAgent.UseScrollbars,
                Profiles = GetProfilesFromProfiles(dbUserAgent.OrderedProfiles),
                CodecPresets = dbUserAgent.CodecPresets.Select(MapToCodecPreset).ToList()
            };
        }

        private List<Profile> GetProfilesFromProfiles(IEnumerable<UserAgentProfileOrderEntity> orderedProfiles)
        {
            var profiles = orderedProfiles.OrderBy(o => o.SortIndex).Select(o => o.Profile).ToList();
            return profiles.Select(MapProfile).ToList();
        }

        private static Profile MapProfile(ProfileEntity profile)
        {
            return new Profile
            {
                Description = profile.Description,
                Id = profile.Id,
                Name = profile.Name,
                Sdp = profile.Sdp
            };
        }

        private static CodecPreset MapToCodecPreset(CodecPresetEntity dbCodecPreset)
        {
            return new CodecPreset()
            {
                Id = dbCodecPreset.Id,
                CreatedBy = dbCodecPreset.CreatedBy,
                CreatedOn = dbCodecPreset.CreatedOn,
                Name = dbCodecPreset.Name,
                UpdatedBy = dbCodecPreset.UpdatedBy,
                UpdatedOn = dbCodecPreset.UpdatedOn
            };
        }

        private void GetProfileEntitiesFromProfiles(CcmDbContext db, UserAgentEntity userAgent, IEnumerable<Profile> profiles)
        {
            userAgent.OrderedProfiles = userAgent.OrderedProfiles ?? new Collection<UserAgentProfileOrderEntity>();
            userAgent.OrderedProfiles.Clear();

            int sortIndex = 0;

            foreach (Profile profile in profiles)
            {
                var dbProfile = db.Profiles.SingleOrDefault(p => p.Id == profile.Id);
                if (dbProfile == null)
                {
                    continue;
                }

                userAgent.OrderedProfiles.Add(new UserAgentProfileOrderEntity()
                {
                    Profile = dbProfile,
                    UserAgent = userAgent,
                    SortIndex = sortIndex
                });

                sortIndex++;
            }
        }

        private void GetDbCodecPresetsFromCodecPresets(CcmDbContext db, UserAgentEntity userAgent, IEnumerable<CodecPreset> codecPresets)
        {
            userAgent.CodecPresets = userAgent.CodecPresets ?? new Collection<CodecPresetEntity>();
            userAgent.CodecPresets.Clear();

            foreach (var codecPreset in codecPresets)
            {
                var dbCodecPreset = db.CodecPresets.SingleOrDefault(c => c.Id == codecPreset.Id);
                if (dbCodecPreset == null)
                {
                    continue;
                }
                userAgent.CodecPresets.Add(dbCodecPreset);
            }
        }
    }
}