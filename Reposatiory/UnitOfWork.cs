using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposatiory
{
    public class UnitOfWork(LoftyContext _dBContext)
    {
        private readonly LoftyContext dBContext = _dBContext;

        public async Task CommitAsync()
        {
            await dBContext.SaveChangesAsync();
        }
    }
}
