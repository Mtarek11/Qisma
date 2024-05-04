using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Reposatiory
{
    public class BlogManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<Blog>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task AddBlogAsync(AddBlogViewModel viewModel)
        {
            Blog blog = new()
            {
                Description = viewModel.Description,
                ImageUrl = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName,
                Link = viewModel.Link,
                Title = viewModel.Title
            };
            FileStream fileStream = new FileStream(
                    Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", blog.ImageUrl),
                    FileMode.Create);
            await viewModel.Image.CopyToAsync(fileStream);
            fileStream.Position = 0;
            fileStream.Close();
            await AddAsync(blog);
            await unitOfWork.CommitAsync();
        }
        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            List<Blog> blogs = await GetAll().AsNoTracking().ToListAsync();
            return blogs;
        }
        public async Task<APIResult<string>> UpdateBlogAsync(UpdateBlogViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            string oldImage = null;
            Blog blog = new()
            {
                Id = viewModel.Id
            };
            PartialUpdate(blog);
            bool isUpdated = false;
            if (viewModel.Link != null)
            {
                blog.Link = viewModel.Link;
                isUpdated = true;
            }
            if (viewModel.Title != null)
            {
                blog.Title = viewModel.Title;
                isUpdated = true;
            }
            if (viewModel.Description != null)
            {
                blog.Description = viewModel.Description;
                isUpdated = true;
            }
            if (viewModel.Image != null)
            {
                oldImage = await GetAll().Where(i => i.Id == viewModel.Id).Select(i => i.ImageUrl).FirstOrDefaultAsync();
                if (oldImage == null)
                {
                    aPIResult.Message = "Blog not found";
                    aPIResult.IsSucceed = false;
                    aPIResult.StatusCode = 404;
                    return aPIResult;
                }
                blog.ImageUrl = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                isUpdated = true;
            }
            if (isUpdated)
            {
                if (oldImage != null)
                {
                    FileStream fileStream = new FileStream(
                    Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", blog.ImageUrl),
                    FileMode.Create);
                    await viewModel.Image.CopyToAsync(fileStream);
                    fileStream.Position = 0;
                    fileStream.Close();
                }
                try
                {
                    await unitOfWork.CommitAsync();
                }
                catch (DbUpdateException)
                {
                    aPIResult.Message = "Blog not found";
                    aPIResult.StatusCode = 404;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                if (oldImage != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", oldImage);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                aPIResult.Message = "Blog updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            else
            {
                aPIResult.Message = "Nothing to update";
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<bool> DeleteBlogAsync(int blogId)
        {
            Blog blog = await GetAll().Where(i => i.Id == blogId).FirstOrDefaultAsync();
            if (blog != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", blog.ImageUrl);
                Remove(blog);
                await unitOfWork.CommitAsync();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
