using ScrumPm.Domain.Common.Specifications;

namespace ScrumPm.Domain.Teams.Specifications
{
    public class TeamIdSpecification : ISpecification<Team, ITeamSpecificationVisitor>
    {
        public TeamId TeamId { get; }

        public TeamIdSpecification(TeamId teamId )
        {
            TeamId = teamId;
        }

        public void Accept(ITeamSpecificationVisitor visitor)
        {
            visitor.Visit (this); 
        }

        public bool IsSatisfiedBy(Team entity)
        {
            return entity.Id.Equals(TeamId);
        }
    }
}
