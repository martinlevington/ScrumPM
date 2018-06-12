namespace ScrumPm.Domain.Tenants
{
    using ScrumPm.Common;

    public class TenantId : Identity
    {
        public TenantId()
            : base()
        {
        }

        public TenantId(string id)
            : base(id)
        {
        }
    }
}
