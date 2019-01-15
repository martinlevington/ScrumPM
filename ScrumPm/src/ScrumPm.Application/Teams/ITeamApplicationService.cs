using System.Collections.Generic;
using FluentResults;
using ScrumPm.Application.Teams.Commands;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams
{
    public interface ITeamApplicationService
    {
        IEnumerable<Team> GetTeams(TenantId tenantId);
        Team GetTeam(TenantId id, TeamId teamId);
        IEnumerable<Team> Search( TenantId teamId, string search);
        Result EnableProductOwner(EnableProductOwnerCommand command);
    }
}