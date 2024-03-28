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
using static System.Net.Mime.MediaTypeNames;

namespace Reposatiory
{
    public class PropertyManager(LoftyContext _mydB, UnitOfWork _unitOfWork, CityManager _cityManager) : MainManager<Property>(_mydB)
    {
        private readonly CityManager cityManager = _cityManager;
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task<APIResult<int>> AddNewPropertyAsync(AddNewPropertyViewModel viewModel)
        {
            APIResult<int> aPIResult = new APIResult<int>();
            bool checkCity = await cityManager.CheckCityAsync(viewModel.CityId, viewModel.GovernorateId);
            if (!checkCity)
            {
                aPIResult.Message = "City must be inside the governorate";
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 400;
                return aPIResult;
            }
            Property property = viewModel.ToPropertyModel();
            if (viewModel.Facilities != null)
            {
                foreach (AddPropertyFacilityViewModel facility in viewModel.Facilities)
                {
                    PropertyFacility propertyFacility = new()
                    {
                        FacilityId = facility.FacilityId,
                        Description = facility.Description,
                    };
                    property.PropertyFacilities.Add(propertyFacility);
                }
            }
            try
            {
                await AddAsync(property);
                await unitOfWork.CommitAsync();
                aPIResult.Data = property.Id;
                aPIResult.Message = "Property added";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            catch(DbUpdateException ex)
            {
                aPIResult.Message = ex.InnerException.Message;
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<PaginationViewModel<PropertyViewModelInListViewForUser>> GetAllPropertiesForUserAsync(int pageNumber, int pageSize, int? governorateId, int? cityId,
            Models.Type? propertyType, double? minUnitPrice, double? maxUnitPrice, double? minSharePrice, double? maxSharePrice)
        {
            int itemsToSkip = pageNumber* pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            IQueryable<Property> query = GetAll().Where(i => !i.IsDeleted);
            if (governorateId != null) 
            {
                query = query.Where(i => i.GovernorateId == governorateId);
            }
            if (cityId != null)
            {
                query = query.Where(i => i.CityId == cityId);
            }
            if (propertyType != null)
            {
                query = query.Where(i => i.Type == propertyType);
            }
            if (minUnitPrice != null)
            {
                query = query.Where(i => i.UnitPrice >= minUnitPrice);
            }
            if (maxUnitPrice != null)
            {
                query = query.Where(i => i.UnitPrice <= maxUnitPrice);
            }
            if (minSharePrice != null)
            {
                query = query.Where(i => i.SharePrice >= minSharePrice);
            }
            if (maxSharePrice != null)
            {
                query = query.Where(i => i.SharePrice <= maxSharePrice);
            }
            List<PropertyViewModelInListViewForUser> properties = await query.Select(i => i.ToPropertyViewModelInListForUser()).Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (properties.Count > 0)
            {
                int totalItems = await query.CountAsync();
                totalItemsNumber = totalItems;
                totalPageNumbers = (int)Math.Ceiling((double)totalItems / 10);
            }
            PaginationViewModel<PropertyViewModelInListViewForUser> paginationViewModel = new()
            {
                ItemsList = properties,
                TotalPageNumbers = totalPageNumbers,
                TotalItemsNumber = totalItemsNumber
            };
            return paginationViewModel;
        }
        public async Task<PropertyDetailsViewModelForUser> GetPropertyDetailsByIdForUserAsync(int propertyId)
        {
            PropertyDetailsViewModelForUser property = await GetAll().Where(i => i.Id == propertyId).Select(i => i.ToPropertyDetailsViewModelForUser()).FirstOrDefaultAsync();
            return property;
        }

    }
}
