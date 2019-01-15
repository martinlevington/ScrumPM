using ScrumPm.Domain.Common.Specifications;

namespace ScrumPm.Domain.Teams.Specifications
{
    public interface ITeamSpecificationVisitor
         : ISpecificationVisitor<Team, ITeamSpecificationVisitor>
    {
        void Visit(TeamNameSearchSpecification specification);
        void Visit(TenantSpecification specification);
        void Visit(TeamIdSpecification specification);
        
    }
}
