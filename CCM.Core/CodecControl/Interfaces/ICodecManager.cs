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

namespace CCM.Core.CodecControl.Interfaces
{
    public interface ICodecManager
    {
        Task<bool> CallAsync(CodecInformation codecInformation, string callee, string profileName);
        Task<bool> HangUpAsync(CodecInformation codecInformation);
        Task<bool> CheckIfAvailableAsync(CodecInformation codecInformation);
        Task<bool?> GetGpoAsync(CodecInformation codecInformation, int gpio);
        Task<bool> SetGpoAsync(CodecInformation codecInformation, int gpo, bool active);

        // GetInputEnabled, SetInputEnabled, GetInputGainLevel och SetInputGainLevel doesn't work on Quantum ST since it lacks controlable inputs.
        Task<bool> GetInputEnabledAsync(CodecInformation codecInformation, int input);
        Task<bool> SetInputEnabledAsync(CodecInformation codecInformation, int input, bool enabled);
        Task<int> GetInputGainLevelAsync(CodecInformation codecInformation, int input);
        Task<int> SetInputGainLevelAsync(CodecInformation codecInformation, int input, int gainLevel);
        Task<LineStatus> GetLineStatusAsync(CodecInformation codecInformation, int line);
        Task<string> GetLoadedPresetNameAsync(CodecInformation codecInformation, string lastPresetName);
        Task<VuValues> GetVuValuesAsync(CodecInformation codecInformation);
        Task<AudioStatus> GetAudioStatusAsync(CodecInformation codecInformation, int nrOfInputs, int nrOfGpos);
        Task<AudioMode> GetAudioModeAsync(CodecInformation codecInformation);
        Task<bool> LoadPresetAsync(CodecInformation codecInformation, string preset);
        Task<bool> RebootAsync(CodecInformation codecInformation);
    }
}
