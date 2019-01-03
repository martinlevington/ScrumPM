using System;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Teams.Adapters
{
    public class ProductOwnerAdapter : IPersistenceAdapter<TenantId, ProductOwnerEf, ProductOwner>
    {
        public ProductOwner ToDomain(TenantId tenantId, ProductOwnerEf persistenceModel)
        {
            var productOwner = new ProductOwner(tenantId, persistenceModel.UserName,persistenceModel.FirstName,persistenceModel.LastName, persistenceModel.EmailAddress,persistenceModel.Modified);

            return productOwner;
        }

        public ProductOwnerEf ToPersistence(TenantId tenant, ProductOwner domain)
        {
            throw new NotImplementedException();
        }
    }
}