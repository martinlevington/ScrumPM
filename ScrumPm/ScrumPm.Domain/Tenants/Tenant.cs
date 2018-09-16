using System;
using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Tenants
{

    public sealed class Tenant : Entity
    {
        private readonly TenantId _id;

        public Tenant(TenantId id)
        {
            _id = id;
        }
    }
}
