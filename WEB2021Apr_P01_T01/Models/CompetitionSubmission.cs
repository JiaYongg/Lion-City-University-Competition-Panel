using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class CompetitionSubmission
    {
        [Required]
        [Display(Name = "Competition ID")]
        public int CompetitionId { get; set; }
        
        [Required]
        [Display(Name= "Competitor ID")]
        public int CompetitorId { get; set; }
        
        [Required]
        [Display(Name = "Competitor's Name")]
        public string? CompetitorName { get; set; }

        [Required]
        [Display(Name = "Competitor's Name")]
        public string? CompetitionName { get; set; }

        [Required]
        [Display(Name = "Number of Judge")]
        public int? numofJudge { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ResultReleasedDate { get; set; }

        [Display(Name = "File Submitted")]
        public string? FileUrl { get; set; }
        
        [Display(Name = "File Submitted On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? FileUploadDateTime { get; set; }

        [Display(Name = "Appeal Remarks")]
        public string? Appeal { get; set; }

        [Required]
        [Display(Name = "Number of Votes")]
        public int VoteCount { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }

        public double? TotalMarks { get; set; }
    }
}
