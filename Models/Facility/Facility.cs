using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Facility
    {
        public int Id {  get; set; }
        public string SVG { get; set; } 
        public virtual ICollection<PropertyFacility> PropertyFacilities { get; set; }
    }
}
