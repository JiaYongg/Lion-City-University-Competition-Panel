using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class Competitor
    {
        [Required]
        [Display(Name = "ID")]
        public int CompetitorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(5)]
        public string? Salutation { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddr { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
