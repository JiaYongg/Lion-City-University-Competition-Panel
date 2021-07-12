using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class CompetitionDetailsViewModel
    {
        public int CompetitionId { get; set; }

        [Display(Name = "Competition Name")]
        [StringLength(100, MinimumLength = 3)]
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

        [Display(Name = "Area of Interest")]
        public string? AoiName { get; set; }

        public List<Judge> JudgeList { get; set; }
        public List<Criteria> CriteriaList { get; set; }
        public List<CompetitionSubmission> SubmissionList { get; set; }

        [Display(Name = "Comment ID")]
        public int? CommentId { get; set; }

        [Required(ErrorMessage = "Please enter your comments")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Please enter a comment between 3 to 250 characters")]
        public string CommentDesc { get; set; }

        public List<Comments> CommentsList { get; set; }

    }
}
