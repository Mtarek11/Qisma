using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuyTracker
    {
        public virtual User User { get; set; }
        public string UserId {  get; set; }
        public virtual Property Property { get; set; }
        public string PropertyId {  get; set; }
        public DateTime LastProceedDate {  get; set; }
    }
}
