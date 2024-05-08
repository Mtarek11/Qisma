using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserPortfolioViewModel
    {
        public double ProtfolioValue {  get; set; }
        public double GrossMonthlyIncome { get; set; }
        public int NumberOfProperties {  get; set; }
        public string CurrentMonth {  get; set; }
        public List<UserPropertiesInPortfolioViewModel> UserStakes {  get; set; } = new List<UserPropertiesInPortfolioViewModel>();
    }
}
