using ScrumPm.Domain.Common.Specifications;

namespace ScrumPm.Domain.Teams.Specifications
{
    public class TeamNameSearchSpecification : ISpecification<Team, ITeamSpecificationVisitor>
    {
        public readonly string SearchTerm;

        public TeamNameSearchSpecification(string search)
        {
            SearchTerm = search;
        }

        public bool IsSatisfiedBy(Team entity)
        {
            return entity.Name.Contains(SearchTerm);
        }

        public void Accept(ITeamSpecificationVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}