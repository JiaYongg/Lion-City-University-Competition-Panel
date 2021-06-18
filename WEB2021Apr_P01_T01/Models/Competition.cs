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
        [Display(Name = "Area of Interest")]
        public int Aoi { get; set; }
        //public AreaInterest Aoi { get; set; }

        [Required]
        [Display(Name = "Competition Name")]
        [StringLength(100, MinimumLength = 3)]
        public string CompetitionName { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Results Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ResultsReleaseDate { get; set; }

        public List<CompetitionSubmission> SubmissionList { get; set; }
    }
}
