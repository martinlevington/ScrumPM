using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Models.Teams
{
    public interface ITeamAdapterFactory
    {
        Team Create(TenantId tenantId, TeamEf persistenceModel);
    }
}