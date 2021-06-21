using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P01_T01.Models
{
    public class Criteria
    {
        [Display(Name = "Criteria ID")]
        public int? CriteriaID { get; set; }
        [Display(Name = "Competition ID")]
        public int? CompetitionID { get; set; }
        [Required]
        [Display(Name = "Criteria Name")]
        public string CriteriaName { get; set; }
        [Required]
        [Display(Name = "Weightage of Criteria")]
        public int Weightage { get; set; }


    }
}
