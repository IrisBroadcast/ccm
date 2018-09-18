using CCM.Core.CodecControl.Enums;
using CCM.Core.Helpers;
using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class AudioModeViewModel : CodecViewModelBase
    {
        public AudioAlgorithm EncoderAudioMode { get; set; }
        public AudioAlgorithm DecoderAudioMode { get; set; }

        public string EncoderAudioModeString => EncoderAudioMode.Description();
        public string DecoderAudioModeString => DecoderAudioMode.Description();
    }
}