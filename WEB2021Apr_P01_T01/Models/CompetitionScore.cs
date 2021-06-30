using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class CompetitionScore
    {
        [Required]
        public int CriteriaID { get; set; }

        [Required]
        public int CompetitorID { get; set; }

        [Required]
        public int CompetitionID { get; set; }

        [Required]
        [Display(Name = "Score")]
        public int Score { get; set; }
    }
}
