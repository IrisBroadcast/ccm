using System.Collections.Generic;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IAppCache cache) : base(cache)
        {
        }

        public List<CcmRole> GetRoles()
        {
            using (var db = GetDbContext())
            {
                var roles = db.Roles.ToList()
                    .Select(MapToRole)
                    .OrderBy(r => r.Name)
                    .ToList();
                return roles;
            }
        }
        
        private CcmRole MapToRole(RoleEntity dbRole)
        {
            return dbRole == null ? null : new CcmRole
            {
                Id = dbRole.Id,
                Name = dbRole.Name
            };
        }
    }
}