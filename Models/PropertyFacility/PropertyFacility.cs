using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyFacility
    {
        public int Id { get; set; }
        public virtual Facility Facility { get; set; } 
        public int FacilityId { get; set; }
        public virtual Property Property { get; set; }
        public string PropertyId {  get; set; }
        public string Description { get; set; }
        public int Number {  get; set; }
    }
}
