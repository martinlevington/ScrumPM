using System.Collections.Generic;
using System.Linq;
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

        public override IEnumerable<object> GetIdentityComponents()
        {
            return new List<object>()
            {
                _id
            }.AsEnumerable() ;
        }
    }
}
