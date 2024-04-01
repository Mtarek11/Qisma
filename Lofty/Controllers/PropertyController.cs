using Microsoft.AspNetCore.Mvc;
using Models;
using Reposatiory;
using System.ComponentModel.DataAnnotations;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Property apis
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
        /// <param name="MaxUnitPrice"></param>
        /// <param name="MinSharePrice"></param>
        /// <param name="MaxSharePrice"></param>
        /// <returns></returns>
        [HttpGet("api/Property/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PaginationViewModel<PropertyViewModelInListViewForUser>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllPropertiesForUserAsync([FromQuery, Required] int PageNumber = 0, [FromQuery, Required] int PageSize = 10, [FromQuery] int? GovernorateId = null,
           [FromQuery] int? CityId = null, [FromQuery] Models.Type? PropertyType = null, [FromQuery] double? MinUnitPrice = null, [FromQuery] double? MaxUnitPrice = null,
              [FromQuery] double? MinSharePrice = null, [FromQuery] double? MaxSharePrice = null)
        {
            if (ModelState.IsValid)
            { 
                PaginationViewModel<PropertyViewModelInListViewForUser> properties = await propertyManager.GetAllPropertiesForUserAsync(PageNumber, PageSize, GovernorateId, CityId, PropertyType,
                   MinUnitPrice, MaxUnitPrice, MinSharePrice, MaxSharePrice);
                if (properties.ItemsList.Count > 0)
                {
                    return Ok(new APIResult<PaginationViewModel<PropertyViewModelInListViewForUser>>()
                    {
                        Data = properties,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get all properties"
                    });
                }
                else
                {
                    return Ok(new APIResult<string>()
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetPropertyDetailsByIdAsync([FromQuery, Required] int PropertyId)
        {
            if (ModelState.IsValid)
            {
                PropertyDetailsViewModelForUser property = await propertyManager.GetPropertyDetailsByIdForUserAsync(PropertyId);
                if (property != null)
                {
                    return Ok(new APIResult<PropertyDetailsViewModelForUser>()
                    {
                        Data = property,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Get property details"
                    });
                }
                else
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "Property not found"
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
