using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
    public class BuyTrackerManager(LoftyContext _mydB, UnitOfWork _unitOfWork) : MainManager<BuyTracker>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;   
        public async Task<bool> ProceedWithBuyAsync(string userId, string propertyId)
        {
            BuyTracker buyTracker = await GetAll().Where(i => i.UserId == userId && i.PropertyId == propertyId).Select(i => new BuyTracker()
            {
                PropertyId = i.PropertyId,
                UserId = i.UserId,
                LastProceedDate = i.LastProceedDate,
            }).FirstOrDefaultAsync();
            if (buyTracker != null)
            {
                PartialUpdate(buyTracker);
                buyTracker.LastProceedDate = DateTime.Now;
                await unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                BuyTracker newBuyTracker = new()
                {
                    PropertyId = propertyId,
                    UserId = userId,
                    LastProceedDate = DateTime.Now,
                };
                try
                {
                    await AddAsync(newBuyTracker);
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
}
