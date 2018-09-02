using System.Collections.Generic;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams
{
    using ScrumPm.Domain.Teams;

    public class TeamApplicationService : ITeamApplicationService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IProductOwnerRepository _productOwnerRepository;

        public TeamApplicationService(ITeamMemberRepository teamMemberRepository, IProductOwnerRepository productOwnerRepository, ITeamRepository teamRepository)
        {
            _teamMemberRepository = teamMemberRepository;
            _productOwnerRepository = productOwnerRepository;
            _teamRepository = teamRepository;
        }

        public TeamApplicationService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public IEnumerable<Team> GetTeams()
        {
           var teams =  _teamRepository.GetAllTeams(new TenantId("1"));
            return teams;
        }
    }
}
