using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reposatiory;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lofty.Controllers
{
    /// <summary>
    /// Settings api
    /// </summary>
    /// <param name="_aboutQismaManager"></param>
    public class SettingController(AboutQismaManager _aboutQismaManager) : ControllerBase
    {
        private readonly AboutQismaManager aboutQismaManager = _aboutQismaManager;
        /// <summary>
        /// Get support email, support phone number
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/AboutQisma/GetSupport")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<SupportViewModel>))]
        public async Task<IActionResult> GetSupportContactInformationsAsync()
        {
            SupportViewModel support = await aboutQismaManager.GetSupportContactInformationsAsync();
            return Ok(new APIResult<SupportViewModel>()
            {
                Data = support,
                Message = "Qisma",
                StatusCode = 200,
                IsSucceed = true
            });
        }
        /// <summary>
        /// Update support email, support phone number
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("api/Dashboard/AboutQisma/UpdateSupport")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAboutQismaContentById([FromBody] SupportViewModel viewModel)
        {
            bool checkUpdatedOrNot = await aboutQismaManager.UpdateSupportContactInformationsAsync(viewModel);
            if (checkUpdatedOrNot)
            {
                return Ok(new APIResult<string>()
                {
                    Message = "Updated",
                    StatusCode = 200,
                    IsSucceed = true
                });
            }
            else
            {
                return Conflict(new APIResult<string>()
                {
                    Message = "Nothing to update",
                    StatusCode = 409,
                    IsSucceed = false
                });
            }
        }
    }
}
