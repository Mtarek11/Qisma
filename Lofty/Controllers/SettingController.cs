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
        /// Get support email ==> id = 1, support phone number  ==> id = 2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("api/GetAboutQismaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAboutQismaContentById([FromQuery, Required] int id)
        {
            if (ModelState.IsValid)
            {
                string content = await aboutQismaManager.GetAboutQismaContentAsync(id);
                if (content != null)
                {
                    return Ok(new APIResult<string>()
                    {
                        Data = content,
                        Message = "Qisma",
                        StatusCode = 200,
                        IsSucceed = true
                    });
                }
                else
                {
                    return NotFound(new APIResult<string>()
                    {
                        Message = "No data found",
                        StatusCode = 404,
                        IsSucceed = false
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
        /// Update support email ==> id = 1, support phone number  ==> id = 2
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost("api/Dashboard/UpdateAboutQismaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> GetAboutQismaContentById([FromQuery, Required] int id, [FromQuery, Required] string content)
        {
            if (ModelState.IsValid)
            {
                bool checkUpdatedOrNot = await aboutQismaManager.UpdateAboutQismaContent(id, content);
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
