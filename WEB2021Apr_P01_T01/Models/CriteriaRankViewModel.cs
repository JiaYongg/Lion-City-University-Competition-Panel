using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class CriteriaRankViewModel
    {
        [Display(Name = "Criteria")]
        public List<int> CriteriaId { get; set; }

        public int CompetitorId { get; set; }

        [Required]
        [Display(Name = "Score")]
        [Range(0,10,ErrorMessage = "Please enter a number from 0 to 10")]
        public List<int>? Score { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }
        public double? TotalMarks { get; set; }
    }
}
