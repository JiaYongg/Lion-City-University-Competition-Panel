﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.DAL;

namespace WEB2021Apr_P01_T01.Models
{
    public class ValidateEmailAddress : ValidationAttribute
    {
        // Change later need database
        private JudgeDAL judgeContext = new JudgeDAL();
        /*
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);

            // Casting the validation context to the "Staff" model class
            Login login = (Login)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            /*
            int staffId = staff.StaffId;
            if (judgeContext.IsEmailExist(email, staffId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
           
        }*/
    }
}
