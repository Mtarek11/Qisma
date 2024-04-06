using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PropertyFacilityViewModelForAdmin
    {
        public int FacilityId { get; set; }
        public int PropertyFacilityId { get; set; }
        public string SVG { get; set; } 
        public string Description { get; set; }
    }
}
