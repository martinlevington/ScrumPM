using System.Collections.Generic;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams
{
    using ScrumPm.Domain.Teams;

    public class TeamApplicationService : ITeamApplicationService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductOwnerRepository _productOwnerRepository;

        public TeamApplicationService(ITeamMemberRepository teamMemberRepository, IProductOwnerRepository productOwnerRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _teamMemberRepository = teamMemberRepository;
            _productOwnerRepository = productOwnerRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public TeamApplicationService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public IEnumerable<Team> GetTeams(TenantId tenantId)
        {
           var teams =  _teamRepository.GetAllTeams(tenantId);
            return teams;
        }
     
        public Team GetTeam(TenantId id, TeamId teamId)
        {
            return _teamRepository.GetById(id, teamId);
        }

        public IEnumerable<Team> Search(TenantId tenantId, string search)
        {
           var searchSpecification = new TenantSpecification(tenantId).And(new TeamNameSearchSpecification(search));

         

            var teams =  _teamRepository.Find(tenantId, searchSpecification );
            return teams;
        }
    }
}
