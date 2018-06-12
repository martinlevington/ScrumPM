namespace ScrumPm.Domain.Teams
{
    using ScrumPm.Common;
    using ScrumPm.Domain.Tenants;

    public class ProductOwnerId : Identity
    {
        public ProductOwnerId(TenantId tenantId, string id)
            : base(tenantId + ":" + id)
        {
        }

        public TenantId TenantId => new TenantId(this.Id.Split(':')[0]);

        public string Identity => this.Id.Split(':')[1];
    }
}
