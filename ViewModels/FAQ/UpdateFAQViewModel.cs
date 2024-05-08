using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UpdateFAQViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
