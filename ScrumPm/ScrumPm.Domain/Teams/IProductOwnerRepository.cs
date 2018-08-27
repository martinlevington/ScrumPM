using System.Collections.Generic;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public interface IProductOwnerRepository
    {
        ICollection<ProductOwner> GetAllProductOwners(TenantId tenantId);

        ProductOwner Get(TenantId tenantId, string userName);

        void Remove(ProductOwner owner);

        void RemoveAll(IEnumerable<ProductOwner> owners);

        void Save(ProductOwner owner);

        void SaveAll(IEnumerable<ProductOwner> owners);
    }
}
