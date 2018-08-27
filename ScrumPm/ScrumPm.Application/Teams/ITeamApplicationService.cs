using System.Collections.Generic;
using ScrumPm.Domain.Teams;

namespace ScrumPm.Application.Teams
{
    public interface ITeamApplicationService
    {
        IEnumerable<Team> GetTeams();
    }
}