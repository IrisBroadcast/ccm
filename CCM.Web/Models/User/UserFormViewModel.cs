using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Entities;

namespace CCM.Web.Models.User
{
    public class UserFormViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "First_Name")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Last_Name")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_To_Short")]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_Dont_Match")]
        [Display(ResourceType = typeof(Resources), Name = "Confirm_Password")]
        public string PasswordConfirm { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Role")]
        public string RoleId { get; set; }

        public List<CcmRole> Roles { get; set; }
    }
}