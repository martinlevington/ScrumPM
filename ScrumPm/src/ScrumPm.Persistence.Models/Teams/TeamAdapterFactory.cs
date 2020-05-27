using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Models.Teams.Adapters;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Models.Teams
{
    public class TeamAdapterFactory : ITeamAdapterFactory
    {
        
       

        public Team Create(TenantId tenantId, TeamEf persistenceModel)
        {
           return new TeamAdapter(new ProductOwnerAdapter()).ToDomain(tenantId,persistenceModel);

        }

       
    }
}
