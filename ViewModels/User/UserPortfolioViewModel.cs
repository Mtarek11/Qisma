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
        public double TotalAppreciation { get; set; }
        public int NumberOfProperties {  get; set; }
        public int CurrentMonth {  get; set; }
        public UserPropertiesInPortifolioViewModel UserStakes {  get; set; } = new UserPropertiesInPortifolioViewModel();
    }
}
