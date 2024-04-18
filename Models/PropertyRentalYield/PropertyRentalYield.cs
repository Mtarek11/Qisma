using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyRentalYield
    {
        public int Id { get; set; }
        public virtual Property Property { get; set; }
        public string PropertyId { get; set; }
        public double RentalYield { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}
