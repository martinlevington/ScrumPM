using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams.Specifications
{
    public class TenantSpecification : ISpecification<Team, ITeamSpecificationVisitor>
    {
        public readonly TenantId TenantId;

        public TenantSpecification(TenantId tenantId )
        {
            TenantId = tenantId;
        }


        public bool IsSatisfiedBy(Team entity)
        {
            return entity.TenantId.Equals(TenantId);
        }

        public void Accept(ITeamSpecificationVisitor visitor)
        {
           visitor.Visit(this);
        }
    }
}
