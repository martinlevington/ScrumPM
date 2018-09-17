using System.Collections.Generic;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAllTeams(TenantId tenantId);

        IReadOnlyList<Team> Find(TenantId tenantId, ISpecification<Team> specification);

        Team GetById(TenantId tenantId, TeamId teamId);

        void Remove(Team team);

        void RemoveAll(IEnumerable<Team> teams);

        void Save(Team team);

        void SaveAll(IEnumerable<Team> teams);

        Team GetByName(TenantId tenantId, string name);
    }
}