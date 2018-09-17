using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Teams.Specifications
{
    public class TeamEFExpressionVisitor : EFExpressionVisitor<TeamEf, Team>
    {
        public override Expression<Func<TeamEf, bool>> ConvertSpecToExpression(ISpecification<Team> specification)
        {
            Visit((dynamic) specification);

            return Expr;
        }

        public void Visit(TeamNameSearchSpecification specification)
        {
            Expr = ef => EF.Functions.Like(ef.Name, "%" + specification.SearchTerm + "%");
        }

        public void Visit(TenantSpecification specification)
        {
            Expr = x => x.TenantId.ToString() == specification.TenantId.Id.ToString();
        }

        public void Visit(TeamIdSpecification specification)
        {
            Expr = ef =>
                ef.Id.ToString().Equals(specification.TeamId.Id.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}