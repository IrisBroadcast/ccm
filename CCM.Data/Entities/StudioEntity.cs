using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Studios")]
    public class StudioEntity : EntityBase
    {
        public string Name { get; set; }
        public string CodecSipAddress { get; set; }
        public string CameraAddress { get; set; }
        public bool CameraActive { get; set; }
        public string CameraUsername { get; set; }
        public string CameraPassword { get; set; }
        public string CameraVideoUrl { get; set; }
        public string CameraImageUrl { get; set; }
        public string CameraPlayAudioUrl { get; set; }
        public string AudioClipNames { get; set; }
        public string InfoText { get; set; }
        public string MoreInfoUrl { get; set; }
        public int NrOfAudioInputs { get; set; }
        public string AudioInputNames { get; set; }
        public int AudioInputDefaultGain { get; set; }
        public int NrOfGpos { get; set; }
        public string GpoNames { get; set; }
        public int InactivityTimeout { get; set; }
    }
}