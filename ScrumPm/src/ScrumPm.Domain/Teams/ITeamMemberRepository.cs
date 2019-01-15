using System.Collections.Generic;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface ITeamMemberRepository
    {
        ICollection<TeamMember> GetAllTeamMembers(TenantId tenantId);

        void Remove(TeamMember teamMember);

        void RemoveAll(IEnumerable<TeamMember> teamMembers);

        void Save(TeamMember teamMember);

        void SaveAll(IEnumerable<TeamMember> teamMembers);

        TeamMember Get(TenantId tenantId, string userName);
    }
}
