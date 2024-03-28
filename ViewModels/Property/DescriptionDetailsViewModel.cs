using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DescriptionDetailsViewModel
    {
        [Required]
        public string Title {  get; set; }
        [Required]
        public List<string> Values {  get; set; }
    }
}
