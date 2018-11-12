using System.Collections.Generic;
using ScrumPm.Domain.Common;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
    public class TeamMemberId : ValueObject
    {
        public TeamMemberId(TenantId tenantId, string id)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenantId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(id, "The id must be provided.");
            AssertionConcern.AssertArgumentLength(id, 36, "The id must be 36 characters or less.");

            TenantId = tenantId;
            Id = id;
        }

        public TenantId TenantId { get; private set; }

        public string Id { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TenantId;
            yield return Id;
        }
    }
}
