using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Reposatiory
{
    public class GovernorateManager(LoftyContext _mydB) : MainManager<Governorate>(_mydB)
    {
        public async Task<List<GovernorateAndCityViewModel>> GetAllGovernoratesAsync()
        {
            List<GovernorateAndCityViewModel> governorates = await GetAll().Select(i => new GovernorateAndCityViewModel()
            {
                Id = i.Id,
                Name = i.NameEn
            }).ToListAsync();
            return governorates;
        }
    }
}
