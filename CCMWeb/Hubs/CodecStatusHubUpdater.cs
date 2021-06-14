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
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.SipEvent;
using CCM.Web.Mappers;
using CCM.Web.Models.ApiExternal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace CCM.Web.Hubs
{
    /// <summary>
    /// The codec status hub sends out codec/user-agent changes to external clients.
    /// Updates clients through SignalR.
    /// </summary>
    public class CodecStatusHubUpdater : ICodecStatusHubUpdater
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ICachedCallRepository _cachedCallRepository;
        private readonly ICachedCallHistoryRepository _cachedCallHistoryRepository;
        private readonly CodecStatusViewModelsProvider _codecStatusViewModelsProvider;
        private readonly IHubContext<CodecStatusHub, ICodecStatusHub> _codecStatusHub;

        public CodecStatusHubUpdater(
            IServiceProvider serviceProvider,
            ICachedCallRepository cachedCallRepository,
            ICachedCallHistoryRepository cachedCallHistoryRepository,
            CodecStatusViewModelsProvider codecStatusViewModelsProvider)
        {
            _codecStatusHub = serviceProvider.GetService<IHubContext<CodecStatusHub, ICodecStatusHub>>(); // TODO: Injection???
            _cachedCallRepository = cachedCallRepository;
            _cachedCallHistoryRepository = cachedCallHistoryRepository;
            _codecStatusViewModelsProvider = codecStatusViewModelsProvider;
        }

        public void Update(SipEventHandlerResult updateResult)
        {
            if (updateResult.ChangeStatus == SipEventChangeStatus.CallStarted)
            {
                // Load call and update to and from codecs
                var callId = updateResult.ChangedObjectId;
                var callInfo = _cachedCallRepository.GetCallInfoById(callId);

                if (callInfo != null)
                {
                    log.Debug($"CodecStatusHub. Call started. From:{callInfo.FromId}, To:{callInfo.ToId}");
                    UpdateCodecStatusByGuid(callInfo.FromId);
                    UpdateCodecStatusByGuid(callInfo.ToId);
                }
                else
                {
                    log.Error($"CodecStatusHub. Call started but was not found in database. Call Id:{callId}");
                }
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CallClosed)
            {
                // Load call and update to and from codecs
                var callId = updateResult.ChangedObjectId;
                CallHistory call = _cachedCallHistoryRepository.GetCallHistoryByCallId(callId);

                if (call != null)
                {
                    log.Debug($"CodecStatusHub. Call closed. From:{call.FromId}, to:{call.ToId}, call id:{callId}");
                    UpdateCodecStatusByGuid(call.FromId);
                    UpdateCodecStatusByGuid(call.ToId);
                }
                else
                {
                    log.Error($"CodecStatusHub. Call closed but was not found in call history. Call id:{callId}");
                }
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CodecAdded)
            {
                UpdateCodecStatusByGuid(updateResult.ChangedObjectId);
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CodecUpdated)
            {
                UpdateCodecStatusByGuid(updateResult.ChangedObjectId);
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CodecRemoved)
            {
                var codecStatus = new CodecStatusViewModel
                {
                    State = CodecState.NotRegistered,
                    SipAddress = updateResult.SipAddress
                };
                UpdateCodecStatusRemoved(codecStatus);
            }

            log.Debug($"CodecStatusHub. Status:{updateResult.ChangeStatus}, id:{updateResult.ChangedObjectId}, sip address:{updateResult.SipAddress}");
        }

        private void UpdateCodecStatusRemoved(CodecStatusViewModel codecStatusViewModel)
        {
            log.Debug($"SignalR is sending codec status to clients. SipAddress: {codecStatusViewModel.SipAddress}, State: {codecStatusViewModel.State}");
            _codecStatusHub.Clients.All.CodecStatus(codecStatusViewModel);
        }

        private void UpdateCodecStatusByGuid(Guid id)
        {
            if (id == Guid.Empty)
            {
                return;
            }

            var userAgentsOnline = _codecStatusViewModelsProvider.GetAll();
            var updatedCodecStatus = userAgentsOnline.FirstOrDefault(x => x.Id == id);
            if (updatedCodecStatus != null)
            {
                log.Debug($"SignalR is sending codec status to clients. SipAddress: {updatedCodecStatus.SipAddress}, State: {updatedCodecStatus.State}");
                _codecStatusHub.Clients.All.CodecStatus(updatedCodecStatus);
            }
            else
            {
                log.Error($"Can't update Codec status hub. No codec online with id: {id}");
            }
        }
    }
}
