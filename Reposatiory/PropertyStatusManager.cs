using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposatiory
{
    public class PropertyStatusManager(LoftyContext _mydB) : MainManager<PropertyStatus>(_mydB)
    {
    }
}
