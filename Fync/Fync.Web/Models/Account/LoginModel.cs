﻿using System.ComponentModel.DataAnnotations;

namespace Fync.Web.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
