using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class Judge
    {
        [Required]
        [Display(Name = "ID")]
        public int JudgeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(5)]
        public string? Salutation { get; set; }

        public int AreaInterestId { get; set; }

        public string? AreaInterestName { get; set; }

        public List<Competition> competitionAssigned { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddr { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
