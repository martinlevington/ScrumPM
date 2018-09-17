using ScrumPm.Domain.Common.Specifications;

namespace ScrumPm.Domain.Teams.Specifications
{
    public class TeamNameSearchSpecification : ISpecification<Team>
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

       
    }
}