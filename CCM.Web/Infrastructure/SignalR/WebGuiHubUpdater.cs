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

using CCM.Core.SipEvent;
using CCM.Web.Hubs;
using NLog;

namespace CCM.Web.Infrastructure.SignalR
{
    /// <summary>
    /// The codec status hub sends out codec/user-agent changes to CCM frontpage.
    /// Updates clients through SignalR.
    /// </summary>
    public class WebGuiHubUpdater : IGuiHubUpdater
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Update(SipEventHandlerResult updateResult)
        {
            log.Debug($"WebGuiHubUpdater. Status: {updateResult.ChangeStatus}, Id: {updateResult.ChangedObjectId}, SipAddress: {updateResult.SipAddress}");

            if (updateResult.ChangeStatus == SipEventChangeStatus.CallStarted)
            {
                WebGuiHub.ThrottlingUpdateOngoingCalls();
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CallClosed)
            {
                WebGuiHub.ThrottlingUpdateOngoingCalls();
                WebGuiHub.ThrottlingUpdateOldCalls();
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }

            if (updateResult.ChangeStatus == SipEventChangeStatus.CodecAdded ||
                updateResult.ChangeStatus == SipEventChangeStatus.CodecUpdated ||
                updateResult.ChangeStatus == SipEventChangeStatus.CodecRemoved)
            {
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }
        }
    }
}
