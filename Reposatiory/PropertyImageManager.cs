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
    public class PropertyImageManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<PropertyImage>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task<List<string>> GetPropertyImagesUrlsAsync(int propertyId)
        {
            List<string> propertyImages = await GetAll().Where(i => i.PropertyId == propertyId).Select(i => i.ImageUrl).ToListAsync();
            return propertyImages;
        }
        public async Task<bool> AddPropertyImagesAsync(AddPropertyImagesViewModel viewModel)
        {
            foreach (IFormFile formFile in viewModel.Images)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                PropertyImage propertyImage = new()
                {
                    PropertyId = viewModel.PropertyId,
                    ImageUrl = uniqueFileName
                };
                try
                {
                    await AddAsync(propertyImage);
                    await unitOfWork.CommitAsync();
                    FileStream fileStream = new FileStream(
                    Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", uniqueFileName),
                    FileMode.Create);
                    await formFile.CopyToAsync(fileStream);
                    fileStream.Position = 0;
                    fileStream.Close();
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<APIResult<PropertyImageViewModelforAdmin>> AddPropertyImageAsync(AddPropertyImagesViewModel viewModel)
        {
            APIResult<PropertyImageViewModelforAdmin> aPIResult = new();
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Images[0].FileName;
            PropertyImage propertyImage = new()
            {
                PropertyId = viewModel.PropertyId,
                ImageUrl = uniqueFileName
            };
            try
            {
                await AddAsync(propertyImage);
                await unitOfWork.CommitAsync();
                FileStream fileStream = new FileStream(
                Path.Combine(
                   Directory.GetCurrentDirectory(), "Content", "Images", uniqueFileName),
                FileMode.Create);
                await viewModel.Images[0].CopyToAsync(fileStream);
                fileStream.Position = 0;
                fileStream.Close();
                aPIResult.Data = propertyImage.ToPropertyImageViewModelForAdmin();
                aPIResult.Message = "Imaged added";
                aPIResult.IsSucceed = true;
                aPIResult.StatusCode = 200;
                return aPIResult;
            }
            catch (DbUpdateException)
            {
                aPIResult.Message = "Property not found";
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 400;
                return aPIResult;
            }
        }
        public async Task<bool> DeletePropertyImageAsync(int imageId)
        {
            string oldStockImage = await GetAll().Where(i => i.Id == imageId).Select(i => i.ImageUrl).FirstOrDefaultAsync();
            if (oldStockImage != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", oldStockImage);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                PropertyImage deletedImage = new()
                {
                    Id = imageId,
                };
                Remove(deletedImage);
                await unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<string>> GetAllPropertyImagesAsync(int propertyId)
        {
            List<string> propertyImages = await GetAll().Where(i => i.PropertyId == propertyId).Select(i => i.ImageUrl).ToListAsync();
            return propertyImages;
        }
    }
}
