using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.ApiExternal
{
    public class UserModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string DisplayName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Comment { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [Required]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_To_Short")]
        public string NewPassword { get; set; }
    }

    public class AddUserModel : UserModel
    {
        [Required]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Password_To_Short")]
        public string Password { get; set; }
    }

}