using ScrumPm.Domain.Common.Specifications;

namespace ScrumPm.Domain.Teams.Specifications
{
    public class TeamIdSpecification : ISpecification<Team>
    {
        public TeamId TeamId { get; }

        public TeamIdSpecification(TeamId teamId )
        {
            TeamId = teamId;
        }

       

        public bool IsSatisfiedBy(Team entity)
        {
            return entity.Id.Equals(TeamId);
        }
    }
}
