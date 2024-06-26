﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                Title = viewModel.Location,
                Description = viewModel.Description,
                MaintenanceCost = viewModel.MaintenanceCost,
                TransactionFees = viewModel.TransactionFees,
                NumberOfShares = viewModel.NumberOfShares,
                MaintenaceInstallment = viewModel.MaintenaceInstallment,
                SharePrice = viewModel.SharePrice,
                AnnualPriceAppreciation = viewModel.AnnualPriceAppreciation,
                DownPayment = viewModel.DownPayment,
                MonthlyInstallment = viewModel.MonthlyInstallment,
                NumberOfYears = viewModel.NumberOfYears,
                MinOfShares = viewModel.MinNumberOfShares,
                DeliveryInstallment = viewModel.DeliveryInstallment,
                Type = viewModel.Type,
                GovernorateId = viewModel.GovernorateId,
                CityId = viewModel.CityId,
                LastModificationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"))
            };
        }
        public static Expression<Func<Models.Property, PropertyViewModelInListView>> ToPropertyViewModelInListExpression(bool isAdmin)
        {
            return i => new PropertyViewModelInListView
            {
                Address = i.Title,
                ProjectedRentalYield = i.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault(),
                ProjectedAnnualReturn = i.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault() + i.AnnualPriceAppreciation,
                City = i.City.NameEn,
                ImageUrl = i.PropertyImages.Select(image => image.ImageUrl).FirstOrDefault(),
                PropertyId = i.Id,
                AvailableTokens = i.AvailableShares, 
                TokenPrice = i.SharePrice,
                StatusId = i.PropertyStatus.Where(i => i.To == null).Select(i => i.Status).FirstOrDefault(),
                StatusName = i.PropertyStatus.Where(i => i.To == null).Select(i => i.Status).FirstOrDefault() == Status.Rented ? "Rented" :
                i.PropertyStatus.Where(i => i.To == null).Select(i => i.Status).FirstOrDefault() == Status.ReadyToMove ? "Ready To Move" : "Under Construction",
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
                TransactionFeesNumerical = property.TransactionFees != null ? property.TransactionFees * property.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault() : null,
                Description = property.Description,
                AnnualPriceAppreciation = property.AnnualPriceAppreciation, 
                AnnualRentalYield = property.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault(),
                AvailableShares = property.AvailableShares,
                City = property.City.NameEn,
                DeliveryInstallment = property.DeliveryInstallment,
                DownPayment = property.DownPayment,
                Governorate = property.Governorate.NameEn,
                Location = property.Title,
                MaintenaceInstallment = property.MaintenaceInstallment,
                MaintenanceCost = property.MaintenanceCost,
                MaintenanceCostNumerical = property.MaintenanceCost != null ? property.MaintenanceCost * property.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault() : null,
                MonthlyInstallment = property.MonthlyInstallment,
                ProjectedAnnualReturn = property.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault() + property.AnnualPriceAppreciation,
                Type = property.Type,
                NumberOfShares = property.NumberOfShares, 
                MinNumberOfShares = property.MinOfShares,
                NumberOfYears = property.NumberOfYears,
                SharePrice = property.SharePrice,
                UnitPrice = property.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault(),
                PendingShares = property.UsedShares,
                Status = property.PropertyStatus.Where(i => i.To == null).Select(i => i.Status).FirstOrDefault(),
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
                    Number = propertyFacility.Number
                }).OrderBy(i => i.Number).ToList(),
            };
        }
        public static Expression<Func<Models.Property, PropertyDetailsViewModelForUser>> ToPropertyDetailsViewModelForUserExpression()
        {
            return i => new PropertyDetailsViewModelForUser
            {
                PropertyId = i.Id,
                TransactionFees = i.TransactionFees,
                TransactionFeesNumerical = i.TransactionFees != null ? i.TransactionFees * i.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault() : null,
                Description = i.Description,
                AnnualPriceAppreciation = i.AnnualPriceAppreciation,
                AnnualRentalYield = i.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault(),
                AvailableShares = i.AvailableShares,
                City = i.City.NameEn,
                DeliveryInstallment = i.DeliveryInstallment,
                DownPayment = i.DownPayment,
                Governorate = i.Governorate.NameEn,
                Location = i.Title,
                MaintenaceInstallment = i.MaintenaceInstallment,
                MaintenanceCost = i.MaintenanceCost,
                MaintenanceCostNumerical = i.MaintenanceCost != null ? i.MaintenanceCost * i.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault() : null,
                MonthlyInstallment = i.MonthlyInstallment,
                ProjectedAnnualReturn = i.PropertyRentalYields.Where(i => i.To == null).Select(i => i.RentalYield).FirstOrDefault() + i.AnnualPriceAppreciation,
                Type = i.Type,
                NumberOfShares = i.NumberOfShares,
                MinNumberOfShares = i.MinOfShares,
                NumberOfYears = i.NumberOfYears,
                SharePrice = i.SharePrice,
                UnitPrice = i.PropertyUnitPrices.Where(i => i.To == null).Select(i => i.UnitPrice).FirstOrDefault(),
                UsedShares = i.UsedShares,
                IsDeleted = false,
                Status = i.PropertyStatus.Where(i => i.To == null).Select(i => i.Status).FirstOrDefault(),
                PropertyImages = i.PropertyImages.Select(image => image.ImageUrl).ToList(),
                PropertyFacilities = i.PropertyFacilities.Select(propertyFacility => new PropertyFacilityViewModelForUser()
                {
                    SVG = propertyFacility.Facility.SVG,
                    Description = propertyFacility.Description,
                    Number = propertyFacility.Number
                }).OrderBy(i => i.Number).ToList(),
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
