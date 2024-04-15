using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Governorate and city apis
    /// </summary>
    public class GovernorateAndCityController(GovernorateManager _governorateManager, CityManager _cityManager) : ControllerBase
    { 
        private readonly GovernorateManager governorateManager = _governorateManager;
        private readonly CityManager cityManager = _cityManager;
        /// <summary>
        /// Get all governorates
        /// </summary> 
        /// <returns></returns>
        [HttpGet("api/Governorate/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<GovernorateAndCityViewModel>>))]
        public async Task<IActionResult> GetAllGovernoratesAsync()
        {
            List<GovernorateAndCityViewModel> governorates = await governorateManager.GetAllGovernoratesAsync();
            return Ok(new APIResult<List<GovernorateAndCityViewModel>>()
            {
                Data = governorates,
                Message = "Get all governorates",
                IsSucceed = true,
                StatusCode = 200
            });
        }
        /// <summary>
        /// Get cities by governorate id
        /// </summary> 
        /// <param name="GovernorateId"></param>
        /// <returns></returns>
        [HttpGet("api/Cities/GetByGovernorateId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<GovernorateAndCityViewModel>>))]
        public async Task<IActionResult> GetCitiesByGovernorateId([FromQuery] int GovernorateId)
        {
            List<GovernorateAndCityViewModel> cities = await cityManager.GetCitiesByGovernorateIdAsync(GovernorateId);
            return Ok(new APIResult<List<GovernorateAndCityViewModel>>()
            {
                Data = cities,
                Message = "Get cities by governorate id",
                IsSucceed = true,
                StatusCode = 200
            });
        }
    }
}
