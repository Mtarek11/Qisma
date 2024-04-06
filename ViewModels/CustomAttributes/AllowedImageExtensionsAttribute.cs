﻿using Microsoft.AspNetCore.Http;
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
            _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".svg", ".heic", ".heif", ".ico", ".pdf" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Identity image requierd");
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