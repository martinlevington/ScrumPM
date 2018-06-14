namespace ScrumPm.Domain.Tenants
{
    using ScrumPm.Common;

    public sealed class Tenant : Identity
    {
        public Tenant(string id)
            : base(id)
        {
        }
    }
}
