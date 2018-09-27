using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Teams
{
    public interface ITeamAdapterFactory
    {
        Team Create(TenantId tenantId, TeamEf persistenceModel);
    }
}