using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using static iTextSharp.text.pdf.AcroFields;
using static System.Net.Mime.MediaTypeNames;

namespace Reposatiory
{
    public class PropertyManager(LoftyContext _mydB, UnitOfWork _unitOfWork, CityManager _cityManager,
        PropertyUnitPriceManager _propertyUnitPriceManager, PropertyRentalYieldManager _propertyRentalYieldManager, BuyTrackerManager _buyTrackerManager,
        PropertyStatusManager _propertyStatusManager) : MainManager<Models.Property>(_mydB)
    {
        private readonly BuyTrackerManager buyTrackerManager = _buyTrackerManager;
        private readonly LoftyContext mydB = _mydB;
        private readonly CityManager cityManager = _cityManager;
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        private readonly PropertyUnitPriceManager propertyUnitPriceManager = _propertyUnitPriceManager;
        private readonly PropertyRentalYieldManager propertyRentalYieldManager = _propertyRentalYieldManager;
        private readonly PropertyStatusManager propertyStatusManager = _propertyStatusManager;
        public async Task<APIResult<string>> AddNewPropertyAsync(AddNewPropertyViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            bool checkCity = await cityManager.CheckCityAsync(viewModel.CityId, viewModel.GovernorateId);
            if (!checkCity)
            {
                aPIResult.Message = "City must be inside the governorate";
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 400;
                return aPIResult;
            }
            Models.Property property = viewModel.ToPropertyModel();
            if (viewModel.Facilities.Count > 0)
            {
                int number = 0;
                foreach (AddPropertyFacilityViewModel facility in viewModel.Facilities)
                {
                    if (facility.FacilityId == null)
                    {
                        aPIResult.Message = "Facility requierd for all property facilities";
                        aPIResult.IsSucceed = false;
                        aPIResult.StatusCode = 400;
                        return aPIResult;
                    }
                    number++;
                    PropertyFacility propertyFacility = new()
                    {
                        FacilityId = (int)facility.FacilityId,
                        Description = facility.Description,
                        Number = number
                    };
                    property.PropertyFacilities.Add(propertyFacility);
                }
            }
            using (var transaction = await mydB.Database.BeginTransactionAsync())
            {
                PropertyUnitPrice propertyUnitPrice = new()
                {
                    From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                    UnitPrice = viewModel.UnitPrice
                };
                PropertyRentalYield propertyRentalYield = new()
                {
                    RentalYield = viewModel.AnnualRentalYield,
                    From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                };
                PropertyStatus propertyStatus = new()
                {
                    Status = viewModel.Status,
                    From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                };
                property.PropertyUnitPrices.Add(propertyUnitPrice);
                property.PropertyRentalYields.Add(propertyRentalYield);
                property.PropertyStatus.Add(propertyStatus);
                char randomChar1 = (char)('A' + new Random().Next(26));
                char randomChar2 = (char)('A' + new Random().Next(26));
                int randomNumber = new Random().Next(100000, 999999);
                string guid = randomChar1 + randomChar2 + randomNumber.ToString();
                property.Id = guid;
                try
                {
                    await AddAsync(property);
                    await unitOfWork.CommitAsync();
                    await transaction.CommitAsync();
                    aPIResult.Data = property.Id;
                    aPIResult.Message = "Property added";
                    aPIResult.StatusCode = 200;
                    aPIResult.IsSucceed = true;
                    return aPIResult;
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    if (ex.InnerException.Message.Contains("'FacilityId'"))
                    {
                        aPIResult.Message = "One of the facilities was not found in the db, please refresh page and try again";
                        aPIResult.StatusCode = 400;
                        aPIResult.IsSucceed = false;
                        return aPIResult;
                    }
                    else
                    {
                        aPIResult.Message = ex.InnerException.Message;
                        aPIResult.StatusCode = 400;
                        aPIResult.IsSucceed = false;
                        return aPIResult;
                    }
                }
            }
        }
        public async Task<APIResult<string>> UpdatePropertyAsync(UpdatePropertyViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            Models.Property property = await GetAll().Where(i => i.Id == viewModel.PropertyId).Select(i => new Models.Property()
            {
                Id = i.Id,
                UsedShares = i.UsedShares,
                PropertyFacilities = i.PropertyFacilities
            }).FirstOrDefaultAsync();
            if (property != null)
            {
                PartialUpdate(property);
                bool isUpdated = false;
                if (viewModel.AnnualRentalYield != null)
                {
                    PropertyRentalYield propertyRentalYield = await propertyRentalYieldManager.GetAll().Where(i => i.PropertyId == viewModel.PropertyId && i.To == null)
                       .Select(i => new PropertyRentalYield()
                       {
                           Id = i.Id,
                           To = i.To,
                           From = i.From
                       }).FirstOrDefaultAsync();
                    if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).Year - propertyRentalYield.From.Year <= 1)
                    {
                        aPIResult.Message = "Property rental yield can be updated one time a year";
                        aPIResult.StatusCode = 409;
                        aPIResult.IsSucceed = false;
                        return aPIResult;
                    }
                    else
                    {
                        propertyRentalYieldManager.PartialUpdate(propertyRentalYield);
                        propertyRentalYield.To = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                        PropertyRentalYield newPropertyRentalYield = new()
                        {
                            PropertyId = viewModel.PropertyId,
                            RentalYield = (double)viewModel.AnnualRentalYield,
                            From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                        };
                        await propertyRentalYieldManager.AddAsync(newPropertyRentalYield);
                        isUpdated = true;
                    }
                }
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
                    else
                    {
                        property.CityId = (int)viewModel.CityId;
                        property.GovernorateId = (int)viewModel.GovernorateId;
                        isUpdated = true;
                    }
                }
                if (viewModel.Location != null)
                {
                    property.Title = viewModel.Location;
                    isUpdated = true;
                }
                if (viewModel.UnitPrice != null)
                {
                    PropertyUnitPrice propertyUnitPrice = await propertyUnitPriceManager.GetAll().Where(i => i.PropertyId == viewModel.PropertyId && i.To == null)
                        .Select(i => new PropertyUnitPrice()
                        {
                            Id = i.Id,
                            To = i.To,
                        }).FirstOrDefaultAsync();
                    propertyUnitPriceManager.PartialUpdate(propertyUnitPrice);
                    propertyUnitPrice.To = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    PropertyUnitPrice newPropertyUnitPrice = new()
                    {
                        PropertyId = viewModel.PropertyId,
                        From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                        UnitPrice = (double)viewModel.UnitPrice
                    };
                    await propertyUnitPriceManager.AddAsync(newPropertyUnitPrice);
                    isUpdated = true;
                }
                if (viewModel.Status != null)
                {
                    PropertyStatus propertyStatus = await propertyStatusManager.GetAll().Where(i => i.PropertyId == viewModel.PropertyId && i.To == null).Select(i => new PropertyStatus()
                    {
                        Id = i.Id,
                        To = i.To
                    }).FirstOrDefaultAsync();
                    propertyStatusManager.PartialUpdate(propertyStatus);
                    propertyStatus.To = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    PropertyStatus newPropertyStatus = new()
                    {
                        PropertyId = viewModel.PropertyId,
                        From = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                        Status = (Status)viewModel.Status
                    };
                    await propertyStatusManager.AddAsync(newPropertyStatus);
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
                    if (viewModel.NumberOfShares < property.UsedShares)
                    {
                        aPIResult.Message = "Total number of shares cannot be less than pending shares, Cancell pending shares first";
                        aPIResult.StatusCode = 409;
                        aPIResult.IsSucceed = false;
                        return aPIResult;
                    }
                    property.NumberOfShares = (int)viewModel.NumberOfShares;
                    isUpdated = true;
                }
                if (viewModel.MinNumberOfShares != null)
                {
                    property.MinOfShares = (int)viewModel.MinNumberOfShares;
                    isUpdated = true;
                }
                if (viewModel.SharePrice != null)
                {
                    property.SharePrice = (int)viewModel.SharePrice;
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
                if (viewModel.NumberOfYears != null)
                {
                    property.NumberOfYears = viewModel.NumberOfYears;
                    isUpdated = true;
                }
                if (viewModel.MaintenaceInstallment != null)
                {
                    property.MaintenaceInstallment = viewModel.MaintenaceInstallment;
                    isUpdated = true;
                }
                if (isUpdated)
                {
                    property.LastModificationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    await unitOfWork.CommitAsync();
                    aPIResult.Message = "Property updated";
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
            else
            {
                aPIResult.Message = "Property not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<PaginationViewModel<PropertyViewModelInListView>> GetAllPropertiesForUserAsync(int pageNumber, int pageSize, int? governorateId, int? cityId,
            Models.Type? propertyType, double? minUnitPrice, double? minSharePrice, bool isAdmin)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            IQueryable<Models.Property> query;
            if (isAdmin)
            {
                query = GetAll();
            }
            else
            {
                query = GetAll().Where(i => !i.IsDeleted);
            }
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
                query = query.Where(i => i.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault() >= minUnitPrice);
            }
            if (minSharePrice != null)
            {
                query = query.Where(i => i.SharePrice >= minSharePrice);
            }
            List<PropertyViewModelInListView> properties = await query.OrderBy(i => i.Id).Select(PropertyExtansions.ToPropertyViewModelInListExpression(isAdmin)).Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (properties.Count > 0)
            {
                int totalItems = await query.CountAsync();
                totalItemsNumber = totalItems;
                totalPageNumbers = (int)Math.Ceiling((double)totalItems / pageSize);
            }
            PaginationViewModel<PropertyViewModelInListView> paginationViewModel = new()
            {
                ItemsList = properties,
                TotalPageNumbers = totalPageNumbers,
                TotalItemsNumber = totalItemsNumber
            };
            return paginationViewModel;
        }
        public async Task<APIResult<PropertyDetailsViewModelForUser>> GetPropertyDetailsByIdForUserAsync(string propertyId, string userId, bool isAdmin)
        {
            APIResult<PropertyDetailsViewModelForUser> aPIResult = new();
            if (isAdmin)
            {
                PropertyDetailsViewModelForUser property = await GetAll().Where(i => i.Id == propertyId)
                 .Select(PropertyExtansions.ToPropertyDetailsViewModelForUserExpression()).FirstOrDefaultAsync();
                aPIResult.Data = property;
                aPIResult.IsSucceed = true;
                aPIResult.StatusCode = 200;
                aPIResult.Message = "Get property details";
                return aPIResult;
            }
            else
            {
                PropertyDetailsViewModelForUser property = await GetAll().Where(i => i.Id == propertyId && i.IsDeleted == false)
                .Select(PropertyExtansions.ToPropertyDetailsViewModelForUserExpression()).FirstOrDefaultAsync();
                if (property != null && userId != null)
                {
                    bool buyerTracker = await buyTrackerManager.ProceedWithBuyAsync(userId, propertyId);
                    if (buyerTracker)
                    {
                        aPIResult.Data = property;
                        aPIResult.IsSucceed = true;
                        aPIResult.StatusCode = 200;
                        aPIResult.Message = "Get property details";
                        return aPIResult;
                    }
                    else
                    {
                        aPIResult.IsSucceed = false;
                        aPIResult.StatusCode = 401;
                        aPIResult.Message = "User not found";
                        return aPIResult;
                    }
                }
                else if (property != null && userId == null)
                {
                    aPIResult.Data = property;
                    aPIResult.IsSucceed = true;
                    aPIResult.StatusCode = 200;
                    aPIResult.Message = "Get property details";
                    return aPIResult;
                }
                else
                {
                    aPIResult.Message = "Property not found";
                    aPIResult.IsSucceed = false;
                    aPIResult.StatusCode = 404;
                    return aPIResult;
                }
            }
        }
        public async Task<PropertyDetailsViewModelForAdmin> GetPropertyDetailsByIdForAdminAsync(string propertyId)
        {
            PropertyDetailsViewModelForAdmin property = await GetAll().Where(i => i.Id == propertyId).Select(PropertyExtansions.ToPropertyDetailsViewModelForAdminExpression()).FirstOrDefaultAsync();
            return property;
        }
        public async Task<APIResult<string>> EnableAndDisablePropertyAsync(string propertyId)
        {
            APIResult<string> aPIResult = new();
            Models.Property property = await GetAll().Where(i => i.Id == propertyId).Select(i => new Models.Property()
            {
                Id = i.Id,
                IsDeleted = i.IsDeleted
            }).FirstOrDefaultAsync();
            if (property != null)
            {
                if (property.IsDeleted)
                {
                    PartialUpdate(property);
                    property.IsDeleted = false;
                    property.LastModificationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    await unitOfWork.CommitAsync();
                    aPIResult.IsSucceed = true;
                    aPIResult.StatusCode = 200;
                    aPIResult.Message = "Property enabled";
                    return aPIResult;
                }
                else
                {
                    PartialUpdate(property);
                    property.IsDeleted = true;
                    property.LastModificationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    await unitOfWork.CommitAsync();
                    aPIResult.IsSucceed = true;
                    aPIResult.StatusCode = 200;
                    aPIResult.Message = "Property disabled";
                    return aPIResult;
                }
            }
            else
            {
                aPIResult.IsSucceed = false;
                aPIResult.StatusCode = 404;
                aPIResult.Message = "Property not found";
                return aPIResult;
            }
        }
        public async Task<APIResult<OrderingPageViewModel>> GetOrderingPageAsync(string userId, string propertyId)
        {
            APIResult<OrderingPageViewModel> aPIResult = new();
            BuyTracker buyTracker = await buyTrackerManager.GetAll().Where(i => i.UserId == userId && i.PropertyId == propertyId).Select(i => new BuyTracker()
            {
                PropertyId = i.PropertyId,
                UserId = i.UserId,
                LastProceedDate = i.LastProceedDate,
            }).FirstOrDefaultAsync();
            if (buyTracker == null)
            {
                aPIResult.Message = "Cannot create order before going to property details page";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            Models.Property property = await GetAll().Where(i => i.Id == propertyId && i.IsDeleted == false).Select(i => new Models.Property()
            {
                Id = i.Id,
                AvailableShares = i.AvailableShares,
                SharePrice = i.SharePrice,
                MinOfShares = i.MinOfShares,
                TransactionFees = i.TransactionFees,
                DownPayment = i.DownPayment,
                MonthlyInstallment = i.MonthlyInstallment,
                NumberOfYears = i.NumberOfYears,
                MaintenaceInstallment = i.MaintenaceInstallment,
                DeliveryInstallment = i.DeliveryInstallment,
                LastModificationDate = i.LastModificationDate,
                NumberOfShares = i.NumberOfShares,
                PropertyUnitPrices = i.PropertyUnitPrices.Where(i => i.To == null).Select(P => new PropertyUnitPrice()
                {
                    UnitPrice = P.UnitPrice
                }).ToList()
            }).FirstOrDefaultAsync();
            if (property == null)
            {
                aPIResult.Message = "Property not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            if (buyTracker.LastProceedDate <= property.LastModificationDate)
            {
                aPIResult.Message = "Property updated, go to property details page again";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            OrderingPageViewModel orderingPage = new()
            {
                PropertyId = property.Id,
                SharePrice = property.SharePrice,
                TransactionFees = property.TransactionFees != null ? (property.TransactionFees * property.PropertyUnitPrices.Select(i => i.UnitPrice).FirstOrDefault()) / property.NumberOfShares : null,
                AvailableShares = property.AvailableShares,
                MinNumberOfShares = property.MinOfShares,
                DownPayment = property.DownPayment,
                MonthlyInstallment = property.MonthlyInstallment,
                NumberOfYears = property.NumberOfYears,
                MaintenaceInstallment = property.MaintenaceInstallment,
                DeliveryInstallment = property.DeliveryInstallment,
            };
            aPIResult.Data = orderingPage;
            aPIResult.Message = "Ordering page";
            aPIResult.StatusCode = 200;
            aPIResult.IsSucceed = true;
            return aPIResult;
        }
        public async Task<APIResult<OrderPreviewPageViewModel>> GetOrderPreviewAsync(string userId, string propertyId, int numberOfshares)
        {
            APIResult<OrderPreviewPageViewModel> aPIResult = new();
            BuyTracker buyTracker = await buyTrackerManager.GetAll().Where(i => i.UserId == userId && i.PropertyId == propertyId).Select(i => new BuyTracker()
            {
                PropertyId = i.PropertyId,
                UserId = i.UserId,
                LastProceedDate = i.LastProceedDate,
                User = new User()
                {
                    FirstName = i.User.FirstName,
                    MiddleName = i.User.MiddleName,
                    LastName = i.User.LastName,
                    Email = i.User.Email,
                    Address = i.User.Address,
                    PhoneNumber = i.User.PhoneNumber,
                    IdentityNumber = i.User.IdentityNumber
                }
            }).FirstOrDefaultAsync();
            if (buyTracker == null)
            {
                aPIResult.Message = "Cannot create order before going to property details page";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            Models.Property property = await GetAll().Where(i => i.Id == propertyId && i.IsDeleted == false).Select(i => new Models.Property()
            {
                Id = i.Id,
                Title = i.Title,
                Governorate = new Governorate()
                {
                    NameEn = i.Governorate.NameEn
                },
                City = new City()
                {
                    NameEn = i.City.NameEn
                },
                AvailableShares = i.AvailableShares,
                SharePrice = i.SharePrice,
                MinOfShares = i.MinOfShares,
                TransactionFees = i.TransactionFees,
                DownPayment = i.DownPayment,
                MonthlyInstallment = i.MonthlyInstallment,
                NumberOfYears = i.NumberOfYears,
                MaintenaceInstallment = i.MaintenaceInstallment,
                DeliveryInstallment = i.DeliveryInstallment,
                LastModificationDate = i.LastModificationDate,
                NumberOfShares = i.NumberOfShares,
                PropertyUnitPrices = i.PropertyUnitPrices.Where(i => i.To == null).Select(P => new PropertyUnitPrice()
                {
                    UnitPrice = P.UnitPrice
                }).ToList()
            }).FirstOrDefaultAsync();
            if (property == null)
            {
                aPIResult.Message = "Property not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            if (buyTracker.LastProceedDate <= property.LastModificationDate)
            {
                aPIResult.Message = "Property updated, go to property details page again";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            if (numberOfshares < property.MinOfShares)
            {
                aPIResult.Message = "Cannot insert less than min number of shares";
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            OrderingPageViewModel orderingPage = new()
            {
                PropertyId = property.Id,
                SharePrice = property.SharePrice,
                TransactionFees = property.TransactionFees != null ?
                ((property.TransactionFees * property.PropertyUnitPrices.Select(i => i.UnitPrice).FirstOrDefault()) / property.NumberOfShares) * numberOfshares : null,
                AvailableShares = property.AvailableShares,
                MinNumberOfShares = property.MinOfShares,
                DownPayment = property.DownPayment / numberOfshares,
                MonthlyInstallment = property.MonthlyInstallment / numberOfshares,
                NumberOfYears = property.NumberOfYears,
                MaintenaceInstallment = property.MaintenaceInstallment / numberOfshares,
                DeliveryInstallment = property.DeliveryInstallment / numberOfshares,
            };
            UserInformationForOrderPreviewViewModel userInformation = new()
            {
                Email = buyTracker.User.Email,
                IdNumber = buyTracker.User.IdentityNumber,
                PhoneNumber = buyTracker.User.PhoneNumber,
                UserAddress = buyTracker.User.Address,
                UserName = buyTracker.User.FirstName + " " + buyTracker.User.MiddleName + " " + buyTracker.User.LastName
            };
            OrderPreviewPageViewModel orderPreviewPage = new()
            {
                OrderingPage = orderingPage,
                UserInformation = userInformation,
                PropertyLocation = property.Governorate.NameEn + ", " + property.City.NameEn,
                PropertyName = property.Title
            };
            aPIResult.Data = orderPreviewPage;
            aPIResult.Message = "Order preview page";
            aPIResult.StatusCode = 200;
            aPIResult.IsSucceed = true;
            return aPIResult;
        }
    }
}
