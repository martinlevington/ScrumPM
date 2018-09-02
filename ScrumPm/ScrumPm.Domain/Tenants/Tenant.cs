using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Tenants
{

    public sealed class Tenant : Identity
    {
        public Tenant(string id)
            : base(id)
        {
        }
    }
}
