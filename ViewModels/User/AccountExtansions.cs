using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class AccountExtansions
    {
        public static User ToUserModel(this UserSignUpViewModel viewModel)
        {
            return new User()
            {
                IdentityNumber = viewModel.IdentityNumber,
                Address = viewModel.Address,
                CompanyName = viewModel.CompanyName,
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
                UserName = viewModel.Email,
                FirstName = viewModel.FirstName,
                InvestoreType = viewModel.InvestoreType,
                LastName = viewModel.LastName,
                MiddleName = viewModel.MiddleName,
                Occupation = viewModel.Occupation,
                PhoneNumber = viewModel.PhoneNumber,
                ReciveEmails = viewModel.ReciveEmails,
            };
        }
    }
}
