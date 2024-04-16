using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UpdatePropertyViewModel
    {
        [Required]
        public string PropertyId {  get; set; }
        public string Location { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        // Numerical
        public double? UnitPrice { get; set; }
        public string Description { get; set; }
        // Numerical and Percentage
        public double? MaintenanceCost { get; set; }
        // Numerical and percentage
        public double? TransactionFees { get; set; }
        public int? NumberOfShares { get; set; }
        public int? MinNumberOfShares {  get; set; }
        // Numerical
        public double? SharePrice { get; set; }
        // Percentage
        public double? AnnualRentalYield { get; set; }
        // Percentage
        public double? AnnualPriceAppreciation { get; set; }
        // Numerical
        public double? DownPayment { get; set; }
        // Numerical
        public double? MonthlyInstallment { get; set; }
        public int? NumberOfYears { get; set; }
        // Numerical
        public double? MaintenaceInstallment { get; set; }
        // Numerical 
        public double? DeliveryInstallment { get; set; }
        public Models.Type? Type { get; set; }
        public Status? Status { get; set; }
    }
}
