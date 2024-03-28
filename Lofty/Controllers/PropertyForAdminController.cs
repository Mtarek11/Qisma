using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.ComponentModel.DataAnnotations;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Property apis for admin
    /// </summary>
    /// <param name="_propertyManager"></param>
    /// <param name="_facilityManager"></param>
    /// <param name="_propertyImageManager"></param>
    public class PropertyForAdminController(PropertyManager _propertyManager, FacilityManager _facilityManager, PropertyImageManager _propertyImageManager) : ControllerBase
    {
        private readonly PropertyImageManager propertyImageManager = _propertyImageManager;
        private readonly FacilityManager facilityManager = _facilityManager;
        private readonly PropertyManager propertyManager = _propertyManager;
        /// <summary>
        /// Add new property ==> Type 1- resedintial 2- commercial
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/Property/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<int>))]
        public async Task<IActionResult> AddNewPropertyAsync([FromBody] AddNewPropertyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<int> result = await propertyManager.AddNewPropertyAsync(viewModel);
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
        /// Add new facility
        /// </summary>
        /// <param name="SVG"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/Facility/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddNewFacilityAsync([FromForm, Required] IFormFile SVG)
        {
            if (ModelState.IsValid)
            {
                await facilityManager.AddNewFacilityAsync(SVG);
                return Ok(new APIResult<string>()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Facility added"
                });
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
        /// Add property images
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/PropertyImages/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddPropertyImagesAsync([FromForm] AddPropertyImagesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool checkAddedOrNot = await propertyImageManager.AddPropertyImagesAsync(viewModel);
                if (checkAddedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Property images added"
                    });
                }
                else
                {
                    return BadRequest(new APIResult<string>()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
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
        /// <summary>
        /// Get all facilities
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/Dashboard/Facility/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<FacilityViewModelForAdmin>>))]
        public async Task<IActionResult> GetAllFacilitiesAsync()
        {
            List<FacilityViewModelForAdmin> facilities = await facilityManager.GetAllFacilitiesAsync();
            if (facilities.Count > 0)
            {
                return Ok(new APIResult<List<FacilityViewModelForAdmin>>()
                {
                    Data = facilities,
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Get all facilities"
                });
            }
            else
            {
                return Ok(new APIResult<string>()
                {
                    IsSucceed = false,
                    StatusCode = 200,
                    Message = "No facility found"
                });
            }
        }
    }
}
