using Microsoft.EntityFrameworkCore;
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
        public async Task<APIResult<PropertyFacilityViewModelForAdmin>> AddFacilityToAPropertyAsync(AddPropertyFacilityViewModel viewModel, int propertyId)
        {
            APIResult<PropertyFacilityViewModelForAdmin> aPIResult = new();
            PropertyFacility propertyFacility = new()
            {
                FacilityId = viewModel.FacilityId,
                Description = viewModel.Description,
                PropertyId = propertyId,
            };
            try
            {
                await AddAsync(propertyFacility);
                await unitOfWork.CommitAsync();
                aPIResult.Data = propertyFacility.ToPropertyFacilityViewModelForAdmin();
                aPIResult.Message = "Facility added";
                aPIResult.IsSucceed = true;
                aPIResult.StatusCode = 200;
                return aPIResult;
            }
            catch (DbUpdateException ex)
            {
                aPIResult.Message = ex.InnerException.Message.Contains("FacilityId") ? "Facility not found" : "Property not found";
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 400;
                return aPIResult;
            }
        }
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
