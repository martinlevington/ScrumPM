using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Teams.Adapters;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Teams
{
    public class TeamAdapterFactory : ITeamAdapterFactory
    {
        
       

        public Team Create(TenantId tenantId, TeamEf persistenceModel)
        {
           return new TeamAdapter(new ProductOwnerAdapter()).ToDomain(tenantId,persistenceModel);

        }

    }
}
