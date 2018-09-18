using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CCM.Core.Entities;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using CCM.Data.Entities.Base;

namespace CCM.Data.Repositories
{
    //public abstract class EntityRepositoryBase<T,TU> : BaseRepository, IRepository<T> 
    //    where T : CoreEntityBaseWithId
    //    where TU : EntityBase
    //{
    //    protected EntityRepositoryBase(IMemoryCacheLoader memoryCacheLoader) : base(memoryCacheLoader)
    //    {
    //    }

    //    public void Delete(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<T> GetAll()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public T GetById(Guid id)
    //    {
    //        using (var db = GetDbContext())
    //        {
    //            Expression<Func<CityEntity, ICollection<LocationEntity>>> expression1 = c => c.Locations;
    //            Expression<Func<EntityBase, ICollection<LocationEntity>>> expression = expression1;

    //            EntityBase dbCity = db.Set<TU>()
    //                .Include(expression)
    //                .SingleOrDefault(g => g.Id == id);

    //            return MapToCoreObject(dbCity);
    //        }
    //    }

    //    protected abstract T MapToCoreObject(EntityBase dbObject);
       

    //    public void Save(T item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}