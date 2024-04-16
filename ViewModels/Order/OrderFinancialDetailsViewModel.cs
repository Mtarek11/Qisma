using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderFinancialDetailsViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
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
        public DateTime? ConfirmationDate { get; set; }
    }
}
