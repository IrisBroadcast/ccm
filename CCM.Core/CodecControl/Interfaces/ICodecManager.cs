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