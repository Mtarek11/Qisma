using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderPreviewPageViewModel
    {
        public string PropertyName { get; set; }
        public string PropertyLocation {  get; set; }
        public OrderingPageViewModel OrderingPage { get; set; }
        public UserInformationForOrderPreviewViewModel UserInformation {  get; set; }
    }
}
