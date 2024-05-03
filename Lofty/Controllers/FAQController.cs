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
    /// FAQ apis
    /// </summary>
    /// <param name="_fAQManager"></param>
    public class FAQController(FAQManager _fAQManager) : ControllerBase
    {
       private readonly FAQManager fAQManager = _fAQManager;
        /// <summary>
        /// Add faq
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("api/Dashboard/FAQ/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddFAQAsync([FromBody] AddFAQViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await fAQManager.AddFAQAsync(viewModel.Qusetion, viewModel.Answer);
                return Ok(new APIResult<string>()
                {
                    IsSucceed = true,
                    Message = "FAQ added",
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
        /// Delete FAQ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("api/Dashboard/FAQ/Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> DeleteFAQAsync([FromQuery, Required] int Id)
        {
            if (ModelState.IsValid)
            {
                bool checkDeletedOrNot = await fAQManager.DeleteFAQAsync(Id);
                if (checkDeletedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        Message = "FAQ deleted",
                        StatusCode = 200
                    });
                }
                else
                {
                    return NotFound(new APIResult<string>()
                    {
                        IsSucceed = false,
                        Message = "FAQ not found",
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
        /// <summary>
        /// Get all FAQs
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/Dashboard/FAQ/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<FAQViewModel>>))]
        public async Task<IActionResult> GetAllFAQAsync()
        {
            List<FAQViewModel> FAQs = await fAQManager.GetAllFAQAsync();
            if (FAQs.Count > 0)
            {
                return Ok(new APIResult<List<FAQViewModel>>()
                {
                    Data = FAQs,
                    IsSucceed = true,
                    Message = "Get all FAQ",
                    StatusCode = 200
                });
            }
            else
            {
                return Ok(new APIResult<List<FAQViewModel>>()
                {

                    IsSucceed = false,
                    Message = "No FAQs found",
                    StatusCode = 200
                });
            }
        }
    }
}
