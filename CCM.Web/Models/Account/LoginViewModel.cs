using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Display(Name = "Remember_Me", ResourceType = typeof(Resources))]
        public bool RememberMe { get; set; }

    }
}