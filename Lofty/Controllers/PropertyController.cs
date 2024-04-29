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
    /// Property APIs
    /// </summary>
    /// <param name="_propertyManager"></param>
    public class PropertyController(PropertyManager _propertyManager) : ControllerBase
    {
        private readonly PropertyManager propertyManager = _propertyManager;
        /// <summary>
        /// Get all properties for user
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="GovernorateId"></param>
        /// <param name="CityId"></param>
        /// <param name="PropertyType"></param>
        /// <param name="MinUnitPrice"></param>
        /// <param name="MinSharePrice"></param>
        /// <returns></returns>
        [HttpGet("api/Property/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<PropertyViewModelInListView>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllPropertiesForUserAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10, [FromQuery] int? GovernorateId = null,
           [FromQuery] int? CityId = null, [FromQuery] Models.Type? PropertyType = null, [FromQuery] double? MinUnitPrice = null, [FromQuery] double? MinSharePrice = null)
        { 
            if (ModelState.IsValid)
            {
                bool isAdmin = false;
                if (User != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                        if (claimsIdentity?.Claims != null)
                        {
                            IEnumerable<string> roles = claimsIdentity.FindAll(ClaimTypes.Role).Select(c => c.Value);
                            foreach (var role in roles)
                            {
                                if (role == "Admin")
                                {
                                    isAdmin = true;
                                }
                            }
                        }
                    }
                }
                PaginationViewModel<PropertyViewModelInListView> properties = await propertyManager.GetAllPropertiesForUserAsync(PageNumber, PageSize, GovernorateId, CityId, PropertyType,
                   MinUnitPrice, MinSharePrice, isAdmin);
                if (properties.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<PropertyViewModelInListView>>()
                    {
                        Data = properties,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all properties"
                    });
                }
                else
                {
                    return Ok(new APIResult<PaginationViewModel<PropertyViewModelInListView>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No property found"
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
        /// Get property details for user by property id ==> Type 1- resedintial 2- commercial ==> Status 1- Under construction 2- Ready to move and rent
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [HttpGet("api/Property/GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PropertyDetailsViewModelForUser>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<PropertyDetailsViewModelForUser>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(APIResult<PropertyDetailsViewModelForUser>))]
        public async Task<IActionResult> GetPropertyDetailsByIdAsync([FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                string userId = null;
                bool isAdmin = false;
                if (User != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                        ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                        if (claimsIdentity?.Claims != null)
                        {
                            IEnumerable<string> roles = claimsIdentity.FindAll(ClaimTypes.Role).Select(c => c.Value);
                            foreach (string role in roles)
                            {
                                if (role == "Admin")
                                {
                                    isAdmin = true;
                                }
                            }
                        }
                        if (userIdClaim != null)
                        {
                            userId = userIdClaim.Value; 
                        }
                    }
                }
                APIResult<PropertyDetailsViewModelForUser> result = await propertyManager.GetPropertyDetailsByIdForUserAsync(PropertyId, userId, isAdmin);
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
        /// Get Ordering page
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpGet("api/Property/GetOrderingPage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<OrderingPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<OrderingPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<OrderingPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetOrderingPageAsync([FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                APIResult<OrderingPageViewModel> result = await propertyManager.GetOrderingPageAsync(userId, PropertyId);
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
        /// Get Order preview page
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <param name="NumberOfShares"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpGet("api/Property/GetOrderPreviewPage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<OrderPreviewPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<OrderPreviewPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<OrderPreviewPageViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetOrderPreviewPageAsync([FromQuery, Required] string PropertyId, [FromQuery, Required] int NumberOfShares)
        {
            if (ModelState.IsValid)
            { 
                Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;
                APIResult<OrderPreviewPageViewModel> result = await propertyManager.GetOrderPreviewAsync(userId, PropertyId, NumberOfShares);
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
    }
}
