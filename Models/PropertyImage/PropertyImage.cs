using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string PropertyId { get; set; }
        public virtual Property Property { get; set; }
    }
}
