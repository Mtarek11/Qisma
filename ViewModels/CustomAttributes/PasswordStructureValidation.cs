using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PasswordStructureValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Password is requierd");
            }
            string password = value.ToString();
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$";
            if (!Regex.IsMatch(password, pattern))
            {
                return new ValidationResult("The password must have at least 6 characters, including at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            }
            return ValidationResult.Success;
        }
    }
}
