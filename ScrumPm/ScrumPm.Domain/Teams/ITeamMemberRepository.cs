namespace ScrumPm.Domain.Teams
{
    using System.Collections.Generic;

    public interface ITeamMemberRepository
    {
        ICollection<TeamMember> GetAllTeamMembers(Tenants.TenantId tenantId);

        void Remove(TeamMember teamMember);

        void RemoveAll(IEnumerable<TeamMember> teamMembers);

        void Save(TeamMember teamMember);

        void SaveAll(IEnumerable<TeamMember> teamMembers);

        TeamMember Get(Tenants.TenantId tenantId, string userName);
    }
}
