using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Reposatiory
{
    public class AccountManager(LoftyContext _mydB, UnitOfWork _unitOfWork, UserManager<User> _userManager, IConfiguration _configuration) : MainManager<User>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        private readonly UserManager<User> userManager = _userManager;
        private readonly IConfiguration configuration = _configuration;
        public async Task<IdentityResult> CreatePasswordAsync(User user, string password)
        {
            try
            {
                IdentityResult identityResult = await userManager.CreateAsync(user, password);
                return identityResult;
            }
            catch (ArgumentNullException)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = "Password is requierd",
                });
            }
            catch (DbUpdateException ex)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = ex.InnerException.Message,
                });
            }
        }
        public async Task<APIResult<UserDataViewModel>> SignUpAsync(UserSignUpViewModel viewModel)
        {
            APIResult<UserDataViewModel> aPIResult = new();
            User user = viewModel.ToUserModel();
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.IdentityImage.FileName;
            user.IdentityImageUrl = uniqueFileName;
            IdentityResult identityResult = await CreatePasswordAsync(user, viewModel.Password);
            if (identityResult.Succeeded)
            {
                FileStream fileStream = new FileStream(
                    Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", uniqueFileName),
                    FileMode.Create);
                await viewModel.IdentityImage.CopyToAsync(fileStream);
                fileStream.Position = 0;
                fileStream.Close();
                await userManager.AddToRoleAsync(user, "Customer");
                List<Claim> claims = new();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                JwtSecurityToken securityToken = new(
                        claims: claims,
                        signingCredentials: new SigningCredentials(
                            key: new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(this.configuration["JWT:Key"])
                                ),
                            algorithm: SecurityAlgorithms.HmacSha384
                            ),
                        expires: DateTime.Now.AddMonths(12)
                        );
                UserDataViewModel userData = new()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Roles = new List<string>
                    {
                        "Customer"
                    },
                    Name = user.FirstName + " " + user.LastName,
                };
                aPIResult.Data = userData;
                aPIResult.IsSucceed = true;
                aPIResult.Message = "Authenticated";
                aPIResult.StatusCode = 200;
                return aPIResult;
            }
            else
            {
                string message = identityResult.Errors.Select(i => i.Description).FirstOrDefault();
                aPIResult.IsSucceed = false;
                aPIResult.Message = message.Contains("Email") || message.Contains("Username") ?
                    "Email already taken" : message.Contains("PhoneNumber") ? "Phone number already taken" :
                    message.Contains("IdentityNumber") ? "Identity number already used befor" : message;
                aPIResult.StatusCode = 400;
                return aPIResult;
            }
        }
        public async Task<APIResult<UserDataViewModel>> SignInAsync(UserSignInViewModel viewModel)
        {
            APIResult<UserDataViewModel> APIResult = new();
            User user = await userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                APIResult<UserDataViewModel> aPIResult = await CheckPasswordAsync(user, viewModel.Password);
                return aPIResult;
            }
            else
            {
                APIResult.IsSucceed = false;
                APIResult.Message = "User not exist";
                APIResult.StatusCode = 401;
                return APIResult;
            }
        }
        public async Task<APIResult<UserDataViewModel>> CheckPasswordAsync(User user, string password)
        {
            APIResult<UserDataViewModel> APIResult = new();
            List<Claim> claims = new();
            bool checkPassword = await userManager.CheckPasswordAsync(user, password);
            if (checkPassword)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                IList<string> roles = await userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                JwtSecurityToken securityToken = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(this.configuration["JWT:Key"])
                            ),
                        algorithm: SecurityAlgorithms.HmacSha384
                        ),
                    expires: DateTime.Now.AddMonths(12)
                    );
                UserDataViewModel userDataViewModel = new()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Roles = roles.ToList(),
                    Name = user.FirstName + " " + user.LastName,
                };
                APIResult.Data = userDataViewModel;
                APIResult.Message = "Welcome back!";
                APIResult.StatusCode = 200;
                APIResult.IsSucceed = true;
                return APIResult;
            }
            else
            {
                APIResult.Message = "Wrong password";
                APIResult.IsSucceed = false;
                APIResult.StatusCode = 401;
                return APIResult;
            }
        }
        public async Task<UserInformationForCheckOutViewModel> GetUserInformationForCheckOutAsync(string userId)
        {
            UserInformationForCheckOutViewModel userInformation = await GetAll().Where(i => i.Id == userId).Select(AccountExtansions.ToUserInformationForCheckOutExpression()).FirstOrDefaultAsync();
            return userInformation;
        }
    }
}
