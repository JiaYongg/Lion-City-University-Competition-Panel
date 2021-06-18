using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;

namespace WEB2021Apr_P01_T01.Models
{
    public class ValidateEmailAddress : ValidationAttribute
    {
        private SignUpDAL signUpContext = new SignUpDAL();

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);

            // Casting the validation context to the "Staff" model class
            Competitor competitor = (Competitor)validationContext.ObjectInstance;
            Judge judge = (Judge)validationContext.ObjectInstance;

            // Get the Staff Id from the staff instance
            int competitorId = competitor.CompetitorId;
            int judgeId = judge.JudgeId;

            if (signUpContext.IsCompetitorEmailExist(email, competitorId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else if (signUpContext.IsJudgeEmailExist(email, judgeId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
