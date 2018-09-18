using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "SDP")]
        public string Sdp { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Locations")]
        public List<ListItemViewModel> Locations { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserAgents")]
        public List<ListItemViewModel> UserAgents { get; set; }
    }
}