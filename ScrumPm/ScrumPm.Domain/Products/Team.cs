using ScrumPm.Domain.Teams;

namespace ScrumPm.Domain.Products
{
    using System.Collections.Generic;
    using ScrumPm.Common;
    using ScrumPm.Domain.Tenants;

    public class Team : Entity
    {
        public Team(TenantId tenantId, string name, ProductOwner productOwner = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenantId must be provided.");

            this.tenantId = tenantId;
            this.Name = name;
            if (productOwner != null)
                this.ProductOwner = productOwner;
            this.teamMembers = new HashSet<TeamMember>();
        }

        readonly TenantId tenantId;
        string name;
        ProductOwner productOwner;
        readonly HashSet<TeamMember> teamMembers;

        public TenantId TenantId
        {
            get { return this.tenantId; }
        }
    }
}
