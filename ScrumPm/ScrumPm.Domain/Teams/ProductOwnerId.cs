using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Teams
{
    using Tenants;

    public class ProductOwnerId : IdentityString
    {
        public ProductOwnerId(TenantId tenantId, string id)
            : base(tenantId + ":" + id)
        {
            TenantId = tenantId;
        }

        public TenantId TenantId;

    }
}
