using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
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
    }
}
