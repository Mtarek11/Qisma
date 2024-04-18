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
        public string OrderNumber {  get; set; }
        public virtual Property Property { get; set; }
        public string PropertyId { get; set; }
        public virtual User User { get; set; }
        public string UserId {  get; set; }
        public double SharePrice { get; set; }
        public double? TransactionFees { get; set; }
        public int NumberOfShares { get; set; }
        public double? DownPayment { get; set; }
        public double? MonthlyInstallment { get; set; }
        public int? NumberOfYears { get; set; }
        public double? MaintenaceInstallment { get; set; }
        public double? DeliveryInstallment { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ConfirmationDate {  get; set; }
        public OrderStatus OrderStatus {  get; set; }
    }
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2
    }
}
