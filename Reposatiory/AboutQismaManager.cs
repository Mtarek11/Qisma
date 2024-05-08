using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            List<AboutQisma> aboutQisma = await GetAll().ToListAsync();
            viewModel.FirstFrame = aboutQisma.Where(i => i.Id == 3).Select(i => i.Content).FirstOrDefault();
            viewModel.FirstFrameImageUrl = await GetAll().Where(i => i.Id == 8).Select(i => i.Content).FirstOrDefaultAsync();
            viewModel.SecondFrameTitle = aboutQisma.Where(i => i.Id == 4).Select(i => i.Content).FirstOrDefault();
            viewModel.SecondFrameDescription = aboutQisma.Where(i => i.Id == 5).Select(i => i.Content).FirstOrDefault();
            viewModel.ThirdFrameTitle = aboutQisma.Where(i => i.Id == 9).Select(i => i.Content).FirstOrDefault();
            viewModel.FirstSectionTitle = aboutQisma.Where(i => i.Id == 10).Select(i => i.Content).FirstOrDefault();
            viewModel.FirstSectionDescription = aboutQisma.Where(i => i.Id == 11).Select(i => i.Content).FirstOrDefault();
            viewModel.SecondSectionTitle = aboutQisma.Where(i => i.Id == 12).Select(i => i.Content).FirstOrDefault();
            viewModel.FirstSectionDescription = aboutQisma.Where(i => i.Id == 13).Select(i => i.Content).FirstOrDefault();
            viewModel.ThirdSectionTitle = aboutQisma.Where(i => i.Id == 14).Select(i => i.Content).FirstOrDefault();
            viewModel.ThirdSectionDescription = aboutQisma.Where(i => i.Id == 15).Select(i => i.Content).FirstOrDefault();
            viewModel.FourthSectionTitle = aboutQisma.Where(i => i.Id == 16).Select(i => i.Content).FirstOrDefault();
            viewModel.FourthSectionDescription = aboutQisma.Where(i => i.Id == 17).Select(i => i.Content).FirstOrDefault();
            return viewModel;
        }
        public async Task<bool> UpdateAboutUsAsync(UpdateAboutUsViewModel viewModel)
        {
            bool isUpdated = false;
            if (viewModel.FirstFrame != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 3
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.FirstFrame;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.FirstFrameImage != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.FirstFrameImage.FileName;
                FileStream fileStream = new FileStream(
                        Path.Combine(
                           Directory.GetCurrentDirectory(), "Content", "Images", uniqueFileName),
                        FileMode.Create);
                await viewModel.FirstFrameImage.CopyToAsync(fileStream);
                fileStream.Position = 0;
                fileStream.Close();
                AboutQisma oldImageUrl = await GetAll().Where(i => i.Id == 8).FirstOrDefaultAsync();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", oldImageUrl.Content);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                oldImageUrl.Content = uniqueFileName;
                Update(oldImageUrl);
                await unitOfWork.CommitAsync();
            }
            if (viewModel.SecondFrameTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 4
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.SecondFrameTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.SecondFrameDescription != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 5
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.SecondFrameDescription;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.ThirdFrameTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 9
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.ThirdFrameTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.FirstSectionTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 10
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.FirstSectionTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.FirstSectionDescription != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 11
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.FirstSectionDescription;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.SecondSectionTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 12
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.SecondSectionTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.SecondSectionDescription != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 13
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.SecondSectionDescription;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.ThirdSectionTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 14
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.ThirdSectionTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.ThirdSectionDescription != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 15
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.ThirdSectionDescription;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.FourthSectionTitle != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 16
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.FourthSectionTitle;
                await unitOfWork.CommitAsync();
                isUpdated = true;
            }
            if (viewModel.FourthSectionDescription != null)
            {
                AboutQisma aboutUs = new()
                {
                    Id = 17
                };
                PartialUpdate(aboutUs);
                aboutUs.Content = viewModel.FourthSectionDescription;
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
        public async Task<string> GetManagerTitleAsync()
        {
            string title = await GetAll().Where(i => i.Id == 6).Select(i => i.Content).FirstOrDefaultAsync();
            return title;
        }
        public async Task<string> GetTeamMemberTitleAsync()
        {
            string title = await GetAll().Where(i => i.Id == 7).Select(i => i.Content).FirstOrDefaultAsync();
            return title;
        }
        public async Task UpdateManagerTitleAsync(string title)
        {
            AboutQisma managerTitle = new()
            {
                Id = 6
            };
            PartialUpdate(managerTitle);
            managerTitle.Content = title;
            await unitOfWork.CommitAsync();
        }
        public async Task UpdateTeamMemberTitleAsync(string title)
        {
            AboutQisma teamMemberTitle = new()
            {
                Id = 7
            };
            PartialUpdate(teamMemberTitle);
            teamMemberTitle.Content = title;
            await unitOfWork.CommitAsync();
        }
    }
}
