using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderingPageViewModel
    {
        public string PropertyId {  get; set; }
        public int AvailableShares {  get; set; }
        public int MinNumberOfShares { get; set; }
        public int NumberOfShares {  get; set; }
        public double SharePrice {  get; set; }
        public double? TransactionFees {  get; set; }
        public double? DownPayment {  get; set; }
        public double? MonthlyInstallment { get; set;}
        public int? NumberOfYears { get; set; }
        public double? MaintenaceInstallment {  get; set; }
        public double? DeliveryInstallment {  get; set; }
        public int? PurchaseShares { get; set; }
    }
}
