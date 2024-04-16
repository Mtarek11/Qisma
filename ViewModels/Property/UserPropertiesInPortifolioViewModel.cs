using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserPropertiesInPortifolioViewModel
    {
        public string Property {  get; set; }
        public string Location { get; set; }
        public double InvestmentValue {  get; set; }
        public double TotalRentalIncome { get; set; }
        public OrderStatus Status {  get; set; }
    }
}
