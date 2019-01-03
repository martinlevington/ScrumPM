using System.Collections.Generic;
using FluentResults;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAllTeams(TenantId tenantId);

        Result<IReadOnlyList<Team>> Find(TenantId tenantId, ISpecification<Team, ITeamSpecificationVisitor> specification);

        Team GetById(TenantId tenantId, TeamId teamId);

        //void Remove(Team team);

        //void RemoveAll(IEnumerable<Team> teams);

        Result<Team> Save(TenantId tenantId, Team team);

        Result<Team> Update(TenantId tenantId, Team team);

        //void SaveAll(IEnumerable<Team> teams);

        //Team GetByName(TenantId tenantId, string name);
    }
}