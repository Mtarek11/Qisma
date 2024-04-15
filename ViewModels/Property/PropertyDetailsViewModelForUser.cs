using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PropertyDetailsViewModelForUser
    {
        public string Governorate { get; set; }
        public  string City { get; set; }
        public string Location { get; set; }
        public double UnitPrice { get; set; }
        public string Description {  get; set; }
        public double? MaintenanceCost { get; set; }
        public double? MaintenanceCostNumerical {  get; set; }
        public double? TransactionFees { get; set; }
        public double? TransactionFeesNumerical {  get; set; }
        public int NumberOfShares { get; set; }
        public int MinNumberOfShares {  get; set; }
        public int AvailableShares { get; set; }
        public int UsedShares { get; set; }
        public double SharePrice { get; set; }
        public double AnnualRentalYield { get; set; }
        public double AnnualPriceAppreciation { get; set; }
        public double ProjectedAnnualReturn { get; set; }
        public double? DownPayment { get; set; }
        public double? MonthlyInstallment { get; set; }
        public int? NumberOfYears { get; set; }
        public double? MaintenaceInstallment { get; set; }
        public double? DeliveryInstallment { get; set; }
        public Models.Type Type { get; set; }
        public Status Status {  get; set; }
        public List<PropertyFacilityViewModelForUser> PropertyFacilities { get; set; } 
        public List<string> PropertyImages { get; set; }
    }
}
