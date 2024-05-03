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
        public async Task<SupportViewModel> GetSupportContactInformationsAsync()
        {
            SupportViewModel viewModel = new SupportViewModel();
            List<AboutQisma> aboutQisma = await GetAll().Where(i => i.Id == 1 && i.Id == 2).ToListAsync();
            viewModel.SupportEmail = aboutQisma.Where(i => i.Id == 1).Select(i => i.Content).FirstOrDefault();
            viewModel.SupportPhoneNumber = aboutQisma.Where(i => i.Id == 2).Select(i => i.Content).FirstOrDefault();
            return viewModel;
        }
        public async Task<bool> UpdateSupportContactInformationsAsync(SupportViewModel viewModel)
        {
            bool isUpdated = false;
            if (viewModel.SupportEmail != null)
            {
                AboutQisma supportEmail = new()
                {
                    Id = 1
                };
                PartialUpdate(supportEmail);
                supportEmail.Content = viewModel.SupportEmail;
                isUpdated = true;
            }
            if (viewModel.SupportPhoneNumber != null)
            {
                AboutQisma supportPhoneNumber = new()
                {
                    Id = 2
                };
                PartialUpdate(supportPhoneNumber);
                supportPhoneNumber.Content = viewModel.SupportPhoneNumber;
                isUpdated = true;
            }
            if (isUpdated)
            {
                await unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
