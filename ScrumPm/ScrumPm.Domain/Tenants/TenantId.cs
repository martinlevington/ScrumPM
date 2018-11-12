using System;
using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Tenants
{


    public class TenantId : Identity
    {
        public TenantId()
        {
        }

        public TenantId(Guid id)
            : base(id)
        {
        }
    }
}
