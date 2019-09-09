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
using System.Web.Mvc;
using CCM.Web.Mappers;
using CCM.Web.Models.ApiExternal;
using CCM.Web.Models.Home;
using Microsoft.AspNet.SignalR;

namespace CCM.Web.Hubs
{
    public class CodecStatusHub : HubBase
    {
        public static void UpdateCodecStatus(Guid id)
        {
            if (id == Guid.Empty)
            {
                return;
            }
            
            var codecStatusViewModelsProvider = (CodecStatusViewModelsProvider)DependencyResolver.Current.GetService(typeof(CodecStatusViewModelsProvider));
            var userAgentsOnline = codecStatusViewModelsProvider.GetAll();

            var updatedCodecStatus = userAgentsOnline.FirstOrDefault(x => x.Id == id);

            if (updatedCodecStatus != null)
            {
                log.Debug($"SignalR is sending codec status to clients. SipAddress: {updatedCodecStatus.SipAddress}, State: {updatedCodecStatus.State}");
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<CodecStatusHub>();
                hubContext.Clients.All.codecStatus(updatedCodecStatus);
            }
            else
            {
                log.Error($"Can't update Codec status hub. No codec online with id: {id}");
            }
        }

        public static void UpdateCodecStatusRemoved(CodecStatusViewModel codecStatus)
        {
            log.Debug($"SignalR is sending codec status to clients. SipAddress: {codecStatus.SipAddress}, State: {codecStatus.State}");
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CodecStatusHub>();
            hubContext.Clients.All.codecStatus(codecStatus);
        }
    }
}
