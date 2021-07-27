using Microsoft.AspNetCore.Http;
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

        [Display(Name = "Competitor ID")]
        public int CompetitorId { get; set; }

        [Required]
        [Display(Name = "by ")]
        public string? CompetitorName { get; set; }

        [Display(Name = "Competition:")]
        public string CompetitionName { get; set; }

        [Display(Name = "Competition Start Date: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Competition End Date: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Results Released: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ResultsReleaseDate { get; set; }

        [Required]
        [Display(Name = "Appeal Remarks")]
        public string? Appeal { get; set; }

        [Required]
        [Display(Name = "votes")]
        public int VoteCount { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }

        [Display(Name = "judges")]
        public int? numOfJudge { get; set; }

        [Display(Name = "days left to the start of the competition")]
        public int? durationLeftToStart { get; set; }

        [Display(Name = "days left to submit a file")]
        public int? durationLeftToSubmit { get; set; }

        [Display(Name = "Results released in ")]
        public int? resultsReleaseDuration { get; set; }

        [Display(Name = "File Submitted")]
        public string? FileUrl { get; set; }

        [Required(ErrorMessage = "Please choose a file.")]
        public IFormFile? fileToUpload { get; set; }

        [Display(Name = "Submitted On:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? FileUploadDateTime { get; set; }
        public string DateImage { get; set; }

        public string Status { get; set; }

        public bool ResultRelease { get; set; }

        [Required]
        [Display(Name = "Score")]
        public List<double>? Score { get; set; }

        public List<string> CriteriaName { get; set; }

        public List<int> Weightage { get; set; }
    }
}
