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
        [Required(ErrorMessage = "This field is required.")]
        public string firstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string lastName { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required(ErrorMessage = "This field is required.")]
        public string emailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "This field is required.")]
        [Compare("password")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }

        public string? salutation { get; set; }
        public string userType { get; set; }
        public int AreaInterestId { get; set; }
    }
}
