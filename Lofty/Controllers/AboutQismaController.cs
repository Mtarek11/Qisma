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
using Models;

namespace Lofty.Controllers
{
    /// <summary>
    /// About qisma api
    /// </summary>
    /// <param name="_aboutQismaManager"></param>
    /// <param name="_teamMeamberManager"></param>
    public class AboutQismaController(AboutQismaManager _aboutQismaManager, TeamMeamberManager _teamMeamberManager) : ControllerBase
    {
        private readonly AboutQismaManager aboutQismaManager = _aboutQismaManager;
        private readonly TeamMeamberManager teamMeamberManager = _teamMeamberManager;
        /// <summary>
        /// Get support email, support phone number
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/AboutQisma/GetSupport")]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdateSuppoortContactInformationsAsync([FromBody] SupportViewModel viewModel)
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
                return BadRequest(new APIResult<string>()
                {
                    Message = "Nothing to update",
                    StatusCode = 400,
                    IsSucceed = false
                });
            }
        }
        /// <summary>
        /// Get about us 1st and 2nd frame
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/AboutQisma/GetAboutUs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<AboutUsViewModel>))]
        public async Task<IActionResult> GetAboutUsAsync()
        {
            AboutUsViewModel aboutUs = await aboutQismaManager.GetAboutUsAsync();
            return Ok(new APIResult<AboutUsViewModel>()
            {
                Data = aboutUs,
                Message = "Qisma",
                StatusCode = 200,
                IsSucceed = true
            });
        }
        /// <summary>
        /// Update about us 1st and 2nd frame
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("api/Dashboard/AboutQisma/UpdateAboutUs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdateAboutUsAsync([FromBody] AboutUsViewModel viewModel)
        {
            bool checkUpdatedOrNot = await aboutQismaManager.UpdateAboutUsAsync(viewModel);
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
                return BadRequest(new APIResult<string>()
                {
                    Message = "Nothing to update",
                    StatusCode = 400,
                    IsSucceed = false
                });
            }
        }
        /// <summary>
        /// Add team member
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("api/Dashboard/AboutQisma/AddTeamMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddTeamMemberAsync([FromForm] AddTeamMemberViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await teamMeamberManager.AddTeamMemberAsync(viewModel);
                return Ok(new APIResult<string>()
                {
                    IsSucceed = true,
                    Message = "Team member added",
                    StatusCode = 200
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
        /// Get all team members
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/AboutQisma/GetAllManagers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<TeamMember>>))]
        public async Task<IActionResult> GetAllManagersAsync()
        {
            List<TeamMember> teamMembers = await teamMeamberManager.GetAllManagersAsync();
            if (teamMembers.Count > 0)
            {
                return Ok(new APIResult<List<TeamMember>>()
                {
                    Data = teamMembers,
                    IsSucceed = true,
                    Message = "Managers",
                    StatusCode = 200
                });
            }
            else
            {
                return Ok(new APIResult<List<TeamMember>>()
                {
                    IsSucceed = false,
                    Message = "No managers found",
                    StatusCode = 200
                });
            }
        }
        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/AboutQisma/GetAllMembers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<TeamMember>>))]
        public async Task<IActionResult> GetAllMembersAsync()
        {
            List<TeamMember> teamMembers = await teamMeamberManager.GetAllMembersAsync();
            if (teamMembers.Count > 0)
            {
                return Ok(new APIResult<List<TeamMember>>()
                {
                    Data = teamMembers,
                    IsSucceed = true,
                    Message = "Team members",
                    StatusCode = 200
                });
            }
            else
            {
                return Ok(new APIResult<List<TeamMember>>()
                {
                    IsSucceed = false,
                    Message = "No team members found",
                    StatusCode = 200
                });
            }
        }
        /// <summary>
        /// Update team members
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("api/Dashboard/AboutQisma/UpdateTeamMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdateTeamMembersAsync([FromForm] UpdateTeamMemberViewModel viewModel)
        {
            if (ModelState.IsValid) 
            {
                APIResult<string> result = await teamMeamberManager.UpdateTeamMemberAsync(viewModel);
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
        /// Delete team members
        /// </summary>
        /// <param name="TeamMemberId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("api/Dashboard/AboutQisma/DeleteTeamMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> DeleteTeamMemberAsync([FromQuery, Required] int TeamMemberId)
        {
            if (ModelState.IsValid)
            {
                bool checkDeletedOrNot = await teamMeamberManager.DeleteTeamMemberAsync(TeamMemberId);
                if (checkDeletedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        Message = "Team member deleted",
                        StatusCode = 200
                    });
                }
                else
                {
                    return NotFound(new APIResult<string>()
                    {
                        IsSucceed = false,
                        Message = "Team member not found",
                        StatusCode = 404
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
