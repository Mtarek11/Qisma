using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PropertyViewModelInListView
    {
        public string PropertyId { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string City {  get; set; }
        public double ProjectedAnnualReturn { get; set; }
        public double ProjectedRentalYield { get; set; }
        public double TokenPrice {  get; set; }
        public int AvailableTokens { get; set; }
        public bool? IsDeleted {  get; set; }
    }
}
