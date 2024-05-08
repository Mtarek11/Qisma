using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UpdateAboutUsViewModel
    {
        public string FirstFrame { get; set; }
        public IFormFile FirstFrameImage { get; set; }
        public string SecondFrameTitle { get; set; }
        public string SecondFrameDescription { get; set; }
        public string ThirdFrameTitle { get; set; }
        public string FirstSectionTitle { get; set; }
        public string FirstSectionDescription { get; set; }
        public string SecondSectionTitle { get; set; }
        public string SecondSectionDescription { get; set; }
        public string ThirdSectionTitle { get; set; }
        public string ThirdSectionDescription { get; set; }
        public string FourthSectionTitle { get; set; }
        public string FourthSectionDescription { get; set; }
    }
}
