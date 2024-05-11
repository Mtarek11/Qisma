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
    public class FacilityManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<Facility>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        //public async Task AddNewFacilityAsync(FileViewModel SVG)
        //{
        //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + SVG.File.FileName;
        //    Facility facility = new()
        //    {
        //        SVG = uniqueFileName,
        //    };
        //    await AddAsync(facility);
        //    await unitOfWork.CommitAsync();
        //    FileStream fileStream = new FileStream(
        //    Path.Combine(
        //       Directory.GetCurrentDirectory(), "Content", "Facilities", uniqueFileName),
        //    FileMode.Create);
        //    await SVG.File.CopyToAsync(fileStream);
        //    fileStream.Position = 0;
        //    fileStream.Close();
        //}
        public async Task<List<FacilityViewModelForAdmin>> GetAllFacilitiesAsync()
        {
            List<FacilityViewModelForAdmin> facilities = await GetAll().Select(i => new FacilityViewModelForAdmin()
            {
                Id = i.Id,
                SVG = i.SVG
            }).ToListAsync();
            return facilities;
        }
    }
}
