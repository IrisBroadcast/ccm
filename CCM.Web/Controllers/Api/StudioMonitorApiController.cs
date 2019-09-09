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
using System.Linq;
using System.Web.Http;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Infrastructure.WebApiFilters;
using CCM.Web.Mappers;
using CCM.Web.Models.ApiExternal;
using CCM.Web.Models.Home;
using NLog;

namespace CCM.Web.Controllers.Api
{
    [StudioMonitorExceptionFilter]
    public class StudioMonitorApiController : ApiController
    {
        private readonly IStudioRepository _studioRepository;

        public StudioMonitorApiController(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        [HttpGet]
        public CodecStatusViewModel GetCodecStatus(Guid studioId)
        {
            var studio = _studioRepository.GetById(studioId);

            if (studio?.CodecSipAddress == null)
            {
                return null;
            }
            // TODO: Alexander, Är detta sätt det bästa?
            var codecStatusViewModelsProvider = (CodecStatusViewModelsProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(CodecStatusViewModelsProvider));
            var userAgentsOnline = codecStatusViewModelsProvider.GetAll();

            var codecStatus = userAgentsOnline.FirstOrDefault(x => x.SipAddress == studio.CodecSipAddress);

            if (codecStatus == null)
            {
                return new CodecStatusViewModel
                {
                    SipAddress = studio.CodecSipAddress,
                    State = CodecState.NotRegistered
                };
            }

            return codecStatus;
        }

    }

}
