using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedImageExtensionsAttribute()
        {
            _allowedExtensions = new[] { ".jpg", ".jpeg", ".pjpeg", ".aviv", ".png", ".gif", ".bmp", ".tiff", ".tif", ".jfif", ".bmp", ".webp", ".svg", ".svgz", ".ico", ".pdf", ".xbm", ".dib", ".pjp", ".apng" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            IFormFile file = value as IFormFile;
            if (file == null)
            {
                return new ValidationResult("The provided value is not a valid file stream.");
            }
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"Only the following image and PDF file types are allowed: {string.Join(", ", _allowedExtensions)}.");
            }
            return ValidationResult.Success;
        }
    }
}
