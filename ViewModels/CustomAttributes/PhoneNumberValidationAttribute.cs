using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Phone number is requierd.");
            }
            string phoneNumber = value.ToString();
            if (!phoneNumber.StartsWith("01") || phoneNumber.Length != 11 || !IsDigitsOnly(phoneNumber))
            {
                return new ValidationResult("The phone number must start with '01' and contain exactly 11 digits.");
            }

            return ValidationResult.Success;
        }
        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
