using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class Competition : IValidatableObject
    {
        [Display(Name = "Competition ID")]
        public int CompetitionId { get; set; }
        
        [Required]
        [Display(Name = "Interest ID")]
        public int AoiId { get; set; }
        //public AreaInterest Aoi { get; set; }

        [Display(Name = "Area of Interest: ")]
        public string? AoiName { get; set; }

        [Required (ErrorMessage = "Please enter the competition name")]
        [Display(Name = "Competition Name: ")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The competition name must be between 3 to 100 characters.")]
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

        public bool Validated { get; set; }

        public bool HaveParticipant { get; set; }

        public List<CompetitionSubmission> SubmissionList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate <= DateTime.Today.AddDays(1))
            {
                yield return new ValidationResult("Start Date must be at least 1 day after today", new[] { "StartDate" });
            }

            if (EndDate <= StartDate)
            {
                yield return new ValidationResult("End Date must be later that Start Date", new[] { "EndDate" });
            }

            if (ResultsReleaseDate <= EndDate)
            {
                yield return new ValidationResult("Result Release Date must be later than Start & End Date", new[] { "ResultsReleaseDate" });
            }

            Validated = true;
        }
    }
}
