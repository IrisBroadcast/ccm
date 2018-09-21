using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Enums;

namespace CCM.Web.Models.UserAgents
{
    public class UserAgentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Name_Required")]
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Name_Required")]
        [Display(ResourceType = typeof(Resources), Name = "Identifier")]
        public string Identifier { get; set; } // Match identifier

        [Display(ResourceType = typeof(Resources), Name = "Image")]
        public string Image { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "User_Interface", Description = "User_Interface_Description")]
        public string UserInterfaceLink { get; set; }

        public bool ActiveX { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Width")]
        public int Width { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Height")]
        public int Height { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "IdentifyType")]
        public MatchType MatchType { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Profiles")]
        public List<ProfileListItemViewModel> Profiles { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Api")]
        public string Api { get; set; }

        public Dictionary<string, string> CodecApis { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Lines")]
        public int Lines { get; set; }

        public int LinesInList { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Inputs")]
        public int Inputs { get; set; }
        public int GposInList { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NrOfGpos")]
        public int NrOfGpos { get; set; }

        public int InputsInList { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Min")]
        public int MinInputDb { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Max")]
        public int MaxInputDb { get; set; }

        public int InputDbInListMin { get; set; }
        public int InputDbInListMax { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Step")]
        public int InputGainStep { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Gpo_Names")]
        public string GpoNames { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Open_User_Interface", Description = "Open_User_Interface_Description")]
        public bool UserInterfaceIsOpen { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Use_Scrollbars")]
        public bool UseScrollbars { get; set; }

        public List<CodecPresetListItemViewModel> CodecPresets { get; set; }
    }
}