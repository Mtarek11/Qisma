using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddPropertyImagesViewModel
    {
        [Required]
        public string PropertyId { get; set; }
        [Required, AllowedImageExtensions]
        public IFormFileCollection Images {  get; set; }
    }
}
