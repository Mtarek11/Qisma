using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Summary { get; set; }
        public string FacebookLink {  get; set; }
        public string XLink { get; set; }
        public string InstagramLink {  get; set; }
        public string LinkedInLink {  get; set; }
        public bool IsManager { get; set; }
    }
}
