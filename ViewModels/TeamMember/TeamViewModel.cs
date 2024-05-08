using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class TeamViewModel
    {
        public string Title {  get; set; }
        public List<TeamMember> TeamMembers { get; set; }
    }
}
