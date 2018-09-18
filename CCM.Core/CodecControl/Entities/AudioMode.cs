using CCM.Core.CodecControl.Enums;

namespace CCM.Core.CodecControl.Entities
{
    public class AudioMode
    {
        public AudioAlgorithm EncoderAudioAlgoritm { get; set; }    
        public AudioAlgorithm DecoderAudioAlgoritm { get; set; }    
    }
}