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
    public class PropertyManager(LoftyContext _mydB, UnitOfWork _unitOfWork, CityManager _cityManager, PropertyImageManager _propertyImageManager) : MainManager<Property>(_mydB)
    {
        private readonly PropertyImageManager propertyImageManager = _propertyImageManager;
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
        public async Task<APIResult<string>> UpdatePropertyAsync(UpdatePropertyViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            if (viewModel.CityId != null && viewModel.GovernorateId != null)
            {
                bool checkCity = await cityManager.CheckCityAsync((int)viewModel.CityId, (int)viewModel.GovernorateId);
                if (!checkCity)
                {
                    aPIResult.Message = "City must be inside the governorate";
                    aPIResult.IsSucceed = false;
                    aPIResult.StatusCode = 400;
                    return aPIResult;
                }
            }
            Property property = new()
            {
                Id = viewModel.PropertyId
            };
            PartialUpdate(property);
            bool isUpdated = false;
            if (viewModel.Location != null)
            {
                property.Location = viewModel.Location;
                isUpdated = true;
            }
            if (viewModel.UnitPrice != null)
            {
                property.UnitPrice = (double)viewModel.UnitPrice;
                isUpdated = true;
            }
            if (viewModel.Description != null)
            {
                property.Description = viewModel.Description;
                isUpdated = true;
            }
            if (viewModel.MaintenanceCost != null)
            {
                property.MaintenanceCost = viewModel.MaintenanceCost;
                isUpdated = true;
            }
            if (viewModel.TransactionFees != null)
            {
                property.TransactionFees = viewModel.TransactionFees;
                isUpdated = true;
            }
            if (viewModel.NumberOfShares != null)
            {
                property.NumberOfShares = (int)viewModel.NumberOfShares;
                isUpdated = true;
            }
            if (viewModel.SharePrice != null)
            {
                property.SharePrice = (int)viewModel.SharePrice;
                isUpdated = true;
            }
            if (viewModel.AnnualRentalYield != null)
            {
                property.AnnualRentalYield = (double)viewModel.AnnualRentalYield;
                isUpdated = true;
            }
            if (viewModel.AnnualPriceAppreciation != null)
            {
                property.AnnualPriceAppreciation = (double)viewModel.AnnualPriceAppreciation;
                isUpdated = true;
            }
            if (viewModel.DownPayment != null)
            {
                property.DownPayment = (double)viewModel.DownPayment;
                isUpdated = true;
            }
            if (viewModel.MonthlyInstallment != null)
            {
                property.MonthlyInstallment = viewModel.MonthlyInstallment;
                isUpdated = true;
            }
            if (viewModel.DeliveryInstallment != null)
            {
                property.DeliveryInstallment = viewModel.DeliveryInstallment;
                isUpdated = true;
            }
            if (viewModel.Type != null)
            {
                property.Type = (Models.Type)viewModel.Type;
                isUpdated = true;
            }
            if (viewModel.Status != null)
            {
                property.Status = (Status)viewModel.Status;
                isUpdated = true;
            }
            if (isUpdated)
            {
                try
                {
                    await unitOfWork.CommitAsync();
                    aPIResult.Message = "Property updated";
                    aPIResult.StatusCode = 200;
                    aPIResult.IsSucceed = true;
                    return aPIResult;
                }
                catch (DbUpdateException)
                {
                    aPIResult.Message = "Property not found";
                    aPIResult.StatusCode = 400;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
            }
            else
            {
                aPIResult.Message = "Nothing to update";
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<PaginationViewModel<PropertyViewModelInListViewForUser>> GetAllPropertiesForUserAsync(int pageNumber, int pageSize, int? governorateId, int? cityId,
            Models.Type? propertyType, double? minUnitPrice, double? maxUnitPrice, double? minSharePrice, double? maxSharePrice)
        {
            int itemsToSkip = pageNumber * pageSize;
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
        public async Task<PropertyDetailsViewModelForAdmin> GetPropertyDetailsByIdForAdminAsync(int propertyId)
        {
            PropertyDetailsViewModelForAdmin property = await GetAll().Where(i => i.Id == propertyId).Select(i => i.ToPropertyDetailsViewModelForAdmin()).FirstOrDefaultAsync();
            return property;
        }
        public async Task<APIResult<string>> DeletePropertyAsync(int propertyId)
        {
            APIResult<string> aPIResult = new();
            List<string> propertyImages = await propertyImageManager.GetPropertyImagesUrlsAsync(propertyId);
            Property property = new()
            {
                Id = propertyId
            };
            try
            {
                Remove(property);
                await unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 400;
                aPIResult.Message = "Property not found";
                return aPIResult;
            }
            foreach(string image in propertyImages)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", image);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            aPIResult.IsSucceed = true;
            aPIResult.StatusCode = 200;
            aPIResult.Message = "Property deleted";
            return aPIResult;
        }
    }
}
