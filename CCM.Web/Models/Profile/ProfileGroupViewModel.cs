using CCM.Web.Models.UserAgents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Profile
{
    public class ProfileGroupViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Namnet är för långt")]
        [Display(Name = "Gruppnamn")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Profiles")]
        public List<ProfileListItemViewModel> Profiles { get; set; }


        public ProfileGroupViewModel()
        {
            Profiles = new List<ProfileListItemViewModel>();
        }
    }
}