using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderViewModelForAdmin
    {
        public UserFullInformationViewModel UserInformation {  get; set; } = new UserFullInformationViewModel();
        public OrderFinancialDetailsViewModel OrderFinancialDetails { get; set;} = new OrderFinancialDetailsViewModel();
        public PropertyViewModelInListView Property {  get; set;} = new PropertyViewModelInListView();
        public OrderStatus OrderStatus { get; set;} 
        public string OrderStatusName {  get; set;}
    }
}
