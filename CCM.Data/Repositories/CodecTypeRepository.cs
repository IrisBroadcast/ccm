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
using Microsoft.EntityFrameworkCore;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;
using NLog;

namespace CCM.Data.Repositories
{
    public class CodecTypeRepository : BaseRepository, ICodecTypeRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CodecTypeRepository(IAppCache cache, CcmDbContext ccmDbContext) : base(cache, ccmDbContext)
        {
        }

        public void Save(CodecType codecType)
        {
            var db = _ccmDbContext;
            CodecTypeEntity dbCodecType = null;

            if (codecType.Id != Guid.Empty)
            {
                dbCodecType = db.CodecTypes
                    .Include(c => c.SipAccounts)
                    .SingleOrDefault(c => c.Id == codecType.Id);
                if(dbCodecType == null)
                {
                    throw new Exception("Codec type could not be found");
                }
            }

            if (dbCodecType == null)
            {
                dbCodecType = new CodecTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = codecType.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };
                db.CodecTypes.Add(dbCodecType);
            }

            dbCodecType.Name = codecType.Name;
            dbCodecType.Color = codecType.Color;
            dbCodecType.UpdatedBy = codecType.UpdatedBy;
            dbCodecType.UpdatedOn = DateTime.UtcNow;

            db.SaveChanges();
        }

        public void Delete(Guid codecTypeId)
        {
            var db = _ccmDbContext;
            var dbCodecType = db.CodecTypes
                .Include(ct => ct.SipAccounts)
                .SingleOrDefault(c => c.Id == codecTypeId);

            if (dbCodecType != null)
            {
                if (dbCodecType.SipAccounts != null)
                {
                    dbCodecType.SipAccounts.Clear();
                }

                db.CodecTypes.Remove(dbCodecType);
                db.SaveChanges();
            }
        }

        public CodecType GetById(Guid codecTypeId)
        {
            if (codecTypeId == Guid.Empty)
            {
                return null;
            }

            var db = _ccmDbContext;
            var dbCodecType = db.CodecTypes
                .Include(ct => ct.SipAccounts)
                .SingleOrDefault(c => c.Id == codecTypeId);

            if (dbCodecType == null)
            {
                return null;
            }

            var codecType = MapToCodecType(dbCodecType);

            return codecType;
        }

        public List<CodecType> GetAll()
        {
            return GetAll(true);
        }

        public List<CodecType> GetAll(bool includeUsers)
        {
            if (includeUsers)
            {
                return _ccmDbContext.CodecTypes
                    .Include(it => it.SipAccounts)
                    .ThenInclude(acc => acc.Owner)
                    .AsEnumerable()
                    .Select(dbCodecType => MapToCodecType(dbCodecType, true))
                    .OrderBy(c => c.Name)
                    .ToList();

            }

            return _ccmDbContext.CodecTypes?
                .AsEnumerable()
                .Select(dbCodecType => MapToCodecType(dbCodecType, false))
                .OrderBy(c => c.Name)
                .ToList();
        }

        public List<CodecType> Find(string search, bool includeUsers = true)
        {
            var db = _ccmDbContext;
            var dbCodecTypes = db.CodecTypes
                .Include(ct => ct.SipAccounts)
                .Where(c => c.Name.ToLower().Contains(search.ToLower())).OrderBy(c => c.Name)
                .ToList();

            return dbCodecTypes
                .Select(dbCodecType => MapToCodecType(dbCodecType, includeUsers))
                .ToList();
        }

        private static CodecType MapToCodecType(CodecTypeEntity dbCodecType, bool includeUsers = true)
        {
            var codecType = new CodecType()
            {
                Id = dbCodecType.Id,
                Name = dbCodecType.Name,
                Color = dbCodecType.Color,
                CreatedBy = dbCodecType.CreatedBy,
                CreatedOn = dbCodecType.CreatedOn,
                UpdatedBy = dbCodecType.UpdatedBy,
                UpdatedOn = dbCodecType.UpdatedOn
            };

            if (includeUsers)
            {
                codecType.Users = dbCodecType.SipAccounts.Select(u => MapEntityToSipAccount(codecType, u)).ToList();
            }

            return codecType;
        }

        private static SipAccount MapEntityToSipAccount(CodecType codecType, SipAccountEntity user)
        {
            return new SipAccount
            {
                Id = user.Id,
                UserName = user.UserName,
                CodecType = codecType,
                Comment = user.Comment,
                DisplayName = user.DisplayName,
                Owner = user.Owner != null ? new Owner { Id = user.Owner.Id, Name = user.Owner.Name } : null,
                AccountLocked = user.AccountLocked,
            };
        }
    }
}
