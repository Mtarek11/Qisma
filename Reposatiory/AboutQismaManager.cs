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
    public class AboutQismaManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<AboutQisma>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task<string> GetAboutQismaContentAsync(int id)
        {
            string content = await GetAll().Where(i => i.Id == id).Select(i => i.Content).FirstOrDefaultAsync();
            return content;
        }
        public async Task<bool> UpdateAboutQismaContent(int id, string content)
        {
            AboutQisma aboutQisma = new()
            {
                Id = id
            };
            PartialUpdate(aboutQisma);
            aboutQisma.Content = content;
            try
            {
                await unitOfWork.CommitAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
