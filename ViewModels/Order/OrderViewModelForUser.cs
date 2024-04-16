using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderViewModelForUser
    {
        public OrderFinancialDetailsViewModel OrderFinancialDetails { get; set; } = new OrderFinancialDetailsViewModel();
        public PropertyViewModelInListView Property { get; set; } = new PropertyViewModelInListView();
    }
}
