using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Check out and orders APIs
    /// </summary>
    /// <param name="_buyTrackerManager"></param>
    /// <param name="_orderManager"></param>
    public class CheckOutAndOrderController(BuyTrackerManager _buyTrackerManager, OrderManager _orderManager) : ControllerBase
    {
        private readonly OrderManager orderManager = _orderManager;
        private readonly BuyTrackerManager buyTrackerManager = _buyTrackerManager;
        /// <summary>
        /// Proceed with buy
        /// </summary> 
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpPost("api/CheckOut/ProceedWithBuy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> ProceedWithBuyAsync([Required, FromQuery] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                APIResult<string> result = await buyTrackerManager.ProceedWithBuyAsync(userId, PropertyId);
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
        /// Place order
        /// </summary> 
        /// <param name="PropertyId"></param>
        /// <param name="NumberOfShares"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpPost("api/CheckOut/PlaceOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> PlaceOrderAsync([Required, FromQuery] string PropertyId, [Required, FromQuery] int NumberOfShares)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                APIResult<string> result = await orderManager.CreateOrderAsync(userId, PropertyId, NumberOfShares);
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
        /// Get all pending orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Dashboard/Order/GetAllPending")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForAdmin>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllPendingOrdersAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                PaginationViewModel<OrderViewModelForAdmin> orders = await orderManager.GetAllPendingOrdersForAdminAsync(PageNumber, PageSize);
                if (orders.ItemsList.Count > 0)
                { 
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all pending orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No pending orders found"
                    });
                }
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
        /// Get all confirmed orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Dashboard/Order/GetAllConfirmed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForAdmin>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllConfirmedOrdersAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                PaginationViewModel<OrderViewModelForAdmin> orders = await orderManager.GetAllConfirmedOrdersForAdminAsync(PageNumber, PageSize);
                if (orders.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all confirmed orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No confirmed orders found"
                    });
                }
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
        /// Confirm order payment
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost("api/Dashboard/Orders/ConfirmOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> ConfirmOrderAsync([FromQuery, Required] int OrderId)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await orderManager.ConfirmOrderPaymentAsync(OrderId);
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
        /// Get all pending user orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Order/GetAllPending")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForUser>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllUserPendingOrdersAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                PaginationViewModel<OrderViewModelForUser> orders = await orderManager.GetAllPendingOrdersForUserAsync(PageNumber, PageSize, userId);
                if (orders.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all pending orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No pending orders found"
                    });
                }
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
        /// Get all confirmed user orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Order/GetAllConfirmed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForUser>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllUserConfirmedOrdersAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                PaginationViewModel<OrderViewModelForUser> orders = await orderManager.GetAllConfirmedOrdersForUserAsync(PageNumber, PageSize, userId);
                if (orders.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all confirmed orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No confirmed orders found"
                    });
                }
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
    }
}
