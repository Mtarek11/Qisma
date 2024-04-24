using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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
    /// Orders APIs
    /// </summary>
    /// <param name="_orderManager"></param>
    public class OrderController(OrderManager _orderManager) : ControllerBase
    {
        private readonly OrderManager orderManager = _orderManager;
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
        /// Get all orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Dashboard/Order/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForAdmin>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] OrderStatus? OrderStatus, [FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                PaginationViewModel<OrderViewModelForAdmin> orders = await orderManager.GetAllOrdersForAdminAsync(PageNumber, PageSize, OrderStatus);
                if (orders.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForAdmin>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No orders found"
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
        /// <param name="ConfirmedOrRejected"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("api/Dashboard/Orders/ConfirmOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> ConfirmOrderAsync([FromQuery, Required] int OrderId, [FromQuery, Required] bool ConfirmedOrRejected)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await orderManager.ConfirmOrRejectOrderPaymentAsync(OrderId, ConfirmedOrRejected);
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
        /// Get all user orders
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpGet("api/Order/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<OrderViewModelForUser>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllUserOrdersAsync([FromQuery] OrderStatus? OrderStatus, [FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                PaginationViewModel<OrderViewModelForUser> orders = await orderManager.GetAllOrdersForUserAsync(PageNumber, PageSize, userId, OrderStatus);
                if (orders.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        Data = orders,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all orders"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<OrderViewModelForUser>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No orders found"
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
