using System.Collections.Generic;
using FluentResults;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface IProductOwnerRepository
    {
        ICollection<ProductOwner> GetAllProductOwners(TenantId tenantId);

        Result<ProductOwner> Get(TenantId tenantId, string userName);

        void Remove(ProductOwner owner);

        void RemoveAll(IEnumerable<ProductOwner> owners);

        Result<ProductOwner> Save(TenantId tenantId, ProductOwner owner);

        Result SaveAll(IEnumerable<ProductOwner> owners);
    }
}
