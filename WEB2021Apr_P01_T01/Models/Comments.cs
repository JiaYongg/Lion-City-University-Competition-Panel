using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WEB2021Apr_P01_T01.Models
{
    public class Comments
    {
        [Display(Name = "Comment ID")]
        public int? CommentId { get; set; }

        [Required]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Required(ErrorMessage = "Please enter your comments")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Comments must be between 3 to 250 characters")]
        public string CommentDesc { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date & Time Posted")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime DateTimePosted { get; set; }
    }
}
