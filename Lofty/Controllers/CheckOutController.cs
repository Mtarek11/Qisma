using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Check out APIs
    /// </summary>
    /// <param name="_buyTrackerManager"></param>
    public class CheckOutController(BuyTrackerManager _buyTrackerManager) : ControllerBase
    {
        private readonly BuyTrackerManager buyTrackerManager = _buyTrackerManager;
        /// <summary>
        /// Proceed with buy
        /// </summary> 
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [Authorize(Roles ="Customer")]
        [HttpGet("api/CheckOut/ProceedWithBuy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllGovernoratesAsync([Required, FromQuery] int PropertyId)
        {
            Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            APIResult<string> result = await buyTrackerManager.ProceedWithBuyAsync(userId, PropertyId);
            return new JsonResult(result)
            { 
                StatusCode = result.StatusCode
            };
        }
    }
}
