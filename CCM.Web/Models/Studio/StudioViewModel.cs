using System;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Studio
{
    public class StudioViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Namnet är för långt")]
        [Display(Name = "Studionamn")]
        public string Name { get; set; }

        [Display(Name="Ljudkodarens SIP-adress")]
        public string CodecSipAddress { get; set; }

        [Display(Name = "Kamerans adress eller IP-nummer")]
        public string CameraAddress { get; set; }

        [Display(Name = "Kamera aktiv", Description = "")]
        public bool CameraActive { get; set; }

        [Display(Name = "Användarnamn", Description = "")]
        public string CameraUsername { get; set; }

        [Display(Name = "Lösenord", Description = "")]
        public string CameraPassword { get; set; }
        [Display(Name="Sökväg video", Description = "")]
        public string CameraVideoUrl { get; set; }

        [Display(Name = "Sökväg stillbild", Description = "")]
        public string CameraImageUrl { get; set; }
        
        [Display(Name = "Sökväg ljuduppspelning")]
        public string CameraPlayAudioUrl { get; set; }

        [Display(Name = "Ljudklippens namn")]
        public string AudioClipNames { get; set; }

        [Display(Name = "Informationstext")]
        public string InfoText { get; set; }

        [Display(Name = "Sökväg till 'manual'-sidan")]
        public string MoreInfoUrl { get; set; }

        [Display(Name = "Antal ingångar")]
        public int NrOfAudioInputs { get; set; }

        [Display(Name = "Ingångarnas namn")]
        public string AudioInputNames { get; set; }

        [Display(Name = "Förvald ingångsnivå")]
        public string AudioInputDefaultGain { get; set; }

        [Display(Name = "Antal GPOs")]
        public int NrOfGpos { get; set; }

        [Display(Name = "GPO Namnlista")]
        public string GpoNames { get; set; }

        [Required]
        [Range(0, 60)]
        [Display(Name = "Antal minuter innan monitorsidan blir inaktiv")]
        public int InactivityTimeout { get; set; }
    }
}