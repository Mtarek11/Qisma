using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Reposatiory
{
    public class TeamMeamberManager(LoftyContext _mydB, UnitOfWork _unitOfWork, AboutQismaManager _aboutQismaManager) : MainManager<TeamMember>(_mydB)
    {
        private readonly AboutQismaManager aboutQismaManager = _aboutQismaManager;
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        public async Task AddTeamMemberAsync(AddTeamMemberViewModel viewModel)
        {
            TeamMember teamMember = new()
            {
                Summary = viewModel.Summary,
                FacebookLink = viewModel.FacebookLink,
                InstagramLink = viewModel.InstagramLink,
                IsManager = viewModel.IsManager,
                JobTitle = viewModel.JobTitle,
                Name = viewModel.Name,
                XLink = viewModel.XLink,
                LinkedInLink = viewModel.LinkedInLink,
                ImageUrl = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName,
            };
            FileStream fileStream = new FileStream(
            Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", teamMember.ImageUrl),
                    FileMode.Create);
            await viewModel.Image.CopyToAsync(fileStream);
            fileStream.Position = 0;
            fileStream.Close();
            await AddAsync(teamMember);
            await unitOfWork.CommitAsync();
        }
        public async Task<TeamViewModel> GetAllManagersAsync()
        {
            TeamViewModel teamViewModel = new();
            List<TeamMember> teamMembers = await GetAll().Where(i => i.IsManager).AsNoTracking().ToListAsync();
            string title = await aboutQismaManager.GetManagerTitleAsync();
            teamViewModel.TeamMembers = teamMembers;
            teamViewModel.Title = title;
            return teamViewModel;
        }
        public async Task<TeamViewModel> GetAllMembersAsync()
        {
            TeamViewModel teamViewModel = new();
            List<TeamMember> teamMembers = await GetAll().Where(i => !i.IsManager).AsNoTracking().ToListAsync();
            string title = await aboutQismaManager.GetTeamMemberTitleAsync();
            teamViewModel.TeamMembers = teamMembers;
            teamViewModel.Title = title;
            return teamViewModel;
        }
        public async Task<APIResult<string>> UpdateTeamMemberAsync(UpdateTeamMemberViewModel viewModel)
        {
            APIResult<string> aPIResult = new();
            string oldImage = null;
            TeamMember teamMember = new()
            {
                Id = viewModel.Id
            };
            PartialUpdate(teamMember);
            bool isUpdated = false;
            if (viewModel.Image != null)
            {
                oldImage = await GetAll().Where(i => i.Id == viewModel.Id).Select(i => i.ImageUrl).FirstOrDefaultAsync();
                if (oldImage == null)
                {
                    aPIResult.Message = "Team member not found";
                    aPIResult.IsSucceed = false;
                    aPIResult.StatusCode = 404;
                    return aPIResult;
                }
                teamMember.ImageUrl = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                isUpdated = true;
            }
            if (viewModel.Name != null)
            {
                teamMember.Name = viewModel.Name;
                isUpdated = true;
            }
            if (viewModel.JobTitle != null)
            {
                teamMember.JobTitle = viewModel.JobTitle;
                isUpdated = true;
            }
            if (viewModel.Summary != null)
            {
                teamMember.Summary = viewModel.Summary;
                isUpdated = true;
            }
            if (viewModel.FacebookLink != null)
            {
                teamMember.FacebookLink = viewModel.FacebookLink;
                isUpdated = true;
            }
            if (viewModel.XLink != null)
            {
                teamMember.XLink = viewModel.XLink;
                isUpdated = true;
            }
            if (viewModel.InstagramLink != null)
            {
                teamMember.InstagramLink = viewModel.InstagramLink;
                isUpdated = true;
            }
            if (viewModel.LinkedInLink != null)
            {
                teamMember.LinkedInLink = viewModel.LinkedInLink;
                isUpdated = true;
            }
            if (isUpdated)
            {
                if (oldImage != null)
                {
                    FileStream fileStream = new FileStream(
                    Path.Combine(
                       Directory.GetCurrentDirectory(), "Content", "Images", teamMember.ImageUrl),
                    FileMode.Create);
                    await viewModel.Image.CopyToAsync(fileStream);
                    fileStream.Position = 0;
                    fileStream.Close();
                }
                try
                {
                    await unitOfWork.CommitAsync();
                }
                catch (DbUpdateException)
                {
                    aPIResult.Message = "Team member not found";
                    aPIResult.StatusCode = 404;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                if (oldImage != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", oldImage);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                aPIResult.Message = "Team member updated";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = true;
                return aPIResult;
            }
            else
            {
                aPIResult.Message = "Nothing to update";
                aPIResult.StatusCode = 400;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<bool> DeleteTeamMemberAsync(int teamMemberId)
        {
            TeamMember teamMember = await GetAll().Where(i => i.Id == teamMemberId).FirstOrDefaultAsync();
            if (teamMember != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", teamMember.ImageUrl);
                Remove(teamMember);
                await unitOfWork.CommitAsync();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
