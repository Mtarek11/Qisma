using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }  
        public string LastName { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public string Address {  get; set; }
        public string IdentityNumber {  get; set; }
        public string IdentityImageUrl {  get; set; }
        public string Occupation {  get; set; }
        public string CompanyName {  get; set; }
        public bool ReciveEmails { get; set; }
        public InvestorType InvestoreType {  get; set; }
    }
    public enum InvestorType
    {
        Retail = 1,
        Institutional = 2
    }
}
