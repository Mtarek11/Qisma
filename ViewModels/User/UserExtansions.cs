using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class UserExtansions
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
                InvestoreType = viewModel.InvestorType,
                LastName = viewModel.LastName,
                MiddleName = viewModel.MiddleName,
                Occupation = viewModel.Occupation,
                PhoneNumber = viewModel.PhoneNumber,
                ReciveEmails = viewModel.ReciveEmails,
            };
        }
        public static Expression<Func<User, UserFullInformationViewModel>> ToUserFullInformationViewModelExpression()
        {
            return user => new UserFullInformationViewModel()
            {
                Address = user.Address,
                CompanyName = user.CompanyName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName, 
                IdentityImageUrl = user.IdentityImageUrl,
                IdentityNumber = user.IdentityNumber,
                InvestorType = user.InvestoreType,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Occupation = user.Occupation,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
                InvestoreType = user.InvestoreType
            };
        }
    }
}
