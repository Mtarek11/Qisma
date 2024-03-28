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
    public class CityManager(LoftyContext _mydB) : MainManager<City>(_mydB)
    {
        public async Task<List<GovernorateAndCityViewModel>> GetCitiesByGovernorateIdAsync(int governorateId)
        {
            List<GovernorateAndCityViewModel> cities = await GetAll().Where(i => i.GovernorateId == governorateId).Select(i => new GovernorateAndCityViewModel()
            {
                Id = i.Id,
                Name = i.NameEn
            }).ToListAsync();
            return cities;
        }
        public async Task<bool> CheckCityAsync(int CityId, int governorateId)
        {
            bool checkCity = await GetAll().AnyAsync(i => i.Id == CityId && i.GovernorateId == governorateId);
            return checkCity;
        }
    }
}
