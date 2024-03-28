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
    }
}
