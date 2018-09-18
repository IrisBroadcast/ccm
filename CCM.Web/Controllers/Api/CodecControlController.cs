using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories.Specialized;
using CCM.Web.Authentication;
using CCM.Web.Models.CodecControl;
using CCM.Web.Models.CodecControl.Base;
using NLog;

namespace CCM.Web.Controllers.Api
{
    public class CodecControlController : ApiController
    {
        #region Constructor and members
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ICodecManager _codecManager;
        private readonly ICodecInformationRepository _codecInformationRepository;

        public CodecControlController(ICodecManager codecManager, ICodecInformationRepository simpleRegisteredSipRepository)
        {
            _codecManager = codecManager;
            _codecInformationRepository = simpleRegisteredSipRepository;
        }
        #endregion

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("CheckCodecAvailable")]
        [HttpGet]
        public async Task<bool> CheckCodecAvailable(Guid id)
        {
            var codecInformation = GetCodecInformationById(id);
            return await _codecManager.CheckIfAvailableAsync(codecInformation);
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetAvailableGpos")]
        [HttpPost]
        public async Task<AvailableGposViewModel> GetAvailableGpos(dynamic data)
        {
            Guid id = data.id;
            int nrOfGpos = data.nrOfGpos ?? 10;

            var model = new AvailableGposViewModel();

            if (id == Guid.Empty)
            {
                model.Error = "Missing parameter";
                return model;
            }

            CodecInformation codecInformation = GetCodecInformationById(id);

            if (codecInformation == null)
            {
                model.Error = Resources.No_Codec_Found;
                return model;
            }

            try
            {
                for (int i = 0; i < nrOfGpos; i++)
                {
                    bool? active = await _codecManager.GetGpoAsync(codecInformation, i);

                    if (!active.HasValue)
                    {
                        // GPO missing. Expected that we passed the last GPO
                        break;
                    }

                    var gpoName = GetGpoName(codecInformation.GpoNames, i);

                    model.Gpos.Add(new GpoViewModel()
                    {
                        Active = active.Value,
                        Name = string.IsNullOrWhiteSpace(gpoName) ? $"GPO {i}" : gpoName,
                        Number = i
                    });
                }
            }
            catch (Exception)
            {
                if (model.Gpos.Count == 0)
                {
                    model.Error = Resources.No_Gpo_Found;
                }
            }

            return model;
        }

        public string GetGpoName(string gpoNames, int index)
        {
            var names = (gpoNames ?? string.Empty).Split(',');
            return index < names.Length ? names[index].Trim() : string.Empty;
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetAudioStatus")]
        [HttpGet]
        public async Task<Models.CodecControl.AudioStatusViewModel> GetAudioStatus([FromUri]Guid id, [FromUri]int nrOfInputs = 2, [FromUri]int nrOfGpos = 2)
        {
            CodecInformation codecInformation = GetCodecInformationById(id);
            var audioStatus = await _codecManager.GetAudioStatusAsync(codecInformation, nrOfInputs, nrOfGpos);
            var model = Mapper.Map<Models.CodecControl.AudioStatusViewModel>(audioStatus);
            return model;
        }
        
        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetInputGainAndStatus")]
        [HttpGet]
        public async Task<InputGainAndStatusViewModel> GetInputGainAndStatus([FromUri]Guid id, [FromUri]int input)
        {
            CodecInformation codecInformation = GetCodecInformationById(id);
            if (codecInformation == null)
            {
                return new InputGainAndStatusViewModel {Error = Resources.No_Codec_Found};
            }

            try
            {
                var model = new InputGainAndStatusViewModel
                {
                    Enabled = await _codecManager.GetInputEnabledAsync(codecInformation, input),
                    GainLevel = await _codecManager.GetInputGainLevelAsync(codecInformation, input)
                };
                return model;
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new InputGainAndStatusViewModel { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetInputStatus")]
        [HttpGet]
        public async Task<InputStatusViewModel> GetInputStatus(Guid id, int input)
        {
            CodecInformation codecInformation = GetCodecInformationById(id);
            if (codecInformation == null)
            {
                return new InputStatusViewModel() { Error = Resources.No_Codec_Found };
            }

            try
            {
                var enabled = await _codecManager.GetInputEnabledAsync(codecInformation, input);
                return new InputStatusViewModel { Enabled = enabled };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new InputStatusViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetLineStatusBySipAddress")]
        [HttpGet]
        public async Task<LineStatusViewModel> GetLineStatusBySipAddress(string sipAddress, int line)
        {
            CodecInformation codecInformation = GetCodecInformationBySipAddress(sipAddress);
            return await GetLineStatus(line, codecInformation);
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetLineStatus")]
        [HttpGet]
        public async Task<LineStatusViewModel> GetLineStatus(Guid id, int line)
        {
            CodecInformation codecInformation = GetCodecInformationById(id);
            return await GetLineStatus(line, codecInformation);
        }

        private async Task<LineStatusViewModel> GetLineStatus(int line, CodecInformation codecInformation)
        {
            if (codecInformation == null)
            {
                return new LineStatusViewModel { Error = Resources.No_Codec_Found };
            }

            try
            {
                var model = new LineStatusViewModel();
                LineStatus lineStatus = await _codecManager.GetLineStatusAsync(codecInformation, line);

                if (lineStatus == null || lineStatus.StatusCode == LineStatusCode.ErrorGettingStatus)
                {
                    model.Error = Resources.ResourceManager.GetString(LineStatusCode.ErrorGettingStatus.ToString());
                }
                else
                {
                    model.Status = Resources.ResourceManager.GetString($"LineStatus_{lineStatus.StatusCode}");
                    model.DisconnectReason = lineStatus.DisconnectReason;
                    model.RemoteAddress = lineStatus.RemoteAddress;
                    model.LineStatus = lineStatus.StatusCode;
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new LineStatusViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }
        
        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetLoadedPreset")]
        [HttpPost]
        public async Task<PresetViewModel> GetLoadedPreset(dynamic data)
        {
            Guid id = data.id;
            CodecInformation codecInformation = GetCodecInformationById(id);
            if (codecInformation == null)
            {
                return new PresetViewModel { Error = Resources.No_Codec_Found };
            }

            try
            {
                var loadedPreset = await _codecManager.GetLoadedPresetNameAsync(codecInformation, string.Empty);
                return new PresetViewModel { LoadedPreset = loadedPreset };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new PresetViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetVuValues")]
        [HttpGet]
        public async Task<VuValuesViewModel> GetVuValues(Guid id)
        {
            CodecInformation codecInformation = GetCodecInformationById(id);
            if (codecInformation == null)
            {
                return new VuValuesViewModel { Error = Resources.No_Codec_Found };
            }

            try
            {
                var vuValues = await _codecManager.GetVuValuesAsync(codecInformation);

                return new VuValuesViewModel
                {
                    RxLeft = vuValues.RxLeft,
                    RxRight = vuValues.RxRight,
                    TxLeft = vuValues.TxLeft,
                    TxRight = vuValues.TxRight
                };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new VuValuesViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetAudioMode")]
        [HttpGet]
        public async Task<AudioModeViewModel> GetAudioMode(Guid id)
        {
            return await GetAudioMode(GetCodecInformationById(id));
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("GetAudioMode")]
        [HttpGet]
        public async Task<AudioModeViewModel> GetAudioMode(string sipAddress)
        {
            return await GetAudioMode(GetCodecInformationBySipAddress(sipAddress));
        }

        private async Task<AudioModeViewModel> GetAudioMode(CodecInformation codecInformation)
        {
            if (codecInformation == null)
            {
                return new AudioModeViewModel { Error = Resources.No_Codec_Found };
            }

            try
            {
                AudioMode result = await _codecManager.GetAudioModeAsync(codecInformation);
                return new AudioModeViewModel
                {
                    EncoderAudioMode = result.EncoderAudioAlgoritm,
                    DecoderAudioMode = result.DecoderAudioAlgoritm
                };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new AudioModeViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
           
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("LoadPreset")]
        [HttpPost]
        public async Task<CodecViewModelBase> LoadPreset(LoadPresetParameters parameters)
        {
            CodecInformation codecInformation = GetCodecInformationById(parameters.Id);
            if (codecInformation == null)
            {
                return new CodecViewModelBase() { Error = Resources.No_Codec_Found };
            }

            try
            {
                await _codecManager.LoadPresetAsync(codecInformation, parameters.Name);
                return new CodecViewModelBase();
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new CodecViewModelBase() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("SetGpo")]
        [HttpGet]
        public async Task<GpoViewModel> SetGpo(string sipAddress, int number, bool active)
        {
            return await SetGpoAsync(GetCodecInformationBySipAddress(sipAddress), number, active);
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("SetGpo")]
        [HttpGet]
        public async Task<GpoViewModel> SetGpo(Guid id, int number, bool active)
        {
            return await SetGpoAsync(GetCodecInformationById(id), number, active);
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("SetInputEnabled")]
        [HttpPost]
        public async Task<InputStatusViewModel> SetInputEnabled(SetInputEnabledParameters parameters)
        {
            // Guid id, int input, bool enabled
            CodecInformation codecInformation = GetCodecInformationById(parameters.Id);
            if (codecInformation == null)
            {
                return new InputStatusViewModel() { Error = Resources.No_Codec_Found };
            }

            try
            {
                var enabled = await _codecManager.SetInputEnabledAsync(codecInformation, parameters.Input, parameters.Enabled);
                return new InputStatusViewModel { Enabled = enabled };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new InputStatusViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [System.Web.Mvc.HttpPost]
        [ActionName("SetInputGainLevel")]
        [HttpPost]
        public async Task<InputGainLevelViewModel> SetInputGainLevelAsync(dynamic data)
        {
            Guid id = data.id;
            int input = data.input;
            int level = data.level;

            CodecInformation codecInformation = GetCodecInformationById(id);
            if (codecInformation == null)
            {
                return new InputGainLevelViewModel() { Error = Resources.No_Codec_Found };
            }
            
            try
            {
                var gainLevel = await _codecManager.SetInputGainLevelAsync(codecInformation, input, level);
                return new InputGainLevelViewModel { GainLevel = gainLevel };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new InputGainLevelViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }

        [CcmAuthorize(Roles = Roles.Admin)]
        [ActionName("RebootCodec")]
        [HttpPost]
        public async Task<bool> RebootCodec(RebootCodecParameters parameters)
        {
            var codecInformation = GetCodecInformationById(parameters.Id);

            if (codecInformation == null)
            {
                return false;
            }

            return await _codecManager.RebootAsync(codecInformation);
        }

        [System.Web.Mvc.HttpPost]
        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("Call")]
        [HttpGet]
        public async Task<bool> Call(Guid id, string callee, string profileName)
        {
            var codecInformation = GetCodecInformationById(id);

            if (codecInformation == null)
            {
                return false;
            }

            return await _codecManager.CallAsync(codecInformation, callee, profileName);
        }

        [System.Web.Mvc.HttpPost]
        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("Hangup")]
        [HttpGet]
        public async Task<bool> Hangup(Guid id)
        {
            var codecInformation = GetCodecInformationById(id);

            if (codecInformation == null)
            {
                return false;
            }

            return await _codecManager.HangUpAsync(codecInformation);
        }

        [System.Web.Mvc.HttpPost]
        [CcmAuthorize(Roles = "Admin, Remote")]
        [ActionName("Hangup")]
        [HttpGet]
        public async Task<bool> Hangup(string sipAddress)
        {
            var codecInformation = GetCodecInformationBySipAddress(sipAddress);

            if (codecInformation == null)
            {
                return false;
            }

            return await _codecManager.HangUpAsync(codecInformation);
        }

        private async Task<GpoViewModel> SetGpoAsync(CodecInformation codecInformation, int number, bool active)
        {
            if (codecInformation == null)
            {
                return new GpoViewModel() { Error = Resources.No_Codec_Found };
            }
            try
            {
                await _codecManager.SetGpoAsync(codecInformation, number, active);
                var gpoActive = await _codecManager.GetGpoAsync(codecInformation, number) ?? false;
                return new GpoViewModel { Number = number, Active = gpoActive };
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when sending codec control command to " + codecInformation.SipAddress);
                return new GpoViewModel() { Error = Resources.Unable_To_Connect_To_Codec };
            }
        }
        
        private CodecInformation GetCodecInformationBySipAddress(string sipAddress)
        {
            var codecInfo = _codecInformationRepository.GetCodecInformationBySipAddress(sipAddress);
            return string.IsNullOrEmpty(codecInfo?.Api) ? null : codecInfo;
        }

        private CodecInformation GetCodecInformationById(Guid id)
        {
            var codecInfo = _codecInformationRepository.GetCodecInformationById(id);
            return string.IsNullOrEmpty(codecInfo?.Api) ? null : codecInfo;
        }

    }
}