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
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IAppCache cache, CcmDbContext ccmDbContext) : base(cache, ccmDbContext)
        {
        }

        public List<CcmRole> GetRoles()
        {
            var roles = _ccmDbContext.Roles.ToList()
                .Select(MapToRole)
                .OrderBy(r => r.Name)
                .ToList();
            return roles;
        }

        public CcmRole GetById(string roleId)
        {
            return string.IsNullOrWhiteSpace(roleId) ? null : GetById(new Guid(roleId)); 
        }

        public CcmRole GetById(Guid roleId)
        {
            RoleEntity role = _ccmDbContext.Roles.SingleOrDefault(r => r.Id == roleId);
            return MapToRole(role);
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
