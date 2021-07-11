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

        [Display(Name = "Competition ID")]
        [Required]
        public int CompetitionID { get; set; }

        [Required(ErrorMessage = "Please enter your comments")]
        [StringLength(250, MinimumLength = 3)]
        public string CommentDesc { get; set; }

        // ignore
        [DataType(DataType.Date)]
        [Display(Name = "Date & Time Posted")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime DateTimePosted { get; set; }
    }
}
