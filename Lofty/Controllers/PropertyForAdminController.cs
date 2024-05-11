using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Property APIs for admin
    /// </summary>
    /// <param name="_propertyManager"></param>
    /// <param name="_facilityManager"></param>
    /// <param name="_propertyImageManager"></param>
    /// <param name="_propertyFacilityManager"></param>
    [Authorize(Roles = "Admin")]
    public class PropertyForAdminController(PropertyManager _propertyManager, FacilityManager _facilityManager, PropertyImageManager _propertyImageManager,
        PropertyFacilityManager _propertyFacilityManager) : ControllerBase
    {
        private readonly PropertyFacilityManager propertyFacilityManager = _propertyFacilityManager;
        private readonly PropertyImageManager propertyImageManager = _propertyImageManager;
        private readonly FacilityManager facilityManager = _facilityManager;
        private readonly PropertyManager propertyManager = _propertyManager;
        ///// <summary>
        ///// Add new property ==> Type 1- resedintial 2- commercial ==> Status 1- Under construction 2- Ready to move and rent
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //[HttpPost("api/Dashboard/Property/Add")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        //public async Task<IActionResult> AddNewPropertyAsync([FromBody] AddNewPropertyViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        APIResult<string> result = await propertyManager.AddNewPropertyAsync(viewModel);
        //        return new JsonResult(result)
        //        {  
        //            StatusCode = result.StatusCode
        //        };
        //    }
        //    else
        //    { 
        //        return BadRequest(new APIResult<string>()
        //        {
        //            Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
        //            IsSucceed = false,
        //            StatusCode = 400
        //        });
        //    }
        //}
        /// <summary>
        /// Add new property ==> Type 1- resedintial 2- commercial ==> Status 1- Under construction 2- Ready to move and rent
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/Property/AddFromForm")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddNewPropertyFromFormAsync([FromForm] AddNewPropertyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await propertyManager.AddNewPropertyAsync(viewModel);
                if (!result.IsSucceed)
                { 
                    return new JsonResult(result)
                    {
                        StatusCode = result.StatusCode
                    };
                }
                AddPropertyImagesViewModel propertyImage = new()
                {
                    Images = viewModel.PropertyImages,
                    PropertyId = result.Data
                };
                bool checkAddedOrNot = await propertyImageManager.AddPropertyImagesAsync(propertyImage);
                if (checkAddedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Property added"
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
        ///// <summary>
        ///// Add new facility
        ///// </summary>
        ///// <param name="SVG"></param>
        ///// <returns></returns>
        //[HttpPost("api/Dashboard/Facility/Add")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        //public async Task<IActionResult> AddNewFacilityAsync([FromForm, Required] FileViewModel SVG)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await facilityManager.AddNewFacilityAsync(SVG);
        //        return Ok(new APIResult<string>()
        //        {
        //            IsSucceed = true,
        //            StatusCode = 200,
        //            Message = "Facility added"
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new APIResult<string>()
        //        {
        //            Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
        //            IsSucceed = false,
        //            StatusCode = 400
        //        });
        //    }
        //}
        /// <summary>
        /// Add property images
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/PropertyImages/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PropertyImageViewModelforAdmin>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<PropertyImageViewModelforAdmin>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddPropertyImagesAsync([FromForm] AddPropertyImagesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<PropertyImageViewModelforAdmin> checkAddedOrNot = await propertyImageManager.AddPropertyImageAsync(viewModel);
                return new JsonResult(checkAddedOrNot)
                { 
                    StatusCode = checkAddedOrNot.StatusCode
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
        /// Get all property images
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [HttpGet("api/Dashboard/PropertyImages/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<string>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAllPropertyImagesAsync([FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                List<string> propertyImages = await propertyImageManager.GetAllPropertyImagesAsync(PropertyId);
                if (propertyImages.Count > 0)
                {
                    return Ok(new APIResult<List<string>>()
                    {
                        Data = propertyImages,
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Property images"
                    });
                }
                else
                {
                    return Ok(new APIResult<List<string>>()
                    {
                        IsSucceed = false,
                        StatusCode = 200,
                        Message = "No images found"
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
                return Ok(new APIResult<List<FacilityViewModelForAdmin>>()
                {
                    IsSucceed = false,
                    StatusCode = 200,
                    Message = "No facility found"
                });
            }
        }
        /// <summary>
        /// Delete property image
        /// </summary>
        /// <param name="PropertyImageId"></param>
        /// <returns></returns>
        [HttpDelete("api/Dashboard/PropertyImage/Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> DeletePropertyImageAsync([FromQuery, Required] int PropertyImageId)
        {
            if (ModelState.IsValid)
            {
                bool checkImageDeletedOrNot = await propertyImageManager.DeletePropertyImageAsync(PropertyImageId);
                if (checkImageDeletedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Property image deleted"
                    });
                }
                else
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "Image not found"
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
        /// Add property facility
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("api/Dashboard/PropertyFacility/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PropertyFacilityViewModelForAdmin>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<PropertyFacilityViewModelForAdmin>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddPropertyFacilityAsync([FromQuery, Required] AddPropertyFacilityViewModel viewModel, [FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                APIResult<PropertyFacilityViewModelForAdmin> result = await propertyFacilityManager.AddFacilityToAPropertyAsync(viewModel, PropertyId);
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
        /// Delete property facility
        /// </summary>
        /// <param name="PropertyFacilityId"></param>
        /// <returns></returns>
        [HttpDelete("api/Dashboard/PropertyFacility/Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> DeletePropertyFacilityAsync([FromQuery, Required] int PropertyFacilityId)
        {
            if (ModelState.IsValid)
            {
                bool checkFacilityDeletedOrNot = await propertyFacilityManager.DeleteFacilityFromAPropertyAsync(PropertyFacilityId);
                if (checkFacilityDeletedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Property facility deleted"
                    });
                }
                else
                {
                    return NotFound(new APIResult<string>()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "facility not found"
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
        /// Update property facilities index
        /// </summary>
        /// <param name="PropertyFacilitiesIds"></param>
        /// <returns></returns>
        [HttpPut("api/Dashboard/PropertyFacility/UpdateIndex")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdatePropertyFacilitiesNumberAsync([FromBody] List<int> PropertyFacilitiesIds)
        {
            APIResult<string> result = await propertyFacilityManager.UpdatePropertyFacilityNumberAsync(PropertyFacilitiesIds);
            return new JsonResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
        /// <summary>
        /// Get property details for admin by property id ==> Type 1- resedintial 2- commercial ==> Status 1- Under construction 2- Ready to move and rent
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [HttpGet("api/Dashboard/Property/GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<PropertyDetailsViewModelForAdmin>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetPropertyDetailsByIdAsync([FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                PropertyDetailsViewModelForAdmin property = await propertyManager.GetPropertyDetailsByIdForAdminAsync(PropertyId);
                if (property != null)
                {
                    return Ok(new APIResult<PropertyDetailsViewModelForAdmin>()
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
                        StatusCode = 404,
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
        /// Update property ==> Type 1- resedintial 2- commercial ==> Status 1- Under construction 2- Ready to move and rent
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut("api/Dashboard/Property/Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdatePropertyAsync([FromBody] UpdatePropertyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await propertyManager.UpdatePropertyAsync(viewModel);
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
        /// Enable and disable property
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <returns></returns>
        [HttpPut("api/Dashboard/Property/EnableAndDisable")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> EnableAndDisablePropertyAsync([FromQuery, Required] string PropertyId)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await propertyManager.EnableAndDisablePropertyAsync(PropertyId);
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
