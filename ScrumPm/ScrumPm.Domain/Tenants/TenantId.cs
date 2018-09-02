using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Tenants
{


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
