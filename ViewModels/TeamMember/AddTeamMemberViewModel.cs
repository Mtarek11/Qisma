using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddTeamMemberViewModel
    {
        [Required, AllowedImageExtensions]
        public IFormFile Image { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string Summary { get; set; }
        public string FacebookLink { get; set; }
        public string XLink { get; set; }
        public string InstagramLink { get; set; }
        public string LinkedInLink { get; set; }
        [Required]
        public bool IsManager { get; set; }
    }
}
