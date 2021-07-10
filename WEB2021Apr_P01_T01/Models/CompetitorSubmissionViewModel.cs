using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P01_T01.Models
{
    public class CompetitorSubmissionViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionId { get; set; }

        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }

        [Display(Name = "Competition Start Date: ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Competition End Date: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Results will be released on ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ResultsReleaseDate { get; set; }

        [Display(Name = "Appeal Remarks")]
        public string? Appeal { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }

        [Display(Name = "judges")]
        public int? numOfJudge { get; set; }

        [Display(Name = "days left to the start of the competition")]
        public int? durationLeft { get; set; }

        [Display(Name = "Results released in ")]
        public int? resultsReleaseDuration { get; set; }

        [Display(Name = "File Submitted")]
        public string? FileUrl { get; set; }

        public string DateImage { get; set; }

        public string Status { get; set; }

        public bool ResultRelease { get; set; }
    }
}
