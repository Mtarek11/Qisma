﻿using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddNewPropertyViewModel
    {
        // Location here means title, but we will leave it due to frontend developer requierments
        [Required]
        public string Location { get; set; }
        [Required]
        public int GovernorateId {  get; set; }
        [Required] 
        public int CityId {  get; set; }
        // Numerical
        [Required, Range(1000000, double.MaxValue, ErrorMessage = "Unit price must be at least 1,000,000.")] 
        public double UnitPrice { get; set; }
        [Required]
        public string Description {  get; set; }
        // Numerical and Percentage
        public double? MaintenanceCost { get; set; } 
        // Numerical and percentage
        public double? TransactionFees { get; set; }
        [Required]
        public int NumberOfShares { get; set; }
        [Required]
        public int MinNumberOfShares { get; set; }
        // Numerical
        [Required, Range(50000, double.MaxValue, ErrorMessage = "Share price must be at least 50,000.")]
        public double SharePrice { get; set; }
        // Percentage
        [Required]
        public double AnnualRentalYield { get; set; }
        // Percentage
        [Required]
        public double AnnualPriceAppreciation { get; set; }
        // Numerical
        public double? DownPayment { get; set; }
        // Numerical
        public double? MonthlyInstallment { get; set; }
        public int? NumberOfYears { get; set; }
        // Numerical
        public double? MaintenaceInstallment { get; set; }
        // Numerical 
        public double? DeliveryInstallment { get; set; }  
        [Required]
        [Range(1, 2, ErrorMessage = "Invalid Type value.")]
        public Models.Type Type { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Invalid Status value.")]
        public Status Status {  get; set; }
        public List<AddPropertyFacilityViewModel> Facilities {  get; set; }
        [Required, AllowedImageExtensions]
        public IFormFileCollection PropertyImages {  get; set; }
    }
}
