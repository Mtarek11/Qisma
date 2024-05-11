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
        private readonly LoftyContext mydB = _mydB;
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task<bool> AddFAQAsync(string question, string answer)
        {
            int number = await GetAll().CountAsync();
            FAQ FAQ = new()
            {
                Question = question,
                Answer = answer,
                Number = number + 1
            };
            try
            {
                await AddAsync(FAQ);
                await unitOfWork.CommitAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }
        public async Task<bool> DeleteFAQAsync(int id)
        {
            FAQ deletedFAQ = await GetAll().Where(i => i.Id == id).FirstOrDefaultAsync();
            if (deletedFAQ != null)
            {
                List<FAQ> modifiedFAQs = await GetAll().Where(i => i.Number > deletedFAQ.Number).ToListAsync();
                if (modifiedFAQs.Count > 0)
                {
                    foreach (FAQ modifiedFAQ in modifiedFAQs)
                    {
                        modifiedFAQ.Number--;
                        Update(modifiedFAQ);
                    }
                }
                Remove(deletedFAQ);
                await unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<FAQ>> GetAllFAQAsync()
        {
            List<FAQ> FAQs = await GetAll().AsNoTracking().OrderBy(i => i.Number).ToListAsync();
            return FAQs;
        }
        public async Task<APIResult<string>> UpdateFAQsIndexAsync(List<int> fAqs)
        {
            APIResult<string> aPIResult = new();
            if (fAqs.Count > 0)
            {
                List<FAQ> oldFaqs = await GetAll().Where(i => fAqs.Contains(i.Id)).ToListAsync();
                if (oldFaqs.Count != fAqs.Count)
                {
                    aPIResult.Message = "FAQs not found";
                    aPIResult.StatusCode = 404;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                int number = 0;
                foreach (int newFaq in fAqs)
                {
                    FAQ oldFAQ = oldFaqs.Where(i => i.Id == newFaq).FirstOrDefault();
                    number++;
                    oldFAQ.Number = number;
                    Update(oldFAQ);
                }
                await unitOfWork.CommitAsync();
                aPIResult.Message = "FAQ updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            else
            {
                aPIResult.StatusCode = 400;
                aPIResult.Message = "Nothing to update";
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<APIResult<string>> UpdateFAQAsync(UpdateFAQViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            bool isUpdated = false;
            FAQ fAQ = new()
            {
                Id = viewModel.Id
            };
            PartialUpdate(fAQ);
            if (viewModel.Question != null)
            {
                fAQ.Question = viewModel.Question;
                isUpdated = true;
            }
            if (viewModel.Answer != null)
            {
                fAQ.Answer = viewModel.Answer;
                isUpdated = true;
            }
            if (isUpdated)
            {
                try
                {
                    await unitOfWork.CommitAsync();
                    aPIResult.Message = "FAQ updated";
                    aPIResult.IsSucceed = true;
                    aPIResult.StatusCode = 200;
                    return aPIResult;
                }
                catch (DbUpdateException)
                {
                    aPIResult.Message = "FAQ not found";
                    aPIResult.StatusCode = 404;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
            }
            else
            {
                aPIResult.Message = "Nothing to update";
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
    }
}
