using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using AutoMapper;
using CCM.Core.Entities;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class ProfileGroupRepository : BaseRepository, IProfileGroupRepository
    {
        public ProfileGroupRepository(IAppCache cache) : base(cache)
        {
        }

        public List<ProfileGroup> GetAll()
        {
            using (var db = GetDbContext())
            {
                return db.ProfileGroups
                    .Include(g => g.OrderedProfiles.Select(op => op.Profile))
                    .ToList()
                    .Select(g => Mapper.Map<ProfileGroup>(g)).ToList();
            }
        }

        public ProfileGroup GetById(Guid id)
        {
            using (var db = GetDbContext())
            {
                var group = db.ProfileGroups
                    .Include(g => g.OrderedProfiles.Select(op => op.Profile))
                    .SingleOrDefault(g => g.Id == id);
                var profileGroup = Mapper.Map<ProfileGroup>(group);

                return profileGroup;
            }
        }

        public void Save(ProfileGroup profileGroup)
        {
            using (var db = GetDbContext())
            {
                bool nameCollision = db.ProfileGroups.Any(p => p.Name == profileGroup.Name && p.Id != profileGroup.Id);
                if (nameCollision)
                {
                    throw new DuplicateNameException();
                }

                ProfileGroupEntity dbProfileGroup;

                if (profileGroup.Id != Guid.Empty)
                {
                    // Uppdatera
                    dbProfileGroup = db.ProfileGroups.SingleOrDefault(g => g.Id == profileGroup.Id);
                    if (dbProfileGroup == null)
                    {
                        throw new Exception("Group could not be found");
                    }

                    Mapper.Map(profileGroup, dbProfileGroup);

                    dbProfileGroup.OrderedProfiles.Where(op => !profileGroup.Profiles.Any(sp => sp.Id == op.ProfileId))
                        .ToList()
                        .ForEach(pg =>
                            {
                                dbProfileGroup.OrderedProfiles.Remove(pg);
                            }
                        );

                    var newProfiles = new List<ProfileGroupProfileOrdersEntity>();

                    profileGroup.Profiles.Where(sp => !dbProfileGroup.OrderedProfiles.Any(op => op.ProfileId == sp.Id))
                        .ToList()
                        .ForEach(sp =>
                        {
                            var pgpo = new ProfileGroupProfileOrdersEntity()
                            {
                                ProfileGroupId = dbProfileGroup.Id,
                                ProfileId = sp.Id,
                            };
                            newProfiles.Add(pgpo);
                            dbProfileGroup.OrderedProfiles.Add(pgpo);
                        });

                    int i = 0;
                    foreach (var p in profileGroup.Profiles.OrderBy(sp => sp.SortIndex))
                    {
                        dbProfileGroup.OrderedProfiles.Where(op => p.Id == op.ProfileId).SingleOrDefault().SortIndex =
                            i++;
                    }
                }
                else
                {
                    // Ny
                    profileGroup.Id = Guid.NewGuid();
                    dbProfileGroup = Mapper.Map<ProfileGroupEntity>(profileGroup);
                    dbProfileGroup.OrderedProfiles = new List<ProfileGroupProfileOrdersEntity>();

                    profileGroup.Profiles.ForEach(profile =>
                        dbProfileGroup.OrderedProfiles.Add(new ProfileGroupProfileOrdersEntity()
                            {
                                ProfileGroupId = dbProfileGroup.Id,
                                ProfileId = profile.Id,
                            }
                        ));

                    int i = 0;
                    foreach (var p in profileGroup.Profiles.OrderBy(sp => sp.SortIndex))
                    {
                        dbProfileGroup.OrderedProfiles.Where(op => p.Id == op.ProfileId).SingleOrDefault().SortIndex =
                            i++;
                    }

                    dbProfileGroup.CreatedBy = profileGroup.CreatedBy;
                    dbProfileGroup.CreatedOn = profileGroup.CreatedOn;

                    db.ProfileGroups.Add(dbProfileGroup);
                }

                db.SaveChanges();
            }
        }


        public void Delete(Guid id)
        {
            using (var db = GetDbContext())
            {
                var group = db.ProfileGroups.SingleOrDefault(g => g.Id == id);
                if (group != null)
                {
                    db.ProfileGroups.Remove(group);
                    db.SaveChanges();
                }
            }
        }

    }
}