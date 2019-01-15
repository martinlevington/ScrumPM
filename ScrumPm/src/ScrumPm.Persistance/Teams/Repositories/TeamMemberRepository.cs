using System;
using System.Collections.Generic;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        public ICollection<TeamMember> GetAllTeamMembers(TenantId tenantId)
        {
            throw new NotImplementedException();
        }

        public void Remove(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<TeamMember> teamMembers)
        {
            throw new NotImplementedException();
        }

        public void Save(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<TeamMember> teamMembers)
        {
            throw new NotImplementedException();
        }

        public TeamMember Get(TenantId tenantId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
