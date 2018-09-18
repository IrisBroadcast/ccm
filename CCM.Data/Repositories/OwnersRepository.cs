using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class OwnersRepository : BaseRepository, IOwnersRepository
    {
        public OwnersRepository(IAppCache cache) : base(cache)
        {
        }

        public void Save(Owner owner)
        {
            using (var db = GetDbContext())
            {
                OwnerEntity dbOwner = null;


                if (owner.Id != Guid.Empty)
                {
                    dbOwner = db.Owners.SingleOrDefault(o => o.Id == owner.Id);
                }

                if (dbOwner == null)
                {
                    dbOwner = new OwnerEntity()
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = owner.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    db.Owners.Add(dbOwner);
                    owner.Id = dbOwner.Id;
                    owner.CreatedOn = dbOwner.CreatedOn;
                }


                dbOwner.Name = owner.Name;
                dbOwner.UpdatedBy = owner.UpdatedBy;
                dbOwner.UpdatedOn = DateTime.UtcNow;
                owner.UpdatedOn = dbOwner.UpdatedOn;

                db.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var db = GetDbContext())
            {
                OwnerEntity owner = db.Owners.SingleOrDefault(o => o.Id == id);
                if (owner == null)
                {
                    return;
                }

                db.Owners.Remove(owner);
                db.SaveChanges();
            }
        }

        public List<Owner> GetAll()
        {
            using (var db = GetDbContext())
            {
                var dbOwners = db.Owners.ToList();
                return dbOwners.Select(owner => MapOwner(owner)).OrderBy(o => o.Name).ToList();
            }
        }

        public Owner GetById(Guid id)
        {
            using (var db = GetDbContext())
            {
                var owner = db.Owners.SingleOrDefault(o => o.Id == id);
                return owner == null ? null : MapOwner(owner);
            }
        }

        public Owner GetByName(string name)
        {
            using (var db = GetDbContext())
            {
                name = (name ?? string.Empty).ToLower();

                var owner = db.Owners.SingleOrDefault(o => o.Name.ToLower() == name);
                return owner == null ? null : MapOwner(owner, false);
            }
        }

        public List<Owner> FindOwners(string search)
        {
            using (var db = GetDbContext())
            {
                var dbOwners = db.Owners.Where(o => o.Name.ToLower().Contains(search.ToLower())).ToList();
                return dbOwners.Select(owner => MapOwner(owner)).OrderBy(o => o.Name).ToList();
            }
        }

        private Owner MapOwner(OwnerEntity owner, bool includeUsers = true)
        {
            return owner == null ? null : new Owner
                {
                    Id = owner.Id,
                    Name = owner.Name,
                    CreatedBy = owner.CreatedBy,
                    CreatedOn = owner.CreatedOn,
                    UpdatedBy = owner.UpdatedBy,
                    UpdatedOn = owner.UpdatedOn,
                    Users = includeUsers && owner.Users != null
                        ? owner.Users.Select(MapUser).OrderBy(u => u.UserName).ToList()
                        : new List<SipAccount>()
                };
        }

        private SipAccount MapUser(SipAccountEntity dbUser)
        {
            return new SipAccount
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName,
                DisplayName = dbUser.DisplayName,
                Comment = dbUser.Comment
            };
        }

    }
}