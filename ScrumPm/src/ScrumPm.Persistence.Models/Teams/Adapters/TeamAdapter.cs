using System;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Models.Teams.Adapters
{
    public class TeamAdapter : IPersistenceAdapter<TenantId, TeamEf, Team>
    {
        private readonly ProductOwnerAdapter _productOwnerAdapter;

        public TeamAdapter(ProductOwnerAdapter productOwnerAdapter)
        {
            _productOwnerAdapter = productOwnerAdapter;
        }

        public Team ToDomain(TenantId tenantId, TeamEf persistenceModel)
        {
            return new Team(tenantId, new TeamId(persistenceModel.Id), persistenceModel.Name, _productOwnerAdapter.ToDomain(tenantId,persistenceModel.ProductOwnerEf)) ;
        }

        public TeamEf ToPersistence(TenantId tenant, Team domain)
        {
            throw new NotImplementedException();
        }

     
    }
}