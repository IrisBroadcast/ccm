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