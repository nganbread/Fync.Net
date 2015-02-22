using System.ComponentModel.DataAnnotations;

namespace Fync.Web.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
