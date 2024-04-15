using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual Property Property { get; set; }
        public int PropertyId { get; set; }
        public double? MaintenanceCost { get; set; }
        public double? TransactionFees { get; set; }
        public int NumberOfShares { get; set; }
        public double SharePrice { get; set; }
        public double AnnualRentalYield { get; set; }
        public double AnnualPriceAppreciation { get; set; }
        public double? DownPayment { get; set; }
        public double? MonthlyInstallment { get; set; }
        public int? NumberOfYears { get; set; }
        public double? MaintenaceInstallment { get; set; }
        public double? DeliveryInstallment { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus {  get; set; }
    }
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2
    }
}
