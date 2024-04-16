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
    public class BuyTrackerManager(LoftyContext _mydB, UnitOfWork _unitOfWork, AccountManager _accountManager, PropertyManager _propertyManager) : MainManager<BuyTracker>(_mydB)
    {
        private readonly PropertyManager propertyManager = _propertyManager;
        private readonly AccountManager accountManager = _accountManager;
        private readonly UnitOfWork unitOfWork = _unitOfWork;   
        public async Task<APIResult<string>> ProceedWithBuyAsync(string userId, string propertyId)
        {
            APIResult<string> aPIResult = new();
            bool checkUser = await accountManager.GetAll().AnyAsync(i => i.Id == userId);
            if (!checkUser)
            {
                aPIResult.Message = "User not found";
                aPIResult.StatusCode = 401;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            bool checkProperty = await propertyManager.GetAll().AnyAsync(i => i.Id == propertyId && !i.IsDeleted);
            if (!checkProperty)
            {
                aPIResult.Message = "Property not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
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
                aPIResult.Message = "Time updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            else
            {
                BuyTracker newBuyTracker = new()
                {
                    PropertyId = propertyId,
                    UserId = userId,
                    LastProceedDate = DateTime.Now,
                };
                await AddAsync(newBuyTracker);
                await unitOfWork.CommitAsync();
                aPIResult.Message = "Time updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
        }
    }
}
