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
    public class UpdateUserInformationViewModel
    {
        [UserNameLength(ErrorMessage = "First name must be at least 3 character.")]
        public string FirstName { get; set; }
        [UserNameLength(ErrorMessage = "Middle name must be at least 3 character.")]
        public string MiddleName { get; set; }
        [UserNameLength(ErrorMessage = "Last name must be at least 3 character.")]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress] 
        public string Email { get; set; }
        [MinimumAge(16)]
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        [NationalIdValidation]
        public string IdentityNumber { get; set; }
        [AllowedImageExtensions]
        public IFormFile IdentityImage { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public bool? ReciveEmails { get; set; }
        [Range(1, 2, ErrorMessage = "Invalid InvestoreType value.")]
        public InvestorType? InvestorType { get; set; }
    }
}
