using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Enums;

namespace CCM.Web.Models.SipAccount
{
    public class SipAccountFormViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "DisplayName")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ExtensionNumber")]
        public string ExtensionNumber { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Account_Locked")]
        public bool AccountLocked { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Owner")]
        public Guid OwnerId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Codec_Type")]
        public Guid CodecTypeId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Account_Type")]
        public SipAccountType AccountType { get; set; }

        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_To_Short")]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_Dont_Match")]
        [Display(ResourceType = typeof(Resources), Name = "Confirm_Password")]
        public string PasswordConfirm { get; set; }

      

        public List<Owner> Owners { get; set; }
        public List<CodecType> CodecTypes { get; set; }

        public List<SelectListItem> AccountTypes { get; set; }

    }
}