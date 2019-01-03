using ScrumPm.Domain.Common;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
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
