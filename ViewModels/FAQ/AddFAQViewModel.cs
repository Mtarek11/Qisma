using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AddFAQViewModel
    {
        [Required]
        public string Question {  get; set; }
        [Required]
        public string Answer {  get; set; }
    }
}
