using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Property
    {
        public int Id { get; set; }
        public virtual Governorate Governorate { get; set; }
        public int GovernorateId { get; set; }
        public virtual City City { get; set; }
        public int CityId { get; set; }
        public string Location { get; set; }
        public string Description {  get; set; }
        // Numerical
        public double UnitPrice { get; set; }
        // Numerical and Percentage
        public double? MaintenanceCost { get; set; }
        // Numerical and Percentage
        public double? TransactionFees { get; set; }
        public int NumberOfShares { get; set; }
        public int MinOfShares { get; set; }
        public int AvailableShares { get; set; }
        public int UsedShares { get; set; }
        // Numerical
        public double SharePrice { get; set; }
        // Percentage
        public double AnnualRentalYield { get; set; }
        // Percentage
        public double AnnualPriceAppreciation { get; set; }
        // Numerical
        public double? DownPayment { get; set; }
        // Numerical
        public double? MonthlyInstallment {  get; set; }
        public int? NumberOfYears {  get; set; }
        // Numerical
        public double? MaintenaceInstallment {  get; set; }
        // Numerical 
        public double? DeliveryInstallment {  get; set; }
        public Type Type { get; set; }
        public Status Status {  get; set; }
        public bool IsDeleted {  get; set; }
        public DateTime LastModificationDate {  get; set; }
        public virtual ICollection<PropertyFacility> PropertyFacilities { get; set; } = new List<PropertyFacility>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<BuyTracker> BuyTrackers {  get; set; }
    }
    public enum Type
    {
        Residential = 1,
        Commercial = 2
    }
    public enum Status
    {
        UnderConstruction = 1,
        ReadyToMove = 2,
        Rented = 3
    }
}
