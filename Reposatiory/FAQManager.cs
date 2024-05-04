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
    public class FAQManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<FAQ>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task AddFAQAsync(string question, string answer)
        {
            FAQ FAQ = new()
            {
                Question = question,
                Answer = answer
            };
            await AddAsync(FAQ);
            await unitOfWork.CommitAsync();
        }
        public async Task<bool> DeleteFAQAsync(int id)
        {
            FAQ FAQ = new()
            {
                Id = id
            };
            try
            {
                Remove(FAQ);
                await unitOfWork.CommitAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public async Task<List<FAQ>> GetAllFAQAsync()
        {
            List<FAQ> FAQs = await GetAll().AsNoTracking().ToListAsync();
            return FAQs;
        }
    }
}
