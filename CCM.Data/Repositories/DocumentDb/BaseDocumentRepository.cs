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
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CCM.Core.Entities.DocumentDb;
using CCM.Data.Entities.DocumentDb;
using LazyCache;
using Newtonsoft.Json;

namespace CCM.Data.Repositories.DocumentDb
{
    public abstract class BaseDocumentRepository<T, TU> : BaseDocumentRepository where T : DocumentDbObjectBase where TU : DocumentDbEntity, new()
    {
        protected BaseDocumentRepository(IAppCache cache) : base(cache)
        {
        }

        public List<T> GetAll()
        {
            using (var ctx = GetDbContext())
            {
                IQueryable<IGrouping<int, TU>> data = ctx.Set<TU>().GroupBy(r => r.ContentId);

                var list = new List<TU>();
                foreach (IGrouping<int, TU> grouping in data)
                {
                    var r = grouping.OrderByDescending(re => re.UpdatedDateTime).FirstOrDefault();
                    list.Add(r);
                }

                return list.Select(MapRowToObject).Where(r => r != null).ToList();
            }
        }

        public T GetById(int id)
        {
            using (var ctx = GetDbContext())
            {
                var data = ctx.Set<TU>().OrderByDescending(r => r.UpdatedDateTime).FirstOrDefault(r => r.ContentId == id);
                var region = MapRowToObject(data);
                return region;
            }
        }

        public T Save(T o)
        {
            using (var ctx = GetDbContext())
            {
                if (o == null)
                {
                    return null;
                }

                if (o.Id == 0)
                {
                    // New item
                    var dbSet = ctx.Set<TU>();
                    int? max = dbSet.Max(r => (int?)r.ContentId);
                    var nextId = (max ?? 0) + 1;
                    o.Id = nextId;
                }

                // TODO: Get user
                //PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
                //UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, Thread.CurrentPrincipal.Identity.Name);
                var userPrincipal = "Unknown";
                var json = JsonConvert.SerializeObject(o);
                var row = new TU()
                {
                    ContentId = o.Id,
                    JsonData = json,
                    UpdatedDateTime = DateTime.UtcNow,
                    UpdatedByUser = userPrincipal
                };
                ctx.Set<TU>().Add(row);
                ctx.SaveChanges();

                return o;
            }
        }

        public bool Delete(int id)
        {
            using (var ctx = GetDbContext())
            {
                var regions = ctx.Set<TU>().Where(r => r.ContentId == id);
                ctx.Set<TU>().RemoveRange(regions);
                int result = ctx.SaveChanges();
                return result > 0;
            }
        }

        public bool DeleteRow(int id)
        {
            using (var ctx = GetDbContext())
            {
                int result = 0;
                var config = ctx.Set<TU>().FirstOrDefault(c => c.Id == id);
                if (config != null)
                {
                    ctx.Set<TU>().Remove(config);
                    result = ctx.SaveChanges();
                }
                return result > 0;
            }
        }

        public bool UpdateDate(int id)
        {
            using (var ctx = GetDbContext())
            {
                int result = 0;
                var config = ctx.Set<TU>().FirstOrDefault(r => r.Id == id);
                if (config != null)
                {
                    //PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
                    //UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, Thread.CurrentPrincipal.Identity.Name);
                    var userPrincipal = "Unknown";

                    config.UpdatedDateTime = DateTime.UtcNow; // Sätter aktuellt datum vilket gör att den konfigurationen används nu
                    config.UpdatedByUser = userPrincipal;

                    ctx.Set<TU>().AddOrUpdate(config);
                    result = ctx.SaveChanges();
                }

                return result > 0;
            }
        }

        private T MapRowToObject(TU data)
        {
            if (data == null)
            {
                return default(T);
            }

            var o = JsonConvert.DeserializeObject<T>(data.JsonData);
            if (o != null)
            {
                o.Id = data.ContentId;
                o.UpdatedDateTime = data.UpdatedDateTime;
                o.UpdatedByUser = data.UpdatedByUser;
            }
            return o;
        }
    }

    public abstract class BaseDocumentRepository
    {
        private readonly IAppCache _cache;

        protected BaseDocumentRepository(IAppCache cache)
        {
            _cache = cache;
        }

        protected CcmDbContext GetDbContext()
        {
            return new CcmDbContext(_cache);
        }
    }
}
