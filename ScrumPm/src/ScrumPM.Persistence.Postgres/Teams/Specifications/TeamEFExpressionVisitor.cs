using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;

namespace ScrumPM.Persistence.Postgres.Teams.Specifications
{
    public class TeamEfExpressionVisitor  : EfExpressionVisitor<TeamEf, ITeamSpecificationVisitor, Team>, ITeamSpecificationVisitor
    {
        public override Expression<Func<TeamEf, bool>> ConvertSpecToExpression (ISpecification<Team, ITeamSpecificationVisitor> spec)
        {
            var visitor = new TeamEfExpressionVisitor ();
            spec.Accept (visitor);
            return visitor.Expr;
        }

        public void Visit (TeamNameSearchSpecification specification)
        {
            Expr = ef =>  EF.Functions.Like(ef.Name, "%"+specification.SearchTerm+"%");
        }

        public void Visit(TenantSpecification specification)
        {
            Expr = x => x.TenantId.ToString() == specification.TenantId.Id.ToString();
        }

        public void Visit(TeamIdSpecification specification)
        {
            Expr = ef => ef.Id.ToString().Equals(specification.TeamId.Id.ToString(),StringComparison.OrdinalIgnoreCase); 
        }

    }



       
  
}
