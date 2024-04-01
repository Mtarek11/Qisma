﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                DeliveryInstallment = viewModel.DeliveryInstallment,
                Type = viewModel.Type,
                GovernorateId = viewModel.GovernorateId,
                Status = viewModel.Status,
                CityId = viewModel.CityId
            };
        }
        public static PropertyViewModelInListViewForUser ToPropertyViewModelInListForUser (this Models.Property property)
        { 
            return new PropertyViewModelInListViewForUser
            {
                Address = property.Location,
                ProjectedRentalYield = property.AnnualRentalYield,
                ProjectedAnnualReturn = property.AnnualRentalYield + property.AnnualPriceAppreciation,
                City = property.City.NameEn,
                ImageUrl = property.PropertyImages.Select(i => i.ImageUrl).FirstOrDefault(),
                PropertyId = property.Id,
                AvailableTokens = property.AvailableShares,
                TokenPrice = property.SharePrice
            };
        }
        public static PropertyDetailsViewModelForAdmin ToPropertyDetailsViewModelForAdmin(this Models.Property property)
        {
            return new PropertyDetailsViewModelForAdmin
            {
                PropertyId = property.Id,
                TransactionFeesPercentage = property.TransactionFees,
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
                MaintenanceCostPercentage = property.MaintenanceCost,
                MaintenanceCostNumerical = property.MaintenanceCost != null ? property.MaintenanceCost * property.UnitPrice : null,
                MonthlyInstallment = property.MonthlyInstallment,
                ProjectedAnnualReturn = property.AnnualRentalYield + property.AnnualPriceAppreciation,
                Type = property.Type,
                NumberOfShares = property.NumberOfShares,
                NumberOfYears = property.NumberOfYears,
                SharePrice = property.SharePrice,
                UnitPrice = property.UnitPrice,
                UsedShares = property.UsedShares,
                Status = property.Status,
                PropertyImages = property.PropertyImages.Select(i => i.ToPropertyImageViewModelForAdmin()).ToList(),
                PropertyFacilities = property.PropertyFacilities.Select(i => i.ToPropertyFacilityViewModelForAdmin()).ToList(),
            };
        }
        public static PropertyFacilityViewModelForAdmin ToPropertyFacilityViewModelForAdmin(this PropertyFacility propertyFacility)
        {
            return new PropertyFacilityViewModelForAdmin()
            {
                SVG = propertyFacility.Facility.SVG,
                Describtion = propertyFacility.Description,
                PropertyFacilityId = propertyFacility.Id,
            };
        }
        public static PropertyImageViewModelforAdmin ToPropertyImageViewModelForAdmin(this PropertyImage propertyImage)
        {
            return new PropertyImageViewModelforAdmin()
            {
                ImageUrl = propertyImage.ImageUrl,
                PropertyId = propertyImage.PropertyId,
            };
        }
        public static PropertyDetailsViewModelForUser ToPropertyDetailsViewModelForUser(this Models.Property property)
        {
            return new PropertyDetailsViewModelForUser
            {
                TransactionFeesPercentage = property.TransactionFees,
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
                MaintenanceCostPercentage = property.MaintenanceCost,
                MaintenanceCostNumerical = property.MaintenanceCost != null ? property.MaintenanceCost * property.UnitPrice : null,
                MonthlyInstallment = property.MonthlyInstallment,
                ProjectedAnnualReturn = property.AnnualRentalYield + property.AnnualPriceAppreciation,
                Type = property.Type,
                NumberOfShares = property.NumberOfShares,
                NumberOfYears = property.NumberOfYears,
                SharePrice = property.SharePrice,
                UnitPrice = property.UnitPrice,
                UsedShares = property.UsedShares,
                Status = property.Status,
                PropertyImages = property.PropertyImages.Select(i => i.ImageUrl).ToList(),
                PropertyFacilities = property.PropertyFacilities.Select(i => i.ToPropertyFacilityViewModelForUser()).ToList(),
            };
        }
        public static PropertyFacilityViewModelForUser ToPropertyFacilityViewModelForUser(this PropertyFacility propertyFacility)
        {
            return new PropertyFacilityViewModelForUser
            {
                SVG = propertyFacility.Facility.SVG,
                Describtion = propertyFacility.Description,
            };
        }
    }
}