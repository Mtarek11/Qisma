using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UpdateBlogViewModel
    {
        [Required]
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public string ImageLink {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}
