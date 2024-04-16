using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class PropertyExtansions
    {
        public static Models.Property ToPropertyModel (this AddNewPropertyViewModel viewModel)
        {
            return new Models.Property
            { 
                Location = viewModel.Location,
                UnitPrice = viewModel.UnitPrice,
                Description = viewModel.Description,
                MaintenanceCost = viewModel.MaintenanceCost,
                TransactionFees = viewModel.TransactionFees,
                NumberOfShares = viewModel.NumberOfShares,
                MaintenaceInstallment = viewModel.MaintenaceInstallment,
                SharePrice = viewModel.SharePrice,
                AnnualRentalYield = viewModel.AnnualRentalYield,
                AnnualPriceAppreciation = viewModel.AnnualPriceAppreciation,
                DownPayment = viewModel.DownPayment,
                MonthlyInstallment = viewModel.MonthlyInstallment,
                NumberOfYears = viewModel.NumberOfYears,
                MinOfShares = viewModel.MinNumberOfShares,
                DeliveryInstallment = viewModel.DeliveryInstallment,
                Type = viewModel.Type,
                GovernorateId = viewModel.GovernorateId,
                Status = viewModel.Status,
                CityId = viewModel.CityId,
                LastModificationDate = DateTime.Now
            };
        }
        public static Expression<Func<Models.Property, PropertyViewModelInListView>> ToPropertyViewModelInListExpression(bool isAdmin)
        {
            return i => new PropertyViewModelInListView
            {
                Address = i.Location,
                ProjectedRentalYield = i.AnnualRentalYield,
                ProjectedAnnualReturn = i.AnnualRentalYield + i.AnnualPriceAppreciation,
                City = i.City.NameEn,
                ImageUrl = i.PropertyImages.Select(image => image.ImageUrl).FirstOrDefault(),
                PropertyId = i.Id,
                AvailableTokens = i.AvailableShares,
                TokenPrice = i.SharePrice,
                IsDeleted = isAdmin ? i.IsDeleted : null,
            };
        }
        public static Expression<Func<Models.Property, PropertyDetailsViewModelForAdmin>> ToPropertyDetailsViewModelForAdminExpression()
        {
            return property => new PropertyDetailsViewModelForAdmin
            {
                PropertyId = property.Id,
                CityId = property.CityId, 
                TransactionFees = property.TransactionFees,
                TransactionFeesNumerical = property.TransactionFees != null ? property.TransactionFees * property.UnitPrice : null,
                Description = property.Description,
                AnnualPriceAppreciation = property.AnnualPriceAppreciation,
                AnnualRentalYield = property.AnnualRentalYield,
                AvailableShares = property.AvailableShares,
                City = property.City.NameEn,
                DeliveryInstallment = property.DeliveryInstallment,
                DownPayment = property.DownPayment,
                Governorate = property.Governorate.NameEn,
                Location = property.Location,
                MaintenaceInstallment = property.MaintenaceInstallment,
                MaintenanceCost = property.MaintenanceCost,
                MaintenanceCostNumerical = property.MaintenanceCost != null ? property.MaintenanceCost * property.UnitPrice : null,
                MonthlyInstallment = property.MonthlyInstallment,
                ProjectedAnnualReturn = property.AnnualRentalYield + property.AnnualPriceAppreciation,
                Type = property.Type,
                NumberOfShares = property.NumberOfShares, 
                MinNumberOfShares = property.MinOfShares,
                NumberOfYears = property.NumberOfYears,
                SharePrice = property.SharePrice,
                UnitPrice = property.UnitPrice,
                PendingShares = property.UsedShares,
                Status = property.Status,
                IsDeleted = property.IsDeleted,
                GovernorateId = property.GovernorateId,
                PropertyImages = property.PropertyImages.Select(propertyImage => new PropertyImageViewModelforAdmin()
                {
                    ImageUrl = propertyImage.ImageUrl,
                    Id = propertyImage.Id,
                }).ToList(),
                Facilities = property.PropertyFacilities.Select(propertyFacility => new PropertyFacilityViewModelForAdmin()
                {
                    FacilityId = propertyFacility.FacilityId,
                    SVG = propertyFacility.Facility.SVG,
                    Description = propertyFacility.Description,
                    PropertyFacilityId = propertyFacility.Id,
                }).ToList(),
            };
        }
        public static Expression<Func<Models.Property, PropertyDetailsViewModelForUser>> ToPropertyDetailsViewModelForUserExpression()
        {
            return i => new PropertyDetailsViewModelForUser
            {
                PropertyId = i.Id,
                TransactionFees = i.TransactionFees,
                TransactionFeesNumerical = i.TransactionFees != null ? i.TransactionFees * i.UnitPrice : null,
                Description = i.Description,
                AnnualPriceAppreciation = i.AnnualPriceAppreciation,
                AnnualRentalYield = i.AnnualRentalYield,
                AvailableShares = i.AvailableShares,
                City = i.City.NameEn,
                DeliveryInstallment = i.DeliveryInstallment,
                DownPayment = i.DownPayment,
                Governorate = i.Governorate.NameEn,
                Location = i.Location,
                MaintenaceInstallment = i.MaintenaceInstallment,
                MaintenanceCost = i.MaintenanceCost,
                MaintenanceCostNumerical = i.MaintenanceCost != null ? i.MaintenanceCost * i.UnitPrice : null,
                MonthlyInstallment = i.MonthlyInstallment,
                ProjectedAnnualReturn = i.AnnualRentalYield + i.AnnualPriceAppreciation,
                Type = i.Type,
                NumberOfShares = i.AvailableShares,
                MinNumberOfShares = i.MinOfShares,
                NumberOfYears = i.NumberOfYears,
                SharePrice = i.SharePrice,
                UnitPrice = i.UnitPrice,
                UsedShares = i.UsedShares,
                Status = i.Status,
                PropertyImages = i.PropertyImages.Select(image => image.ImageUrl).ToList(),
                PropertyFacilities = i.PropertyFacilities.Select(propertyFacility => new PropertyFacilityViewModelForUser()
                {
                    SVG = propertyFacility.Facility.SVG,
                    Description = propertyFacility.Description,
                }).ToList(),
            };
        }
        public static PropertyImageViewModelforAdmin ToPropertyImageViewModelForAdmin(this PropertyImage propertyImage)
        {
            return new PropertyImageViewModelforAdmin()
            {
                ImageUrl = propertyImage.ImageUrl,
                Id = propertyImage.Id,
            };
        }
    }
}
