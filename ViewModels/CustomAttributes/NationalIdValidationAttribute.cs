using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class NationalIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }
            string nationalId = value.ToString();
            if (nationalId.Length < 9 || !nationalId.All(char.IsDigit))
            {
                return new ValidationResult("Identity number must be not less than 9 digits.");
            }
            return ValidationResult.Success;
        }
    }
}
