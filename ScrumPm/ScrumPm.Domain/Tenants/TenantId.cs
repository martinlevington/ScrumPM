using System;
using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Tenants
{


    public class TenantId : Identity
    {
        public TenantId()
            : base()
        {
        }

        public TenantId(Guid id)
            : base(id)
        {
        }
    }
}
