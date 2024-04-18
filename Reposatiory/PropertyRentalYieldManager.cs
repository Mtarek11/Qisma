using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposatiory
{
    public class PropertyRentalYieldManager(LoftyContext _mydB) : MainManager<PropertyRentalYield>(_mydB)
    {
    }
}
