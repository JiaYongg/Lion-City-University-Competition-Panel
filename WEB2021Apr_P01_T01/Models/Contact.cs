using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class Contact
    {
        [Display(Name = "Name: ")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Display(Name = "Email Address: ")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter your email")]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[89]\d{7}$", ErrorMessage = "Invalid phone number. Phone number must start with 8 or 9")]
        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "Message")]
        [MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }
    }
}
