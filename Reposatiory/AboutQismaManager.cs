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
            List<AboutQisma> aboutQisma = await GetAll().Where(i => i.Id == 1 || i.Id == 2).ToListAsync();
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
                await unitOfWork.CommitAsync();
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
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (isUpdated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<AboutUsViewModel> GetAboutUsAsync()
        {
            AboutUsViewModel viewModel = new();
            List<AboutQisma> aboutQisma = await GetAll().Where(i => i.Id == 3 || i.Id == 4 || i.Id == 5).ToListAsync();
            viewModel.FirstFrame = aboutQisma.Where(i => i.Id == 3).Select(i => i.Content).FirstOrDefault();
            viewModel.SecondFrameTitle = aboutQisma.Where(i => i.Id == 4).Select(i => i.Content).FirstOrDefault();
            viewModel.SecondFrameDescription = aboutQisma.Where(i => i.Id == 5).Select(i => i.Content).FirstOrDefault();
            return viewModel;
        }
        public async Task<bool> UpdateAboutUsAsync(AboutUsViewModel viewModel)
        {
            bool isUpdated = false;
            if (viewModel.FirstFrame != null)
            {
                AboutQisma supportEmail = new()
                {
                    Id = 3
                };
                PartialUpdate(supportEmail);
                supportEmail.Content = viewModel.FirstFrame;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.SecondFrameTitle != null)
            {
                AboutQisma supportPhoneNumber = new()
                {
                    Id = 4
                };
                PartialUpdate(supportPhoneNumber);
                supportPhoneNumber.Content = viewModel.SecondFrameTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.SecondFrameDescription != null)
            {
                AboutQisma supportPhoneNumber = new()
                {
                    Id = 5
                };
                PartialUpdate(supportPhoneNumber);
                supportPhoneNumber.Content = viewModel.SecondFrameDescription;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (isUpdated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
