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
using System.Web.Http;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Mappers;
using CCM.Web.Models.ApiExternal;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;

namespace CCM.Web.Controllers.ApiExternal
{
    /// <summary>
    /// Used by Dialers
    /// </summary>
    public class CodecStatusController : ApiController
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly ISettingsManager _settingsManager;

        public CodecStatusController(IRegisteredSipRepository registeredSipRepository, ISipAccountRepository sipAccountRepository,
            ISettingsManager settingsManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _sipAccountRepository = sipAccountRepository;
            _settingsManager = settingsManager;
        }

        [HttpGet]
        public IList<CodecStatus> GetAll(bool includeCodecsOffline = false)
        {
            IEnumerable<RegisteredSipDto> allRegisteredSips = _registeredSipRepository.GetCachedRegisteredSips();

            var notRegisteredSips = Enumerable.Empty<RegisteredSipDto>();
            if (includeCodecsOffline)
            {
                var sipIdsOnline = allRegisteredSips.Select(rs => rs.Sip).ToList();
                var accounts = _sipAccountRepository.GetAll();
                var accountsNotOnline = accounts.Where(a => !sipIdsOnline.Contains(a.UserName));
                var sipDomain = _settingsManager.SipDomain;
                notRegisteredSips = accountsNotOnline.Select(a => new RegisteredSipDto
                {
                    Id = Guid.Empty,
                    Sip = a.UserName,
                    DisplayName = DisplayNameHelper.GetDisplayName("", a.DisplayName, string.Empty, "", a.UserName, "", sipDomain),
                });
            }

            return allRegisteredSips.Concat(notRegisteredSips)
                                    .Select(CodecStatusMapper.MapToCodecStatus)
                                    .ToList();
        }

        [HttpGet]
        public CodecStatus Get(string sipId)
        {
            RegisteredSipDto regSip = null;

            if (!string.IsNullOrEmpty(sipId))
            {
                var allRegisteredSips = _registeredSipRepository.GetCachedRegisteredSips();
                regSip = allRegisteredSips.FirstOrDefault(s => s.Sip == sipId);
            }

            if (regSip == null)
            {
                return new CodecStatus { SipAddress = sipId, State = CodecState.NotRegistered, HasGpo = false };
            }

            return CodecStatusMapper.MapToCodecStatus(regSip);
        }

    }
}
