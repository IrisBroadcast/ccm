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
using System.Threading.Tasks;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using NLog;

namespace CCM.CodecControl.Mandozzi.Umac
{
    /// <summary>
    /// Codec control implementation for the Mandozzi Umac codec
    /// This implementation is experimental
    /// </summary>
    public class UmacApi : ICodecApi
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public Task<bool> CheckIfAvailableAsync(string ip)
        {
            log.Debug("Checking if codec at " + ip + " is reachable");
            try
            {
                using (var client = new UmacClient(ip, Sdk.Umac.ExternalProtocolIpCommandsPort))
                {
                    return Task.FromResult(true);
                }
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool?> GetGpoAsync(string ipp, int gpio) { throw new NotImplementedException(); }

        public Task<bool> GetInputEnabledAsync(string ip, int input) { throw new NotImplementedException(); }

        public Task<int> GetInputGainLevelAsync(string ip, int input) { throw new NotImplementedException(); }

        public Task<LineStatus> GetLineStatusAsync(string ip, int line)
        {
            log.Debug("Getting line status from Umac codec at {0}", ip);

            try
            {
                using (var client = new UmacClient(ip, Sdk.Umac.ExternalProtocolIpCommandsPort))
                {
                    var lineStatus = client.GetLineStatus();
                    return Task.FromResult(lineStatus);
                }
            }
            catch (Exception)
            {
                return Task.FromResult(new LineStatus
                {
                    StatusCode = LineStatusCode.ErrorGettingStatus,
                    DisconnectReason = DisconnectReason.None
                });
            }

        }

        public Task<string> GetLoadedPresetNameAsync(string ip, string lastPresetName) { throw new NotImplementedException(); }

        public Task<VuValues> GetVuValuesAsync(string ip) { throw new NotImplementedException(); }

        public Task<AudioMode> GetAudioModeAsync(string ip) { throw new NotImplementedException(); }

        public Task<AudioStatus> GetAudioStatusAsync(string hostAddress, int nrOfInputs, int nrOfGpos) { throw new NotImplementedException(); }

        public Task<bool> SetGpoAsync(string ip, int gpo, bool active) { throw new NotImplementedException(); }

        public Task<bool> SetInputEnabledAsync(string ip, int input, bool enabled) { throw new NotImplementedException(); }

        public Task<bool> SetInputGainLevelAsync(string ip, int input, int gainLevel) { throw new NotImplementedException(); }

        public Task<bool> LoadPresetAsync(string ip, string presetName) { throw new NotImplementedException(); }

        public Task<bool> RebootAsync(string ip) { throw new NotImplementedException(); }

        public Task<bool> CallAsync(string hostAddress, string callee, string profileName)
        {
            log.Debug("Call from Umac codec at {0}", hostAddress);
            
            if (profileName != "Telefon")
            {
                // We can't deal with anything but the phone profile for now
                return Task.FromResult(false);
            }
            
            try
            {
                using (var client = new UmacClient(hostAddress, Sdk.Umac.ExternalProtocolIpCommandsPort))
                {
                    var lineStatus = client.Call(callee, profileName);
                    return Task.FromResult(lineStatus);
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return Task.FromResult(false);
            }

        }

        public Task<bool> HangUpAsync(string hostAddress)
        {
            log.Debug("Hanging up Umac codec at {0}", hostAddress);
            
            /* Example dialog:
                        admin> hangup
                        admin> not streaming
                        CALL_STATE disconnected [tx: idle, rx: idle]
                        */

            try
            {
                using (var client = new UmacClient(hostAddress, Sdk.Umac.ExternalProtocolIpCommandsPort))
                {
                    var lineStatus = client.HangUp();
                    return Task.FromResult(lineStatus);
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return Task.FromResult(false);
            }
        }

    }
}
