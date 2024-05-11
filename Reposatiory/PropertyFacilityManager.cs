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
        public async Task<APIResult<PropertyFacilityViewModelForAdmin>> AddFacilityToAPropertyAsync(AddPropertyFacilityViewModel viewModel, string propertyId)
        {
            APIResult<PropertyFacilityViewModelForAdmin> aPIResult = new();
            int number = await GetAll().CountAsync(i => i.PropertyId == propertyId);
            PropertyFacility propertyFacility = new()
            {
                FacilityId = (int)viewModel.FacilityId,
                Description = viewModel.Description,
                PropertyId = propertyId,
                Number = number + 1
            };
            try
            {
                await AddAsync(propertyFacility);
                await unitOfWork.CommitAsync();
                PropertyFacilityViewModelForAdmin propertyFacilityForAdmin = await GetAll()
                    .Where(i => i.Id == propertyFacility.Id).Select(i => new PropertyFacilityViewModelForAdmin()
                    {
                        FacilityId = i.FacilityId,
                        SVG = i.Facility.SVG,
                        Description = i.Description,
                        PropertyFacilityId = i.Id,
                    }).FirstOrDefaultAsync();
                aPIResult.Data = propertyFacilityForAdmin;
                aPIResult.Message = "Facility added";
                aPIResult.IsSucceed = true;
                aPIResult.StatusCode = 200;
                return aPIResult;
            }
            catch (DbUpdateException ex)
            {
                aPIResult.Message = ex.InnerException.Message.Contains("FacilityId") ? "Facility not found" : "Property not found";
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 404;
                return aPIResult;
            }
        }
        public async Task<bool> DeleteFacilityFromAPropertyAsync(int propertyFacilityId)
        {
            PropertyFacility deletedFacility = await GetAll().Where(i => i.Id == propertyFacilityId).Select(i => new PropertyFacility()
            {
                Id = i.Id,
                PropertyId = i.PropertyId,
                Number = i.Number
            }).FirstOrDefaultAsync();
            if (deletedFacility != null)
            {
                List<PropertyFacility> modifiedFacilities = await GetAll().Where(i => i.PropertyId == deletedFacility.PropertyId && i.Number > deletedFacility.Number).Select(i => new PropertyFacility()
                {
                    Id = i.Id,
                    Number = i.Number,
                }).ToListAsync();
                if (modifiedFacilities.Count > 0)
                {
                    foreach(PropertyFacility modifiedFaciloty in modifiedFacilities)
                    {
                        PartialUpdate(modifiedFaciloty);
                        modifiedFaciloty.Number--;
                    }
                }
                Remove(deletedFacility);
                await unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<APIResult<string>> UpdatePropertyFacilityNumberAsync(List<int> propertyFacilitiesIds)
        {
            APIResult<string> aPIResult = new();
            if (propertyFacilitiesIds.Count > 0)
            {
                List<PropertyFacility> oldPropertyFacilities = await GetAll().Where(i => propertyFacilitiesIds.Contains(i.Id)).Select(i => new PropertyFacility()
                {
                    Id=i.Id,
                    Number = i.Number,
                }).ToListAsync();
                if (oldPropertyFacilities.Count != propertyFacilitiesIds.Count)
                {
                    aPIResult.Message = "Property facilities not found";
                    aPIResult.StatusCode = 404;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                int number = 0;
                foreach (int propertyFacilityId in propertyFacilitiesIds)
                {
                    PropertyFacility propertyFacility = oldPropertyFacilities.Where(i => i.Id == propertyFacilityId).FirstOrDefault();
                    PartialUpdate(propertyFacility);
                    number++;
                    propertyFacility.Number = number;
                }
                await unitOfWork.CommitAsync();
                aPIResult.Message = "FAQ updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            else
            {
                aPIResult.StatusCode = 400;
                aPIResult.Message = "Nothing to update";
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
    }
}
