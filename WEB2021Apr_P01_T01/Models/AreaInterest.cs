using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class AreaInterest
    {
        [Required]
        [Display(Name = "ID")]
        public int AreaInterestId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
