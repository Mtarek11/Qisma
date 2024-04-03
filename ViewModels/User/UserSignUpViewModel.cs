using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserSignUpViewModel
    {
        [Required]
        [UserNameLength(ErrorMessage = "First name must be at least 3 character.")]
        public string FirstName { get; set; }
        [Required] 
        [UserNameLength(ErrorMessage = "Middle name must be at least 3 character.")]
        public string MiddleName { get; set; }
        [Required]
        [UserNameLength(ErrorMessage = "Last name must be at least 3 character.")]
        public string LastName { get; set; }
        [Required]
        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinimumAge(16)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [NationalIdValidation]
        public string IdentityNumber { get; set; }
        [Required]
        [AllowedImageExtensions]
        public IFormFile IdentityImage { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public bool ReciveEmails { get; set; }
        [Required]
        [Range(1, 2, ErrorMessage = "Invalid InvestoreType value.")]
        public InvestoreType InvestoreType { get; set; }
        [Required]
        [PasswordStructureValidation]
        public string Password { get; set; }
    }
}
