using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserPropertiesInPortfolioViewModel
    {
        public string PropertyId { get; set; }
        public string PropertyLocation { get; set; }
        public double InvestmentValue { get; set; }
        public double TotalRentalIncome { get; set; }
        public Status StatusId { get; set; }
        public string Status { get; set; }
    }
}
