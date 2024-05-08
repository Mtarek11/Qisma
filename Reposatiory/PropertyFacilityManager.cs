using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Reposatiory
{
    public class PropertyFacilityManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<PropertyFacility>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        // Need to be refactore due to NUMBER
        //public async Task<APIResult<PropertyFacilityViewModelForAdmin>> AddFacilityToAPropertyAsync(AddPropertyFacilityViewModel viewModel, string propertyId)
        //{
        //    APIResult<PropertyFacilityViewModelForAdmin> aPIResult = new();
        //    PropertyFacility propertyFacility = new()
        //    {
        //        FacilityId = (int)viewModel.FacilityId,
        //        Description = viewModel.Description,
        //        PropertyId = propertyId,
        //        Number = (int)viewModel.Number
        //    };
        //    try
        //    {
        //        await AddAsync(propertyFacility);
        //        await unitOfWork.CommitAsync();
        //        PropertyFacilityViewModelForAdmin propertyFacilityForAdmin = await GetAll()
        //            .Where(i => i.Id == propertyFacility.Id).Select(i => new PropertyFacilityViewModelForAdmin()
        //            {
        //                FacilityId = i.FacilityId,
        //                SVG = i.Facility.SVG,
        //                Description = i.Description,
        //                PropertyFacilityId = i.Id,
        //            }).FirstOrDefaultAsync();
        //        aPIResult.Data = propertyFacilityForAdmin; 
        //        aPIResult.Message = "Facility added";
        //        aPIResult.IsSucceed = true;
        //        aPIResult.StatusCode = 200;
        //        return aPIResult;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        aPIResult.Message = ex.InnerException.Message.Contains("FacilityId") ? "Facility not found" : "Property not found";
        //        aPIResult.IsSucceed = false;
        //        aPIResult.StatusCode = 404;
        //        return aPIResult;
        //    }
        //}
        public async Task<bool> DeleteFacilityFromAPropertyAsync(int propertyFacilityId)
        {
            PropertyFacility propertyFacility = new()
            {
                Id = propertyFacilityId
            };
            try
            {
                Remove(propertyFacility);
                await unitOfWork.CommitAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
