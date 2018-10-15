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
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using NLog;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using CCM.CodecControl.SR.BaresipRest;
using CCM.Core.Helpers;

namespace CCM.CodecControl.SR.BaresipRest
{
    /// <summary>
    /// Codec control implementation for the Baresip, this implementation is experimental/proprietary
    /// </summary>
    public class BaresipRestApi : ICodecApi
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();


        public async Task<bool> CheckIfAvailableAsync(string ip)
        {
            // Connect to the unit and check for response on API:port
            var url = $"http://{ip}:{Sdk.Baresip.ExternalProtocolIpCommandsPort}/api/isavailable";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> CallAsync(string ip, string callee, string profileName)
        {
            var url = CreateUrl(ip, "api/call");
            var response = await HttpService.PostJsonAsync(url, new { address = callee });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> HangUpAsync(string ip)
        {
            var url = CreateUrl(ip, "api/hangup");
            var response = await HttpService.PostJsonAsync(url);
            return response.IsSuccessStatusCode;
        }

        public Task<bool?> GetGpoAsync(string ip, int gpio) { throw new NotImplementedException(); }

        public async Task<bool> GetInputEnabledAsync(string ip, int input)
        {
            var url = CreateUrl(ip, "api/inputenable?input=" + (input + 1) );
            var gainObject = await HttpService.GetAsync<InputEnableResponse>(url);
            return gainObject?.Value ?? false;
        }

        public async Task<int> GetInputGainLevelAsync(string ip, int input)
        {
            var url = CreateUrl(ip, "api/inputgain?input=" + (input + 1));
            var gainObject = await HttpService.GetAsync<InputGainResponse>(url);
            return gainObject?.Value ?? 0;
        }

        public async Task<LineStatus> GetLineStatusAsync(string ip, int line)
        {
            var url = CreateUrl(ip, "api/linestatus");
            var lineStatus = await HttpService.GetAsync<BaresipLineStatus>(url);

            return new LineStatus
            {
                RemoteAddress = "",
                DisconnectReason = MapToDisconnectReason(lineStatus.Call.Code),
                StatusCode = MapToLineStatusCode(lineStatus.State)
            };
        }

        private DisconnectReason MapToDisconnectReason(int statusCode)
        {
            if (statusCode == 0)
            {
                return DisconnectReason.SipOk;
            }

            if (Enum.TryParse(statusCode.ToString(), out DisconnectReason disconnectReason))
            {
                return disconnectReason;
            }

            return DisconnectReason.None;
        }

        private LineStatusCode MapToLineStatusCode(BaresipState baresipState)
        {
            switch (baresipState)
            {
                case BaresipState.Idle:
                    return LineStatusCode.Disconnected;
                case BaresipState.Calling:
                    return LineStatusCode.Calling;
                case BaresipState.ReceivingCall:
                    return LineStatusCode.ReceivingCall;
                case BaresipState.ConnectedReceived:
                    return LineStatusCode.ConnectedReceived;
                case BaresipState.ConnectedCalled:
                    return LineStatusCode.ConnectedCalled;
                case BaresipState.Disconnecting:
                    return LineStatusCode.Disconnecting;
                default:
                    return LineStatusCode.Disconnected;
            }
        }


        public Task<string> GetLoadedPresetNameAsync(string ip, string lastPresetName) { throw new NotImplementedException(); }

        public Task<VuValues> GetVuValuesAsync(string ip) { throw new NotImplementedException(); }

        public Task<AudioMode> GetAudioModeAsync(string ip) { throw new NotImplementedException(); } // Encoder / Decoder


        public async Task<AudioStatus> GetAudioStatusAsync(string ip, int nrOfInputs, int nrOfGpos)
        {
            return await GetAudioStatusAsync(ip);
        }

        private async Task<AudioStatus> GetAudioStatusAsync(string ip)
        {
            var url = CreateUrl(ip, "api/audiostatus");
            var bareSipAudioStatus = await HttpService.GetAsync<BaresipAudioStatus>(url);

            try
            {
                // Convert to AudioStatus
                var audioStatus = new AudioStatus()
                {
                    Gpos = bareSipAudioStatus.Control.Gpo.Select(gpo => gpo.Active).ToList(),
                    InputStatuses = bareSipAudioStatus.Inputs.Select(i => new InputStatus()
                    {
                        Enabled = i.On,
                        GainLevel = i.Level
                    }).ToList(),
                    VuValues = new VuValues()
                    {
                        RxLeft = bareSipAudioStatus.Meters.Rx.L,
                        RxRight = bareSipAudioStatus.Meters.Rx.R,
                        TxLeft = bareSipAudioStatus.Meters.Tx.L,
                        TxRight = bareSipAudioStatus.Meters.Tx.R
                    }
                };
                return audioStatus;
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when converting audio status");
                return null;
            }
        }

        public Task<bool> SetGpoAsync(string ip, int gpo, bool active) { throw new NotImplementedException(); }

        public async Task<bool> SetInputEnabledAsync(string ip, int input, bool enabled)
        {
            var url = CreateUrl(ip, "api/inputenable");
            var response = await HttpService.PostJsonAsync(url, new { input = input, value = enabled });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetInputGainLevelAsync(string ip, int input, int gainLevel)
        {
            var url = CreateUrl(ip, "api/inputgain");
            var response = await HttpService.PostJsonAsync(url, new { input = input, value = gainLevel});
            return response.IsSuccessStatusCode;
        }

        public Task<bool> LoadPresetAsync(string ip, string presetName) { throw new NotImplementedException(); }

        public Task<bool> RebootAsync(string ip) { throw new NotImplementedException(); }
        
        private Uri CreateUrl(string ip, string path)
        {
            var baseUrl = new Uri($"http://{ip}:{Sdk.Baresip.ExternalProtocolIpCommandsPort}");
            return new Uri(baseUrl, path);
        }
    }

    public abstract class BaresipResponse
    {
        public bool Success { get; set; }
    }

    public class InputGainResponse : BaresipResponse
{
        public int Value { get; set; }
    }

    public class InputEnableResponse : BaresipResponse
    {
        public bool Value { get; set; }
    }

}
