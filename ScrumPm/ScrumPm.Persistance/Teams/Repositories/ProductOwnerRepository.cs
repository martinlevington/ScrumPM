using System;
using System.Collections.Generic;
using System.Text;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Persistence.Teams.Repositories
{
    public class ProductOwnerRepository : IProductOwnerRepository
    {
        public ICollection<ProductOwner> GetAllProductOwners(TenantId tenantId)
        {
            throw new NotImplementedException();
        }

        public ProductOwner Get(TenantId tenantId, string userName)
        {
            throw new NotImplementedException();
        }

        public void Remove(ProductOwner owner)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<ProductOwner> owners)
        {
            throw new NotImplementedException();
        }

        public void Save(ProductOwner owner)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<ProductOwner> owners)
        {
            throw new NotImplementedException();
        }
    }
}
