using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Reposatiory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace Lofty.Controllers
{
    /// <summary>
    /// Blog apis
    /// </summary>
    /// <param name="_blogManager"></param>
    public class BlogController(BlogManager _blogManager) : ControllerBase
    {
        private readonly BlogManager blogManager = _blogManager;
        /// <summary>
        /// Add blog
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("api/Dashboard/Blog/Add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> AddBlogAsync([FromForm] AddBlogViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await blogManager.AddBlogAsync(viewModel);
                return Ok(new APIResult<string>()
                {
                    IsSucceed = true,
                    Message = "Blog added",
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
        /// Get all blogs
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/Blog/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<List<Blog>>))]
        public async Task<IActionResult> GetAllBlogsAsync()
        {
            List<Blog> blogs = await blogManager.GetAllBlogsAsync();
            if (blogs.Count > 0)
            {
                return Ok(new APIResult<List<Blog>>()
                {
                    Data = blogs,
                    IsSucceed = true,
                    Message = "Blogs",
                    StatusCode = 200
                });
            }
            else
            {
                return Ok(new APIResult<List<Blog>>()
                {
                    IsSucceed = false,
                    Message = "No blogs found",
                    StatusCode = 200
                });
            }
        }
        /// <summary>
        /// Update blog
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("api/Dashboard/Blog/Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> UpdateBlogAsync([FromForm] UpdateBlogViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                APIResult<string> result = await blogManager.UpdateBlogAsync(viewModel);
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
        /// Delete blog
        /// </summary>
        /// <param name="BlogId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("api/Dashboard/Blog/Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResult<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResult<string>))]
        public async Task<IActionResult> DeleteBlogAsync([FromQuery, Required] int BlogId)
        {
            if (ModelState.IsValid)
            {
                bool checkDeletedOrNot = await blogManager.DeleteBlogAsync(BlogId);
                if (checkDeletedOrNot)
                {
                    return Ok(new APIResult<string>()
                    {
                        IsSucceed = true,
                        Message = "Blog deleted",
                        StatusCode = 200
                    });
                }
                else
                {
                    return NotFound(new APIResult<string>()
                    {
                        IsSucceed = false,
                        Message = "Blog not found",
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
