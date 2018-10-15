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

using System.Threading.Tasks;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;

namespace CCM.CodecControl
{
    /// <remarks>
    /// Gpi = General Purpose Input
    /// Gpo = General Purpose Output
    /// VuValues = Sound Level Values
    /// </remarks>
    public interface ICodecApi
    {
        Task<bool> CallAsync(string hostAddress, string callee, string profileName);
        Task<bool> HangUpAsync(string hostAddress);
        Task<bool> CheckIfAvailableAsync(string ip);
        
        Task<bool?> GetGpoAsync(string ipp, int gpio);
        Task<bool> GetInputEnabledAsync(string ip, int input);
        Task<int> GetInputGainLevelAsync(string ip, int input);
        Task<LineStatus> GetLineStatusAsync(string ip, int line);
        Task<string> GetLoadedPresetNameAsync(string ip, string lastPresetName);
        Task<VuValues> GetVuValuesAsync(string ip);
        Task<AudioMode> GetAudioModeAsync(string ip);
        Task<AudioStatus> GetAudioStatusAsync(string hostAddress, int nrOfInputs, int nrOfGpos);

        Task<bool> SetGpoAsync(string ip, int gpo, bool active);
        Task<bool> SetInputEnabledAsync(string ip, int input, bool enabled);
        Task<bool> SetInputGainLevelAsync(string ip, int input, int gainLevel);
        
        Task<bool> LoadPresetAsync(string ip, string presetName);
        Task<bool> RebootAsync(string ip);
    }
}
