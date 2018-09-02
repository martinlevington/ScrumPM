using System.Collections.Generic;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAllTeams(TenantId tenantId);

        void Remove(Team team);

        void RemoveAll(IEnumerable<Team> teams);

        void Save(Team team);

        void SaveAll(IEnumerable<Team> teams);

        Team GetByName(TenantId tenantId, string name);
    }
}