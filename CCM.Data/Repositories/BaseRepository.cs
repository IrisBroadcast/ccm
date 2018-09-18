using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using CCM.Core.Entities.Base;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories.Base;
using CCM.Data.Entities.Base;
using LazyCache;

namespace CCM.Data.Repositories
{
    public abstract class BaseRepository<T, TU> : BaseRepository, IRepository<T> where T : CoreEntityBase, new() where TU : EntityBase
    {
        protected BaseRepository(IAppCache cache) : base(cache)
        {
        }

        public abstract void Save(T item);

        public abstract void Delete(Guid id);

        public virtual T GetById(Guid id)
        {
            return GetById(id, null);
        }

        public virtual T GetById(Guid id, Expression<Func<TU, object>> includeExpression)
        {
            using (var db = GetDbContext())
            {
                IQueryable<TU> query = db.Set<TU>();
                query = includeExpression != null ? query.Include(includeExpression) : query;
                var dbEntity = query.SingleOrDefault(g => g.Id == id);
                return MapToCoreObject(dbEntity);
            }
        }

        public abstract List<T> GetAll();

        protected virtual List<T> GetAll(Expression<Func<TU, object>> includeExpression)
        {
            using (var db = GetDbContext())
            {
                IQueryable<TU> query = db.Set<TU>();
                query = includeExpression != null ? query.Include(includeExpression) : query;
                var dbEntities = query.ToList();
                return dbEntities.Select(MapToCoreObject).ToList();
            }
        }

        protected List<T> GetList(
            Expression<Func<TU, bool>> whereExpression, 
            Expression<Func<TU, object>> includeExpression,
            Func<T, object> orderbyFunction)
        {
            using (var db = GetDbContext())
            {
                IQueryable<TU> query = db.Set<TU>();
                if (includeExpression != null)
                {
                    query = query.Include(includeExpression);
                }

                if (whereExpression != null)
                {
                    query = query.Where(whereExpression);
                }
                var dbEntities = query.ToList();

                var list = dbEntities.Select(MapToCoreObject);
                if (orderbyFunction != null)
                {
                    list = list.OrderBy(orderbyFunction);
                }
                return list.ToList();
            }
        }

        public abstract T MapToCoreObject(TU dbEntity);

    }


    public abstract class BaseRepository
    {
        private readonly IAppCache _cache;

        protected BaseRepository(IAppCache cache)
        {
            _cache = cache;
        }

        protected CcmDbContext GetDbContext()
        {
            return new CcmDbContext(_cache);
        }

        protected  bool CurrentUserIsAdmin()
        {
            return Thread.CurrentPrincipal.IsInRole(Roles.Admin);
        }
        
        protected string CurrentUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }
    }
}