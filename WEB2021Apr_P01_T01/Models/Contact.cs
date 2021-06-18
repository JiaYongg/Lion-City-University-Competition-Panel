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
        [Required(ErrorMessage = "Name Required!")]
        public string Name { get; set; }

        [Display(Name = "Email Address: ")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Required(ErrorMessage = "Email Required!")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string PhoneNo { get; set; }
    }
}
