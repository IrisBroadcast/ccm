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
using System.Threading.Tasks;
using System.Web.Http;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Mappers;
using CCM.Web.Models.ApiExternal;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories.Specialized;
using System.Net;

namespace CCM.Web.Controllers.ApiExternal
{
    public class CodecStatusController : ApiController
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly ISettingsManager _settingsManager;
        private readonly ICodecInformationRepository _codecInformationRepository;
        private readonly ICodecManager _codecManager;

        public CodecStatusController(IRegisteredSipRepository registeredSipRepository, ISipAccountRepository sipAccountRepository,
            ISettingsManager settingsManager, ICodecInformationRepository codecInformationRepository, ICodecManager codecManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _sipAccountRepository = sipAccountRepository;
            _settingsManager = settingsManager;
            _codecInformationRepository = codecInformationRepository;
            _codecManager = codecManager;
        }

        [HttpGet]
        public IList<CodecStatus> GetAll(bool includeCodecsOffline = false)
        {
            List<RegisteredSipDto> allRegisteredSips = _registeredSipRepository.GetCachedRegisteredSips();

            if (includeCodecsOffline)
            {
                var sipIdsOnline = allRegisteredSips.Select(rs => rs.Sip).ToList();
                var accounts = _sipAccountRepository.GetAll();
                var accountsNotOnline = accounts.Where(a => !sipIdsOnline.Contains(a.UserName)).ToList();
                var sipDomain = _settingsManager.SipDomain;
                var notRegisteredSips = accountsNotOnline.Select(a => new RegisteredSipDto()
                {
                    Id = Guid.Empty,
                    Sip = a.UserName,
                    DisplayName = DisplayNameHelper.GetDisplayName("", a.DisplayName, string.Empty, "", a.UserName, "", sipDomain),
                }).ToList();
                allRegisteredSips.AddRange(notRegisteredSips);
            }

            return allRegisteredSips.Select(CodecStatusMapper.MapToCodecStatus).ToList();
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
                return new CodecStatus() { SipAddress = sipId, State = CodecState.NotRegistered, HasGpo = false };
            }

            return CodecStatusMapper.MapToCodecStatus(regSip);
        }

        [HttpPost]
        [Route("api/codecstatus/iscodeconline")]
        public async Task<bool> IsCodecOnline(string sipAddress)
        {
            // Returnerar true om kodaren är registrerad och nåbar via sitt API, annars false

            if (string.IsNullOrEmpty(sipAddress))
            {
                return false;
            }
            sipAddress = sipAddress.Trim().ToLower();

            var allRegisteredSips = _registeredSipRepository.GetCachedRegisteredSips();
            var regSip = allRegisteredSips.FirstOrDefault(s => s.Sip.Trim().ToLower() == sipAddress);

            if (regSip == null)
            {
                return false;
            }

            CodecInformation codecInfo = _codecInformationRepository.GetCodecInformationBySipAddress(sipAddress);
            
            // Ping codec
            var available = await _codecManager.CheckIfAvailableAsync(codecInfo);
            return available;
        }


        [HttpGet]
        public async Task<bool> GetInputEnabled(string sipAddress, int input)
        {
            CodecInformation codecInformation = GetCodecInformationBySipAddress(sipAddress);
            if (codecInformation == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                return await _codecManager.GetInputEnabledAsync(codecInformation, input);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<int> GetInputGain(string sipAddress, int input)
        {
            CodecInformation codecInformation = GetCodecInformationBySipAddress(sipAddress);
            if (codecInformation == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                return await _codecManager.GetInputGainLevelAsync(codecInformation, input);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<bool> SetInputEnabled(SetInputEnabledParameters2 parameters)
        {
            try
            {
                CodecInformation codecInformation = GetCodecInformationBySipAddress(parameters.SipAddress);
                if (codecInformation == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                return await _codecManager.SetInputEnabledAsync(codecInformation, parameters.Input, parameters.Enabled);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<int> IncreaseInputGain(ChangeGainParameters2 parameters)
        {
            return await SetGain(parameters, true);
        }

        [HttpPost]
        public async Task<int> DecreaseInputGain(ChangeGainParameters2 parameters)
        {
            return await SetGain(parameters, false);
        }

        private async Task<int> SetGain(ChangeGainParameters2 parameters, bool increase)
        {
            try
            {
                CodecInformation codecInformation = GetCodecInformationBySipAddress(parameters.SipAddress);
                if (codecInformation == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var gain = await _codecManager.GetInputGainLevelAsync(codecInformation, parameters.Input);
                var newGain = gain + (increase ? 1 : -1);
                return await _codecManager.SetInputGainLevelAsync(codecInformation, parameters.Input, newGain);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        private CodecInformation GetCodecInformationBySipAddress(string sipAddress)
        {
            var codecInfo = _codecInformationRepository.GetCodecInformationBySipAddress(sipAddress);
            return string.IsNullOrEmpty(codecInfo?.Api) ? null : codecInfo;
        }

        public class ChangeGainParameters2
        {
            public string SipAddress { get; set; }
            public int Input { get; set; }
        }

        public class SetInputEnabledParameters2
        {
            public string SipAddress { get; set; }
            public int Input { get; set; }
            public bool Enabled { get; set; }
        }

    }
}
