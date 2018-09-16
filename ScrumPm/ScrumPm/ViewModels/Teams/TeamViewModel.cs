using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPm.ViewModels.Teams
{
    public class TeamViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TeamMembersViewModel> TeamMembers { get; set; }
        public ProductOwnerViewModel ProductOwner { get; set; }

    }
}
