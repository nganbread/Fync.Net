using System.ComponentModel.DataAnnotations;

namespace Fync.Web.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}