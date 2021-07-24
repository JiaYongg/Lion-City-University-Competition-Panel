﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class CriteriaRankViewModel
    {
        public List<int> CriteriaId { get; set; }

        public int CompetitorId { get; set; }

        [Required]
        [Display(Name = "Score")]
        public List<int>? Score { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }
        public double? TotalMarks { get; set; }
    }
}
