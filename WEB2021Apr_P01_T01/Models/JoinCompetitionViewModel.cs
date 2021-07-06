using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class JoinCompetitionViewModel
    {
        [Required]
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Required]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Required]
        [Display(Name = "Competition's Name")]
        public string CompetitionName { get; set; }

        [Required]
        [Display(Name = "Competitor's Name")]
        public string CompetitorName { get; set; }

        [Display(Name = "File Submitted")]
        public string FileUrl { get; set; }
    }
}
