using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class Login
    {
        [Display(Name = "Email Address")]
        [EmailAddress]
        [ValidateEmailAddress]
        [Required]

        public string Email { get; set; }

        [Display(Name = "Password")]
        []
    }
}
