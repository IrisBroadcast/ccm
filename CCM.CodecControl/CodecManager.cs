using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCM.CodecControl.Mandozzi.Umac;
using CCM.CodecControl.Prodys.IkusNet;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Exceptions;
using CCM.Core.Interfaces.Managers;
using NLog;
using CCM.CodecControl.SR.BaresipRest;

namespace CCM.CodecControl
{
    /// <summary>
    /// Manager for connecting with Code APIs
    /// </summary>
    public class CodecManager : ICodecManager
    {
        private readonly ISettingsManager _settingsManager;
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public CodecManager(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public static Dictionary<string, CodecApiInformation> AvailableApis
        {
            get
            {
                var apis = new List<CodecApiInformation>
                {
                    new CodecApiInformation { DisplayName = "Prodys IkusNet", Name = "IkusNet" },
                    new CodecApiInformation { DisplayName = "Mandozzi Umac", Name = "Umac" },
                    new CodecApiInformation { DisplayName = "Baresip Proprietary", Name = "BaresipRest" }
                };
                return apis.ToDictionary(c => c.Name);
            }
        }

        private ICodecApi CreateCodecApi(CodecInformation codecInformation)
        {
            // Pga en bugg in Prodys Quantum, där den kan hänga sig då man skickar kommandon, 
            // införs möjligheten att helt kunna stänga av kodarstyrningsfunktionen.
            var codecControlActive = _settingsManager.CodecControlActive;

            if (!codecControlActive)
            {
                throw new CodecApiNotFoundException("Codec control is disabled.");
            }

            if (codecInformation == null)
            {
                throw new CodecApiNotFoundException("Missing codec api information.");
            }

            switch (codecInformation.Api)
            {
                case "IkusNet":
                    return new IkusNetApi();
                case "Umac":
                    return new UmacApi();
                case "BaresipRest":
                    return new BaresipRestApi();
                default:
                    throw new CodecApiNotFoundException($"Could not load API {codecInformation.Api}.");
            }
        }


        public async Task<bool> CallAsync(CodecInformation codecInformation, string callee, string profileName)
        {
            // TODO: first check codec call status. Do not execute the call method if the codec is already in a call.
            // Some codecs will hangup the current call and dial up the new call without hesitation.

            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.CallAsync(codecInformation.Ip, callee, profileName);
        }

        public async Task<bool> HangUpAsync(CodecInformation codecInformation)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.HangUpAsync(codecInformation.Ip);
        }

        public async Task<bool> CheckIfAvailableAsync(CodecInformation codecInformation)
        {
            try
            {
                var codecApi = CreateCodecApi(codecInformation);
                return await codecApi.CheckIfAvailableAsync(codecInformation.Ip);
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception in CheckIfAvailableAsync");
                return false;
            }
        }

        public async Task<bool?> GetGpoAsync(CodecInformation codecInformation, int gpio)
        {
            try
            {
                var codecApi = CreateCodecApi(codecInformation);
                var gpo = await codecApi.GetGpoAsync(codecInformation.Ip, gpio);
                return gpo;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> GetInputEnabledAsync(CodecInformation codecInformation, int input)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetInputEnabledAsync(codecInformation.Ip, input);
        }

        public async Task<int> GetInputGainLevelAsync(CodecInformation codecInformation, int input)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetInputGainLevelAsync(codecInformation.Ip, input);
        }

        public async Task<LineStatus> GetLineStatusAsync(CodecInformation codecInformation, int line)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetLineStatusAsync(codecInformation.Ip, line);
        }

        public async Task<string> GetLoadedPresetNameAsync(CodecInformation codecInformation, string lastPresetName)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetLoadedPresetNameAsync(codecInformation.Ip, lastPresetName);
        }

        public async Task<VuValues> GetVuValuesAsync(CodecInformation codecInformation)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetVuValuesAsync(codecInformation.Ip);
        }

        public async Task<AudioStatus> GetAudioStatusAsync(CodecInformation codecInformation, int nrOfInputs, int nrOfGpos)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetAudioStatusAsync(codecInformation.Ip, nrOfInputs, nrOfGpos);
        }

        public async Task<AudioMode> GetAudioModeAsync(CodecInformation codecInformation)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.GetAudioModeAsync(codecInformation.Ip);
        }

        public async Task<bool> LoadPresetAsync(CodecInformation codecInformation, string preset)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.LoadPresetAsync(codecInformation.Ip, preset);
        }

        public async Task<bool> RebootAsync(CodecInformation codecInformation)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.RebootAsync(codecInformation.Ip);
        }

        public async Task<bool> SetGpoAsync(CodecInformation codecInformation, int gpo, bool active)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.SetGpoAsync(codecInformation.Ip, gpo, active);
        }

        public async Task<bool> SetInputEnabledAsync(CodecInformation codecInformation, int input, bool enabled)
        {
            var codecApi = CreateCodecApi(codecInformation);
            return await codecApi.SetInputEnabledAsync(codecInformation.Ip, input, enabled);
        }

        public async Task<int> SetInputGainLevelAsync(CodecInformation codecInformation, int input, int gainLevel)
        {
            var codecApi = CreateCodecApi(codecInformation);
            await codecApi.SetInputGainLevelAsync(codecInformation.Ip, input, gainLevel);
            return await codecApi.GetInputGainLevelAsync(codecInformation.Ip, input);
        }

    }
}