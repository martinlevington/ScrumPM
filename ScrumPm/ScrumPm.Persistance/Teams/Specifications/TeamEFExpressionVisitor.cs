using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing;
using ScrumPm.Domain.Common.Specifications;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Teams.Specifications;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Teams.Specifications
{
    public class TeamEFExpressionVisitor  : EFExpressionVisitor<TeamEf, ITeamSpecificationVisitor, Team>, ITeamSpecificationVisitor
    {
        public override Expression<Func<TeamEf, bool>> ConvertSpecToExpression (ISpecification<Team, ITeamSpecificationVisitor> spec)
        {
            var visitor = new TeamEFExpressionVisitor ();
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

        //public void Visit (AndSpecification<Team,ITeamSpecificationVisitor>  specification) 
        //{
        //    var leftExpr = ConvertSpecToExpression (specification.Left);
        //    var rightExpr = ConvertSpecToExpression (specification.Right);

        //    var exprBody = Expression.AndAlso (leftExpr.Body, rightExpr.Body);
        //    Expr = Expression.Lambda<Func<TeamEf, bool>> (exprBody, leftExpr.Parameters.Single ());
        //}

       
    }



       
  
}
