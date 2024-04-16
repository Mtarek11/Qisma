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
    public class UserFullInformationViewModel
    {
        public string UserId {  get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentityImageUrl { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public InvestorType InvestorType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
