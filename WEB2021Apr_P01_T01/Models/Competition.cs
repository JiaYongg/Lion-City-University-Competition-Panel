using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class Competition
    {
        [Display(Name = "Competition ID")]
        public int CompetitionId { get; set; }
        
        [Required]
        [Display(Name = "Interest ID")]
        public int AoiId { get; set; }
        //public AreaInterest Aoi { get; set; }

        [Display(Name = "Area of Interest")]
        public string? AoiName { get; set; }

        [Required]
        [Display(Name = "Competition Name")]
        [StringLength(100, MinimumLength = 3)]
        public string CompetitionName { get; set; }

        [Required]
        [Display(Name = "Competition Start Date: ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Competition End Date: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Results will be released on ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ResultsReleaseDate { get; set; }

        public List<CompetitionSubmission> SubmissionList { get; set; }
    }
}
