using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// User account apis
    /// </summary>
    /// <param name="_accountManager"></param>
    public class UserAccountController(AccountManager _accountManager) : ControllerBase
    {
        private readonly AccountManager accountManager = _accountManager;
        /// <summary>
        /// Sign up for customers ==> InvestoreType 1- Retail 2- Institutional
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/SignUpForCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<UserDataViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<UserDataViewModel>))]
        public async Task<IActionResult> SignUpForCustomersAsync([FromForm] UserSignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<UserDataViewModel> result = await accountManager.SignUpAsync(viewModel);
                return new JsonResult(result)
                {
                    StatusCode = result.StatusCode
                };
            }
            else
            {
                return BadRequest(new APIResult<string>()
                {
                    Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
                    IsSucceed = false,
                    StatusCode = 400
                });
            }
        }
        /// <summary>
        /// Sign in 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<UserDataViewModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(APIResult<UserDataViewModel>))]
        public async Task<IActionResult> SignInForUserAsync([FromForm] UserSignInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<UserDataViewModel> result = await accountManager.SignInAsync(viewModel);
                return new JsonResult(result)
                {
                    StatusCode = result.StatusCode
                };
            }
            else
            {
                return Unauthorized(new APIResult<UserDataViewModel>()
                {
                    Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
                    IsSucceed = false,
                    StatusCode = 401
                });
            }
        }
        /// <summary>
        /// Get user information for check out
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/User/UserInformationForCheckOut")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<UserInformationForCheckOutViewModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetUserInformationForCheckOutAsync()
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            UserInformationForCheckOutViewModel userInformation = await accountManager.GetUserInformationForCheckOutAsync(userId);
            if (userInformation != null)
            {
                return Ok(new APIResult<UserInformationForCheckOutViewModel>()
                {
                    Data = userInformation,
                    StatusCode = 200,
                    IsSucceed = true,
                    Message = "User information"
                });
            }
            else
            {
                return Unauthorized(new APIResult<string>()
                {
                    StatusCode = 401,
                    IsSucceed = false,
                    Message = "User not found"
                });
            }
        }
    }
}
