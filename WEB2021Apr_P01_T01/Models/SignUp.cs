using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class SignUp
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a first name")]
        public string firstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name")]
        public string lastName { get; set; }
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")] // <- [EmailAddress]
        [Required(ErrorMessage = "Please enter a valid email address")]
        [ValidateEmailAddress]
        public string emailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a last name")]
        [DataType(DataType.Password)]
        public string password { get; set; }

    }
}
